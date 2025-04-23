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
        private OrderedDictionary<string, MaterialSelectTile> materialTiles = new OrderedDictionary<string, MaterialSelectTile>();
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            var matGroups = Materials.List.Where(m => m.PixelStackerID != "AIR").GroupBy(m => m.Category)
                .Append(new FakeGrouping<Material>())
                .Reverse()
                .ToArray();

            for (int y = 0; y < matGroups.Length; y++)
            {
                var mg = matGroups[y];
                var matsInCat = mg.ToList();
                var cRef = new CategoryReferenceContainer();
                this.categoryRefs[mg.Key] = cRef;
                var tiles = mg.Select(m => new MaterialSelectTile()
                {
                    Opts = this.Options,
                    Material = m,
                    Visible = true
                }).ToList();
                cRef.Tiles = tiles;
                {
                    Padding margin = materialPanel.Padding;
                    margin.Left = 96; // tiles[0].Width * 2;
                    materialPanel.Padding = margin;
                }

                /// -----------------------
                ////// The checkbox and label
                {
                    var cbxCategory = new CheckBoxExtended();
                    cRef.Checkbox = cbxCategory;
                    cRef.Checkbox.BackColor = Color.Transparent;
                    cRef.Checkbox.Size = new Size(82, 60);
                    cRef.Checkbox.TextAlign = ContentAlignment.TopLeft;
                    cRef.Checkbox.CheckAlign = ContentAlignment.TopLeft;
                    cRef.Checkbox.MaximumSize = new Size(85, 60);
                    cRef.Checkbox.AutoSize = false;
                    cRef.Checkbox.Text = mg.Key;
                    this.materialPanel.Controls.Add(cRef.Checkbox);
                    cbxCategory.CheckedChanged += (sender, evt) =>
                    {
                        var visibleTiles = cRef.Tiles.Where(t => t.Visible && t.Material.IsEnabledF(this.Options) != cbxCategory.Checked);
                        foreach (var t in visibleTiles)
                        {
                            t.Material.IsEnabledF(this.Options, cbxCategory.Checked);
                        }
                        cRef.TilePanel.Invalidate();
                    };
                }

                ////// The flow layout
                var matList = new CustomFlowLayoutPanel();
                matList.OnCommandKey = (msg, key) => this.ProcessCmdKeyFromTileGrid(msg, key);
                matList.Controls.AddRange(tiles.ToArray());
                cRef.TilePanel = matList;
                {
                    //Padding margin = matList.Padding;
                    //margin.Bottom = 16;
                    //margin.Top = 8;
                    //matList.Padding = margin;
                }

                this.materialPanel.Controls.Add(matList);
                matList.AutoSize = true;
                matList.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                matList.Dock = DockStyle.Top;

                foreach (var m in tiles)
                {
                    this.materialTiles[m.Material.PixelStackerID] = m;
                    this.materialTiles[m.Material.PixelStackerID].MouseEnter += this.OnMouseEnter_Tile;
                    this.materialTiles[m.Material.PixelStackerID].MouseLeave += this.OnMouseLeave_Tile;
                    this.materialTiles[m.Material.PixelStackerID].MouseClick += this.OnMouseClick_Tile;
                }
            }

            this.materialPanel.ResumeLayout();
            RepositionCheckboxes();
        }

        /// <summary>
        /// This ALREADY makes the call to SetVisibleMaterials at the end.
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

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            SetVisibleMaterials(Materials.List ?? new List<Material>());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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
                    if (match.Success)
                    {
                        return match.Groups[1].Value;
                    }
                    else
                    {
                        return blockIdAndNBT;
                    }
                }).Distinct().ToArray()
            );
        }

        #endregion Constructor Stuff

        #region Mouse Listeners
        // *-----------------+--------------------------------------------------------------------*
        // *                   M O U S E _ L I S T E N E R S                                      *
        // *-----------------+--------------------------------------------------------------------*
        private void OnMouseLeave_Tile(object sender, EventArgs e)
        {
            if (this.lblInfo.Text != "")
            {
                this.lblInfo.Text = "";
            }
        }

        private void OnMouseEnter_Tile(object sender, EventArgs e)
        {
            this.lblInfo.Text = ((MaterialSelectTile)sender).Material.Label;
        }

        private PixelStacker.WF.Components.MaterialSelectTile previouslyClickedTile = null;
        private void OnMouseClick_Tile(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var currentTile = (PixelStacker.WF.Components.MaterialSelectTile)sender;
                if (!Control.ModifierKeys.HasFlag(Keys.Shift) || previouslyClickedTile == null)
                {
                    previouslyClickedTile = currentTile;
                }
                else
                {
                    string categoryKey = this.AreMaterialsCombined ? "*" : this.previouslyClickedTile.Material.Category;
                    var cRef = this.categoryRefs[categoryKey];

                    // go time.
                    int idxA = cRef.TilePanel.Controls.IndexOf(previouslyClickedTile);
                    int idxB = cRef.TilePanel.Controls.IndexOf(currentTile);

                    // Maybe going across different categories.
                    if (idxA == -1 || idxB == -1) return;


                    int minI = Math.Min(idxA, idxB);
                    int maxI = Math.Max(idxA, idxB);
                    cRef.TilePanel.Controls.OfType<PixelStacker.WF.Components.MaterialSelectTile>()
                        .Skip(minI).Take(maxI - minI).ToList()
                        .ForEach(x =>
                        {
                            x.Material.IsEnabledF(this.Options, !Control.ModifierKeys.HasFlag(Keys.Control));
                            x.Invalidate();
                        });

                    RefreshCheckboxStates();
                }
            }
        }
        #endregion Mouse Listeners

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
                var matsInCat = kvp.Value.Tiles;
                var numEnabled = matsInCat.Count(x => x.Material.IsEnabledF(this.Options));
                var numTotal = matsInCat.Count;
                var newCbxState = (numEnabled > numTotal / 2) ? CheckState.Checked : CheckState.Unchecked;
                kvp.Value.Checkbox.SetCheckStateWithoutRaisingEvents(newCbxState);
            }
        }

        /// <summary>
        /// We tried every type of layout available, and each of them was a bit buggy.
        /// We had 6 devs try to get it to work. Finally we decided to just manually
        /// reposition the checkboxes. It works. Checkboxes will be aligned to the
        /// left of the grid area they represent.
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

            this.materialTiles.ToList().ForEach(x =>
            {
                x.Value.Invalidate();
            });
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

            tbxMaterialFilter.Text = "enabled";
            await SetSearchFilter("enabled", CancellationToken.None);
        }

        private void btnEditColorProfiles_Click(object sender, EventArgs e)
        {
            this.dlgSave.InitialDirectory = FilePaths.ColorProfilesPath;
            var result = this.dlgSave.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                //Prompt
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

        private KeyValuePair<string, CategoryReferenceContainer> GetCategoryRefForMaterialTile(MaterialSelectTile currentTile)
        {
            return this.categoryRefs.FirstOrDefault(x => x.Value.Tiles.Contains(currentTile));
        }

        // *-----------------+--------------------------------------------------------------------*
        // *                   S E A R C H                                                        *
        // *-----------------+--------------------------------------------------------------------*
        private DelayThrottle searchLimiter = new DelayThrottle(TimeSpan.FromMilliseconds(400));
        private void ddlMaterialSearch_TextChanged(object sender, EventArgs e)
        {
            var start = DateTime.Now;
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

            int i = 0;
            var visibleSet = mats.ToDictionary(k => k.PixelStackerID, v => i++);

            foreach (var kvp in categoryRefs)
            {
                if (_worker?.SafeIsCancellationRequested() == true)
                {
                    materialPanel.ResumeLayout();
                    materialPanel.ResumeDrawing();
                    _worker?.SafeThrowIfCancellationRequested();
                    await Task.Yield();
                }

                var tilePanel = kvp.Value.TilePanel;
                tilePanel.SuspendDrawing();
                tilePanel.SuspendLayout();

                bool hasVisibleTile = false;

                foreach (var tile in kvp.Value.Tiles)
                {
                    var key = tile.Material.PixelStackerID;
                    var mat = tile.Material;
                    var opts = this.Options;
                    bool b1 = mat.IsVisibleF(opts);
                    bool b2 = visibleSet.ContainsKey(key);
                    bool shouldBeVisible = mat.IsVisibleF(this.Options) && visibleSet.ContainsKey(key);
                    if (tile.Visible != shouldBeVisible)
                    {
                        tile.Visible = shouldBeVisible;
                        //tile.Invalidate(); // force redraw
                    }

                    if (shouldBeVisible)
                        hasVisibleTile = true;
                }

                kvp.Value.Checkbox.Visible = hasVisibleTile;
                tilePanel.Visible = hasVisibleTile;

                tilePanel.ResumeLayout();
                tilePanel.ResumeDrawing();
            }

            materialPanel.ResumeLayout();
            materialPanel.ResumeDrawing();

            RefreshCheckboxStates();
            RepositionCheckboxes();
            this.Refresh();
        }


        private void SetMaterialsCombinedMode(bool shouldBeCombined)
        {
            this.materialPanel.SuspendLayout();
            var targetRef = categoryRefs["*"];

            if (AreMaterialsCombined && !shouldBeCombined)
            {
                // Switch to SPLIT mode
                AreMaterialsCombined = shouldBeCombined;
                // First we REMOVE the combined element.
                this.materialPanel.Controls.Remove(targetRef.Checkbox);
                this.materialPanel.Controls.Remove(targetRef.TilePanel);
                targetRef.TilePanel.ClearControlsQuick();

                // Then we populate the sub groups
                foreach (var tileGroup in targetRef.Tiles.GroupBy(tr => tr.Material.Category))
                {
                    var cRef = this.categoryRefs[tileGroup.Key];
                    cRef.Tiles.AddRange(tileGroup);
                    cRef.TilePanel.AddControlsQuick(tileGroup.ToArray());
                    this.materialPanel.Controls.Add(cRef.Checkbox);
                    this.materialPanel.Controls.Add(cRef.TilePanel);
                }
                targetRef.Tiles.Clear();
            }
            else if (!AreMaterialsCombined && shouldBeCombined)
            {
                targetRef.Tiles.Clear();
                targetRef.TilePanel.ClearControlsQuick();

                // Switch to combined mode
                AreMaterialsCombined = shouldBeCombined;
                foreach (var cRefKvp in categoryRefs)
                {
                    var cRef = cRefKvp.Value;
                    if (cRefKvp.Key != "*")
                    {
                        this.materialPanel.Controls.Remove(cRef.Checkbox);
                        this.materialPanel.Controls.Remove(cRef.TilePanel);
                        targetRef.Tiles.AddRange(cRef.Tiles);
                        cRef.Tiles.Clear();
                        cRef.TilePanel.ClearControlsQuick();
                    }
                }

                // Lastly we CONSTRUCT the combined element.
                this.materialPanel.Controls.Add(targetRef.Checkbox);
                this.materialPanel.Controls.Add(targetRef.TilePanel);
                targetRef.TilePanel.AddControlsQuick(targetRef.Tiles.ToArray());
            }

            this.materialPanel.ResumeLayout();
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
            }

            var foo = Materials.List.SelectMany(x => x.Tags).Distinct().ToArray();

            var newList = Materials.List.Where(x =>
            {
                if (!x.IsVisibleF(this.Options)) return false;

                if (needle == "on" || needle == "enabled" || needle == "active")
                {
                    return x.IsEnabledF(this.Options);
                }

                if (needle == "off" || needle == "disabled" || needle == "inactive")
                {
                    return !x.IsEnabledF(this.Options);
                }

                if (string.IsNullOrWhiteSpace(needle)) return true;
                if (x.Label.StartsWithOrContains(needle, 3)) return true;
                if (x.MinimumSupportedMinecraftVersion.StartsWithOrContains(needle, 3)) return true;
                if (x.Category.StartsWithOrContains(needle, 2)) return true;
                if (needle.Length > 1 && x.Tags.Any(t => t.ToLowerInvariant().StartsWith(needle))) return true;

                string blockIdAndNBT = x.GetBlockNameAndData(false).ToLowerInvariant();
                var match = regexMatName.Match(blockIdAndNBT);
                if (match.Success)
                {
                    if (match.Groups[1].Value.StartsWithOrContains(needle, 3))
                    {
                        return true;
                    }
                }

                return false;
            }).ToList();

            _worker?.SafeThrowIfCancellationRequested();
            await Task.Yield();


            int cnt = newList.Count;
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                var task = this.TryHideAsync();
                task.Wait();
                return true;
            }

            KonamiWatcher.ProcessKey(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected bool ProcessCmdKeyFromTileGrid(Message msg, Keys keyData)
        {
            if (keyData.HasFlag(Keys.A) && keyData.HasFlag(Keys.Control))
            {
                this.materialTiles.Where(kvp => kvp.Value.Visible).ToList().ForEach(kvp =>
                {
                    kvp.Value.Material.IsEnabledF(this.Options, true);
                    kvp.Value.Invalidate();
                });

                RefreshCheckboxStates();

                return true;
            }

            if (keyData.HasFlag(Keys.D) && keyData.HasFlag(Keys.Control))
            {
                this.materialTiles.Where(kvp => kvp.Value.Visible).ToList().ForEach(kvp =>
                {
                    kvp.Value.Material.IsEnabledF(this.Options, false);
                    kvp.Value.Invalidate();
                });
                RefreshCheckboxStates();
                return true;
            }

            return this.ProcessCmdKey(ref msg, keyData);
        }

        private class CategoryReferenceContainer
        {
            public CheckBoxExtended Checkbox { get; set; }
            public List<MaterialSelectTile> Tiles { get; set; } = new List<MaterialSelectTile>();
            public CustomFlowLayoutPanel TilePanel { get; internal set; }
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
