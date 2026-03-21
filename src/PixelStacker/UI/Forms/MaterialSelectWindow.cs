using Newtonsoft.Json;
using PixelStacker.Extensions;
using PixelStacker.IO;
using PixelStacker.Logic.Collections;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using PixelStacker.Resources.Localization;
using PixelStacker.UI.Controls;
using PixelStacker.WF.Components;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    [Serializable]
    // TODO: Fix weird display when going to narrow viewing windows
    public partial class MaterialSelectWindow : Form, ILocalized
    {
        #region Constructor Stuff
        // *-----------------+--------------------------------------------------------------------*
        // *                   C O N S T R U C T I O N                                            *
        // *-----------------+--------------------------------------------------------------------*
        private Regex regexMatName = new Regex(@"minecraft:([a-zA-Z_09]+)(.*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private Dictionary<string, CategoryReferenceContainer> categoryRefs = new Dictionary<string, CategoryReferenceContainer>();
        public Action<CancellationToken> OnColorPaletteRecompileRequested;
        private bool AreMaterialsCombined = false;
        private Options Options { get; }

        [Obsolete("Only use in design view", false)]
        public MaterialSelectWindow() : this(Options.GetInMemoryFallback)
        {
        }

        public MaterialSelectWindow(Options opts)
        {
            this.Options = opts;
            InitializeComponent();
            ApplyLocalization();
            this.InitializeAutoComplete();
            this.InitializeMaterialTiles();
        }

        /// <summary>
        /// Called when form is first loaded as well as any time it is brought back.
        /// </summary>
        private void MaterialSelectWindow_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                LoadFromSettings();
                RepositionCheckboxes();
                RefreshCheckboxStates();
            }
        }

        private void InitializeMaterialTiles()
        {
            this.materialPanel.SuspendLayout();
            this.materialPanel.SuspendDrawing();

            // Set left padding once for the checkbox overlay area
            var padding = materialPanel.Padding;
            padding.Left = 96;
            materialPanel.Padding = padding;

            var matGroups = Materials.List.Where(m => m.PixelStackerID != "AIR")
                .GroupBy(m => m.Category)
                .Reverse()
                .ToArray();

            foreach (var mg in matGroups)
            {
                CreateCategoryRef(mg.Key, mg.ToList(), visible: true);
            }

            // The "*" combined panel contains all materials; hidden until search is active
            var allMats = Materials.List.Where(m => m.PixelStackerID != "AIR").ToList();
            CreateCategoryRef("*", allMats, visible: false);

            this.materialPanel.ResumeLayout();
            this.materialPanel.ResumeDrawing();
            RepositionCheckboxes();
        }

        private void CreateCategoryRef(string key, List<Material> mats, bool visible)
        {
            var cRef = new CategoryReferenceContainer();
            cRef.Tiles = mats;
            this.categoryRefs[key] = cRef;

            var cbxCategory = new CheckBoxExtended();
            cRef.Checkbox = cbxCategory;
            cbxCategory.BackColor = Color.Transparent;
            cbxCategory.Size = new Size(82, 60);
            cbxCategory.TextAlign = ContentAlignment.TopLeft;
            cbxCategory.CheckAlign = ContentAlignment.TopLeft;
            cbxCategory.MaximumSize = new Size(85, 60);
            cbxCategory.AutoSize = false;
            cbxCategory.Text = key;
            cbxCategory.Visible = visible;
            this.materialPanel.Controls.Add(cbxCategory);
            cbxCategory.CheckedChanged += (sender, evt) =>
            {
                foreach (var m in cRef.TilePanel.VisibleMaterials)
                {
                    if (m.IsEnabledF(this.Options) != cbxCategory.Checked)
                        m.IsEnabledF(this.Options, cbxCategory.Checked);
                }
                cRef.TilePanel.Invalidate();
            };

            var gridPanel = new MaterialCategoryGridPanel(this.Options);
            gridPanel.SetVisibleMaterials(mats);
            gridPanel.Dock = DockStyle.Top;
            gridPanel.Visible = visible;
            gridPanel.MaterialClicked += (s, mat) => RefreshCheckboxStates();
            gridPanel.MaterialHovered += (s, mat) => { lblInfo.Text = mat?.Label ?? ""; };
            cRef.TilePanel = gridPanel;
            this.materialPanel.Controls.Add(gridPanel);
        }

        /// <summary>
        /// Called when form is first loaded as well as any time it is brought back.
        /// </summary>
        private void LoadFromSettings()
        {
            cbxIsMultiLayer.Checked = this.Options.IsMultiLayer;
            cbxIsSideView.Checked = this.Options.IsSideView;
            cbxRequire2ndLayer.Checked = this.Options.IsMultiLayerRequired;
            var dirColorProfiles = new DirectoryInfo(FilePaths.ColorProfilesPath);
            if (dirColorProfiles.Exists)
            {
                var fis = dirColorProfiles.GetFiles();
                this.ddlColorProfile.Items.Clear();

                this.ddlColorProfile.Text = Resources.Text.DDL_SelectProfile;
                this.ddlColorProfile.Items.AddRange(fis.Select(x => x.Name).ToArray());
            }

#pragma warning disable CS4014
            SetVisibleMaterials(Materials.List ?? new List<Material>());
#pragma warning restore CS4014
        }

        private void InitializeAutoComplete()
        {
            tbxMaterialFilter.AutoCompleteCustomSource.Clear();

            tbxMaterialFilter.AutoCompleteCustomSource.AddRange(
                Materials.List.SelectMany(x => x.Tags).Distinct().ToArray()
            );

            tbxMaterialFilter.AutoCompleteCustomSource.AddRange(
                Materials.List.Select(x => x.Category.ToLowerInvariant()).Distinct().ToArray()
            );

            tbxMaterialFilter.AutoCompleteCustomSource.AddRange(
                Materials.List.Select(x => x.Label.ToLowerInvariant()).Distinct().ToArray()
            );

            tbxMaterialFilter.AutoCompleteCustomSource.AddRange(
                Materials.List.Select(x =>
                {
                    string blockIdAndNBT = x.GetBlockNameAndData(false).ToLowerInvariant();
                    var match = regexMatName.Match(blockIdAndNBT);
                    return match.Success ? match.Groups[1].Value : blockIdAndNBT;
                }).Distinct().ToArray()
            );
        }

        #endregion Constructor Stuff

        #region Checkboxes
        // *-----------------+--------------------------------------------------------------------*
        // *                   C H E C K B O X E S                                                *
        // *-----------------+--------------------------------------------------------------------*
        /// <summary>
        /// For each category, checked = (numChecked > numTotal/2).
        /// </summary>
        private void RefreshCheckboxStates()
        {
            RateLimit.Check(3, 2000);
            foreach (var kvp in this.categoryRefs)
            {
                var mats = kvp.Value.Tiles;
                var numEnabled = mats.Count(x => x.IsEnabledF(this.Options));
                var numTotal = mats.Count;
                var newCbxState = (numEnabled > numTotal / 2) ? CheckState.Checked : CheckState.Unchecked;
                kvp.Value.Checkbox.SetCheckStateWithoutRaisingEvents(newCbxState);
            }
        }

        /// <summary>
        /// Manually reposition the checkboxes to align with their grid panels.
        /// </summary>
        private void RepositionCheckboxes()
        {
            foreach (var cRef in this.categoryRefs.Values)
            {
                cRef.Checkbox.Location = new Point(10, 10 + cRef.TilePanel.Location.Y);
            }
        }
        #endregion Checkboxes

        #region Form visibility
        // *-----------------+--------------------------------------------------------------------*
        // *                   F O R M _ V I S I B I L I T Y                                      *
        // *-----------------+--------------------------------------------------------------------*
        protected async Task TryHideAsync()
        {
            if (this.Options.IsMultiLayerRequired)
            {
                if (!Materials.List.Any(x => x.IsEnabledF(this.Options) && x.PixelStackerID != "AIR" && x.CanBeUsedAsTopLayer && x.IsVisibleF(this.Options)))
                {
                    MessageBox.Show(
                        text: Resources.Text.Error_GlassRequiredForMultiLayer,
                        caption: Resources.Text.Error_SomethingIsWrong,
                        buttons: MessageBoxButtons.OK,
                        icon: MessageBoxIcon.Error
                        );

                    return;
                }
            }

            if (!Materials.List.Any(x => x.IsEnabledF(this.Options) && x.PixelStackerID != "AIR" && x.IsVisibleF(this.Options)))
            {
                MessageBox.Show(
                    text: Resources.Text.Error_OneMaterialRequired,
                    caption: Resources.Text.Error_SomethingIsWrong,
                    buttons: MessageBoxButtons.OK,
                    icon: MessageBoxIcon.Error
                    );

                return;
            }

            if (!Materials.List.Any(x => x.IsEnabledF(this.Options) && x.CanBeUsedAsBottomLayer && x.IsVisibleF(this.Options)))
            {
                MessageBox.Show(
                    text: Resources.Text.Error_NonGlassRequired,
                    caption: Resources.Text.Error_SomethingIsWrong,
                    buttons: MessageBoxButtons.OK,
                    icon: MessageBoxIcon.Error
                    );

                return;
            }

            Options.Save();

            if (OnColorPaletteRecompileRequested != null)
            {
                await Task.Run(() => TaskManager.Get.StartAsync(OnColorPaletteRecompileRequested));
            }

            this.Hide();
        }

        private async void MaterialSelectWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                await this.TryHideAsync();
            }
        }
        #endregion Form visibility

        #region Controls on top.
        // *-----------------+--------------------------------------------------------------------*
        // *                   C O N T R O L S _ O N _ T O P                                      *
        // *-----------------+--------------------------------------------------------------------*
        private void cbxIsMultiLayer_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbx = (CheckBox)sender;
            bool isChecked = cbx.CheckState == CheckState.Checked;
            this.Options.IsMultiLayer = isChecked;
            if (isChecked)
            {
                this.Options.IsMultiLayer = true;
            }
            else
            {
                cbxRequire2ndLayer.Checked = false;
                this.Options.IsMultiLayerRequired = false;
                this.Options.IsMultiLayer = false;
            }
        }

        private void cbxRequire2ndLayer_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbx = (CheckBox)sender;
            bool isChecked = cbx.CheckState == CheckState.Checked;
            if (isChecked)
            {
                cbxIsMultiLayer.Checked = true;
                this.Options.IsMultiLayer = true;
                this.Options.IsMultiLayerRequired = true;
            }
            else
            {
                this.Options.IsMultiLayerRequired = false;
            }
        }

        private void cbxIsSideView_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbx = (CheckBox)sender;
            bool isChecked = cbx.CheckState == CheckState.Checked;
            this.Options.IsSideView = isChecked;

            foreach (var cRef in categoryRefs.Values)
                cRef.TilePanel.InvalidateBitmapCache();
        }

        private async void ddlColorProfile_SelectedValueChanged(object sender, EventArgs e)
        {
            string item = (string)ddlColorProfile.SelectedItem;
            string path = Path.Combine(FilePaths.ColorProfilesPath, item);

            if (File.Exists(path))
            {
                var existingProfile = JsonConvert.DeserializeObject<ColorProfile>(File.ReadAllText(path));
                if (existingProfile != null)
                {
                    foreach (var mat in existingProfile.Materials)
                    {
                        var material = Materials.FromPixelStackerID(mat.Key);
                        material?.IsEnabledF(this.Options, mat.Value);
                    }
                }
            }
            await SetSearchFilter(tbxMaterialFilter.Text, CancellationToken.None);
        }

        private void btnEditColorProfiles_Click(object sender, EventArgs e)
        {
            this.dlgSave.InitialDirectory = FilePaths.ColorProfilesPath;
            var result = this.dlgSave.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                string profileLabel = null;
                string path = this.dlgSave.FileName;

                if (File.Exists(path))
                {
                    try
                    {
                        var existingProfile = JsonConvert.DeserializeObject<ColorProfile>(File.ReadAllText(path));
                        if (existingProfile != null)
                        {
                            profileLabel = existingProfile.Label;
                        }
                    }
                    catch (Exception)
                    { }
                }

                if (string.IsNullOrWhiteSpace(profileLabel))
                {
                    profileLabel = new FileInfo(this.dlgSave.FileName).Name;
                }

                var profile = new ColorProfile()
                {
                    Label = profileLabel,
                    Materials = Materials.List.ToDictionary(k => k.PixelStackerID, v => v.IsEnabledF(this.Options))
                };

                string json = JsonConvert.SerializeObject(profile, Formatting.Indented);
                File.WriteAllText(path, json);

                var dirColorProfiles = new DirectoryInfo(FilePaths.ColorProfilesPath);
                if (dirColorProfiles.Exists)
                {
                    var fis = dirColorProfiles.GetFiles();
                    this.ddlColorProfile.Items.Clear();
                    this.ddlColorProfile.Items.AddRange(fis.Select(x => x.Name).ToArray());
                    this.ddlColorProfile.SelectedItem = new FileInfo(this.dlgSave.FileName).Name;
                }
            }
        }

        #endregion Controls on top.

        // *-----------------+--------------------------------------------------------------------*
        // *                   S E A R C H                                                        *
        // *-----------------+--------------------------------------------------------------------*
        private DelayThrottle searchLimiter = new DelayThrottle(TimeSpan.FromMilliseconds(400));
        private void ddlMaterialSearch_TextChanged(object sender, EventArgs e)
        {
            string needle = this.tbxMaterialFilter.Text.ToLowerInvariant();

            Task.Run((async () => {
                if (!(await this.searchLimiter.CanWaitEntireDelayWithoutInteruptions()))
                {
                    return;
                }

                await this.InvokeEx((async (c) =>
                {
                    await c.SetSearchFilter(needle);
                }));
            }));
        }

        public async Task SetVisibleMaterials(List<Material> mats, CancellationToken? _worker = null)
        {
            materialPanel.SuspendDrawing();
            materialPanel.SuspendLayout();

            var visibleSet = new HashSet<string>(mats.Select(m => m.PixelStackerID));

            if (AreMaterialsCombined)
            {
                var cRef = categoryRefs["*"];
                var visible = cRef.Tiles.Where(m => visibleSet.Contains(m.PixelStackerID)).ToList();
                cRef.TilePanel.SetVisibleMaterials(visible);
                bool hasAny = visible.Count > 0;
                cRef.TilePanel.Visible = hasAny;
                cRef.Checkbox.Visible = hasAny;
            }
            else
            {
                foreach (var kvp in categoryRefs)
                {
                    if (kvp.Key == "*") continue;
                    var cRef = kvp.Value;
                    var visible = cRef.Tiles
                        .Where(m => m.IsVisibleF(Options) && visibleSet.Contains(m.PixelStackerID))
                        .ToList();
                    cRef.TilePanel.SetVisibleMaterials(visible);
                    bool hasAny = visible.Count > 0;
                    cRef.TilePanel.Visible = hasAny;
                    cRef.Checkbox.Visible = hasAny;
                }
            }

            materialPanel.ResumeLayout();
            materialPanel.ResumeDrawing();

            RefreshCheckboxStates();
            RepositionCheckboxes();
        }

        private void SetMaterialsCombinedMode(bool shouldBeCombined)
        {
            if (AreMaterialsCombined == shouldBeCombined) return;
            AreMaterialsCombined = shouldBeCombined;

            materialPanel.SuspendLayout();
            materialPanel.SuspendDrawing();

            foreach (var kvp in categoryRefs)
            {
                bool isStar = kvp.Key == "*";
                bool show = shouldBeCombined ? isStar : !isStar;
                kvp.Value.TilePanel.Visible = show;
                kvp.Value.Checkbox.Visible = show;
            }

            materialPanel.ResumeLayout();
            materialPanel.ResumeDrawing();
            RepositionCheckboxes();
        }

        private async Task SetSearchFilter(string needle, CancellationToken? _worker = null)
        {
            this.materialPanel.SuspendDrawing();
            SetMaterialsCombinedMode(needle.Length > 0);
            _worker?.SafeThrowIfCancellationRequested();
            await Task.Yield();

            needle = needle.ToLowerInvariant();
            bool isv = this.Options.IsSideView;

            if (needle.StartsWith("#"))
            {
                SKColor? cNeedle = needle.ToSKColor();
                if (cNeedle == null) return;

                var found = Materials.List
                    .Where(x => x.IsVisibleF(this.Options))
                    .OrderBy(m => m.GetAverageColor(isv).GetColorDistanceSquared(cNeedle.Value))
                    .Take(20).ToList();

                await SetVisibleMaterials(found, _worker);
                this.materialPanel.ResumeDrawing();
                return;
            }

            var newList = Materials.List.Where(x =>
            {
                if (!x.IsVisibleF(this.Options)) return false;

                if (needle == "on" || needle == "enabled" || needle == "active")
                    return x.IsEnabledF(this.Options);

                if (needle == "off" || needle == "disabled" || needle == "inactive")
                    return !x.IsEnabledF(this.Options);

                if (string.IsNullOrWhiteSpace(needle)) return true;
                if (x.Label.StartsWithOrContains(needle, 3)) return true;
                if (x.MinimumSupportedMinecraftVersion.StartsWithOrContains(needle, 3)) return true;
                if (x.Category.StartsWithOrContains(needle, 2)) return true;
                if (needle.Length > 1 && x.Tags.Any(t => t.ToLowerInvariant().StartsWith(needle))) return true;

                string blockIdAndNBT = x.GetBlockNameAndData(false).ToLowerInvariant();
                var match = regexMatName.Match(blockIdAndNBT);
                if (match.Success && match.Groups[1].Value.StartsWithOrContains(needle, 3))
                    return true;

                return false;
            }).ToList();

            _worker?.SafeThrowIfCancellationRequested();
            await Task.Yield();

            await SetVisibleMaterials(newList, _worker);
            this.materialPanel.ResumeDrawing();
        }

        #region OTHER
        // *-----------------+--------------------------------------------------------------------*
        // *                   O T H E R                                                          *
        // *-----------------+--------------------------------------------------------------------*
        public void ApplyLocalization()
        {
            this.Text = Resources.Text.MaterialSelect_Title;
            lblColorProfile.Text = Resources.Text.MaterialSelect_ColorProfile;
            lblFilter.Text = Resources.Text.Action_Filter;
            btnEditColorProfiles.Text = Resources.Text.Action_Save;
            cbxIsMultiLayer.Text = Resources.Text.MaterialSelect_IsMultiLayer;
            cbxIsSideView.Text = Resources.Text.Orientation_Vertical;
            cbxRequire2ndLayer.Text = Resources.Text.MaterialSelect_IsMultiLayerRequired;
        }

        private void MaterialSelectWindow_Resize(object sender, EventArgs e)
            => RepositionCheckboxes();

        protected override void OnResizeBegin(EventArgs e)
        {
            base.OnResizeBegin(e);
            materialPanel.SuspendDrawing();
            materialPanel.SuspendLayout();
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            materialPanel.ResumeLayout();
            materialPanel.ResumeDrawing();
            RepositionCheckboxes();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                var task = this.TryHideAsync();
                return true;
            }

            KonamiWatcher.ProcessKey(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected bool ProcessCmdKeyFromTileGrid(Message msg, Keys keyData)
        {
            if (keyData.HasFlag(Keys.A) && keyData.HasFlag(Keys.Control))
            {
                foreach (var kvp in categoryRefs)
                {
                    if (!kvp.Value.TilePanel.Visible) continue;
                    foreach (var mat in kvp.Value.TilePanel.VisibleMaterials)
                        mat.IsEnabledF(this.Options, true);
                    kvp.Value.TilePanel.Invalidate();
                }
                RefreshCheckboxStates();
                return true;
            }

            if (keyData.HasFlag(Keys.D) && keyData.HasFlag(Keys.Control))
            {
                foreach (var kvp in categoryRefs)
                {
                    if (!kvp.Value.TilePanel.Visible) continue;
                    foreach (var mat in kvp.Value.TilePanel.VisibleMaterials)
                        mat.IsEnabledF(this.Options, false);
                    kvp.Value.TilePanel.Invalidate();
                }
                RefreshCheckboxStates();
                return true;
            }

            return this.ProcessCmdKey(ref msg, keyData);
        }

        private class CategoryReferenceContainer
        {
            public CheckBoxExtended Checkbox { get; set; }
            public List<Material> Tiles { get; set; } = new List<Material>();
            public MaterialCategoryGridPanel TilePanel { get; internal set; }
        }

        private class FakeGrouping<V> : IGrouping<string, V>
        {
            public string Key => "*";
            private List<V> Items = new List<V>();

            public IEnumerator<V> GetEnumerator()
            {
                return Items.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return Items.GetEnumerator();
            }
        }

        private class CheckBoxExtended : CheckBox
        {
            private bool IsCheckEventEnabled = true;

            protected override void OnCheckedChanged(EventArgs e)
            {
                if (IsCheckEventEnabled)
                    base.OnCheckedChanged(e);
            }

            protected override void OnCheckStateChanged(EventArgs e)
            {
                if (IsCheckEventEnabled)
                    base.OnCheckStateChanged(e);
            }

            public void SetCheckStateWithoutRaisingEvents(CheckState s)
            {
                this.IsCheckEventEnabled = false;
                this.CheckState = s;
                this.IsCheckEventEnabled = true;
            }

            public void SetCheckStateWithoutRaisingEvents(bool s)
            {
                this.IsCheckEventEnabled = false;
                this.Checked = s;
                this.IsCheckEventEnabled = true;
            }
        }
        #endregion OTHER
    }
}
