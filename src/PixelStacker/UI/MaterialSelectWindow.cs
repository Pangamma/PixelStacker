using Newtonsoft.Json;
using PixelStacker.Components;
using PixelStacker.Logic;
using PixelStacker.Logic.Collections;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO;
using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    [Serializable]
    // TODO: Fix weird display when going to narrow viewing windows
    public partial class MaterialSelectWindow : Form, ILocalized
    {
        private Regex regexMatName = new Regex(@"minecraft:([a-zA-Z_09]+)(.*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private OrderedDictionary<string, MaterialSelectTile> materialTiles = new OrderedDictionary<string, MaterialSelectTile>();
        private Dictionary<string, CategoryReferenceContainer> categoryRefs = new Dictionary<string, CategoryReferenceContainer>();

        public MaterialSelectWindow()
        {
            InitializeComponent();
            ApplyLocalization(System.Threading.Thread.CurrentThread.CurrentUICulture);

            bool isv = Options.Get.IsSideView;
            this.InitializeAutoComplete();
            this.InitializeMaterialTiles();
            this.LoadFromSettings();
        }

        private class CategoryReferenceContainer
        {
            public CheckBox Checkbox { get; set; }
            public List<MaterialSelectTile> Tiles { get; set; } = new List<MaterialSelectTile>();
            public CustomFlowLayoutPanel TilePanel { get; internal set; }

            public void SetVisible(bool b)
            {
                this.Checkbox.Visible = true;
            }
        }

        /**
         * I need references to:
         * - the checkboxes
         * - the "check-all" checkbox
         * 
         */
        private void InitializeMaterialTiles()
        {
            this.materialPanel.SuspendLayout();
            var matGroups = Materials.List.Where(m => m.PixelStackerID != "AIR").GroupBy(m => m.Category)
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
                    Material = m,
                    Visible = true
                }).ToList();
                cRef.Tiles = tiles;
                {
                    Padding margin = materialPanel.Padding;
                    margin.Left = tiles[0].Width * 2;
                    materialPanel.Padding = margin;
                }

                /// -----------------------
                ////// The checkbox and label
                {
                    var cbxCategory = new CheckBox();
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
                        cRef.Tiles.ForEach(t => t.Material.IsEnabled = cbxCategory.Checked);
                        cRef.TilePanel.Refresh();
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
                    this.materialTiles[GetMaterialTileID(m, mg.Key)] = m;
                    this.materialTiles[GetMaterialTileID(m, mg.Key)].MouseEnter += this.OnMouseEnter_Tile;
                    this.materialTiles[GetMaterialTileID(m, mg.Key)].MouseLeave += this.OnMouseLeave_Tile;
                    this.materialTiles[GetMaterialTileID(m, mg.Key)].MouseClick += this.OnMouseClick_Tile;
                }
            }

            this.materialPanel.ResumeLayout();
            RepositionCheckboxes();
        }

        private void LoadFromSettings()
        {
            cbxIsMultiLayer.Checked = Options.Get.IsMultiLayer;
            cbxIsSideView.Checked = Options.Get.IsSideView;
            cbxRequire2ndLayer.Checked = Options.Get.IsMultiLayerRequired;
            var dirColorProfiles = new DirectoryInfo(FilePaths.ColorProfilesPath);
            if (dirColorProfiles.Exists)
            {
                var fis = dirColorProfiles.GetFiles();
                this.ddlColorProfile.Items.Clear();

                this.ddlColorProfile.Text = Resources.Text.DDL_SelectProfile;
                this.ddlColorProfile.Items.AddRange(fis.Select(x => x.Name).ToArray());
            }

            SetVisibleMaterials(Materials.List ?? new List<Material>());
        }

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

        private MaterialSelectTile previouslyClickedTile = null;

        private KeyValuePair<string, CategoryReferenceContainer> GetCategoryRefForMaterialTile(MaterialSelectTile currentTile)
        {
            return this.categoryRefs.FirstOrDefault(x => x.Value.Tiles.Contains(currentTile));
        }

        [Obsolete("Should do this a different way and be more efficient.", false)]
        private string GetMaterialTileID(MaterialSelectTile tile, string category = null)
        {
            //if (category == null)
            //{
            //    category = this.GetCategoryRefForMaterialTile(tile).Key ?? null;
            //}

            return $"{tile.Material.PixelStackerID}";
        }

        private void OnMouseClick_Tile(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var currentTile = (MaterialSelectTile)sender;
                if (!Control.ModifierKeys.HasFlag(Keys.Shift) || previouslyClickedTile == null)
                {
                    previouslyClickedTile = currentTile;
                }
                else
                {
                    // go time.
                    int idxA = this.materialTiles.IndexOf(GetMaterialTileID(previouslyClickedTile));
                    int idxB = this.materialTiles.IndexOf(GetMaterialTileID(currentTile));
                    int minI = Math.Min(idxA, idxB);
                    int maxI = Math.Max(idxA, idxB);
                    this.materialTiles.Skip(minI).Take(maxI - minI).ToList()
                        .ForEach(x =>
                        {
                            x.Value.Material.IsEnabled = !Control.ModifierKeys.HasFlag(Keys.Control);
                            x.Value.Refresh();
                        });

                    RefreshCheckboxStates();
                }
            }
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

        protected async void TryHide()
        {
            if (Options.Get.IsMultiLayerRequired)
            {
                if (!Materials.List.Any(x => x.IsEnabled && x.PixelStackerID != "AIR" && x.Category == "Glass"))
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

            if (!Materials.List.Any(x => x.IsEnabled && x.PixelStackerID != "AIR"))
            {
                MessageBox.Show(
                    text: Resources.Text.Error_OneMaterialRequired,
                    caption: Resources.Text.Error_SomethingIsWrong,
                    buttons: MessageBoxButtons.OK,
                    icon: MessageBoxIcon.Error
                    );

                return;
            }

            if (!Materials.List.Any(x => x.IsEnabled && x.Category != "Glass" && x.PixelStackerID != "AIR"))
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
            await TaskManager.Get.StartAsync((token) =>
            {
                ColorMatcher.Get.CompileColorPalette(token, true, Materials.List)
                .GetAwaiter().GetResult();
            });

            this.Hide();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.TryHide();
                return true;
            }

            MainForm.Self.konamiWatcher.ProcessKey(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void RefreshCheckboxStates()
        {
            foreach (var kvp in this.categoryRefs)
            {
                var matsInCat = kvp.Value.Tiles;
                var numEnabled = matsInCat.Count(x => x.Material.IsEnabled);
                var numTotal = matsInCat.Count;

                kvp.Value.Checkbox.CheckState = (numEnabled > numTotal / 2)
                ? CheckState.Checked
                : CheckState.Unchecked;
            }
        }

        protected bool ProcessCmdKeyFromTileGrid(Message msg, Keys keyData)
        {
            if (keyData.HasFlag(Keys.A) && keyData.HasFlag(Keys.Control))
            {
                this.materialTiles.Where(kvp => kvp.Value.Visible).ToList().ForEach(kvp =>
                {
                    kvp.Value.Material.IsEnabled = true;
                    kvp.Value.Refresh();
                });

                RefreshCheckboxStates();

                return true;
            }

            if (keyData.HasFlag(Keys.D) && keyData.HasFlag(Keys.Control))
            {
                this.materialTiles.Where(kvp => kvp.Value.Visible).ToList().ForEach(kvp =>
                {
                    kvp.Value.Material.IsEnabled = false;
                    kvp.Value.Refresh();
                });
                RefreshCheckboxStates();
                return true;
            }

            return this.ProcessCmdKey(ref msg, keyData);
        }

        private void ddlMaterialSearch_TextChanged(object sender, EventArgs e)
        {
            var needle = tbxMaterialFilter.Text.ToLowerInvariant();
            this.SetSearchFilter(needle);
        }

        private void SetSearchFilter(string needle)
        {
            //if (needle.Length > 0)
            //{
            //    categoryRefs.ToList().ForEach(x =>
            //    {
            //        if (x.Key != "*")
            //        {
            //            this.materialPanel.Controls.Remove(x.Value.Checkbox);
            //            this.materialPanel.Controls.Remove(x.Value.TilePanel);
            //        }
            //        else
            //        {
            //            this.materialPanel.Controls.Add(x.Value.Checkbox);
            //            this.materialPanel.Controls.Add(x.Value.TilePanel);
            //        }
            //    });
            //}
            //else
            //{
            //    categoryRefs.ToList().ForEach(x =>
            //    {
            //        if (x.Key == "*")
            //        {
            //            this.materialPanel.Controls.Remove(x.Value.Checkbox);
            //            this.materialPanel.Controls.Remove(x.Value.TilePanel);
            //        }
            //        else
            //        {
            //            this.materialPanel.Controls.Add(x.Value.Checkbox);
            //            this.materialPanel.Controls.Add(x.Value.TilePanel);
            //        }
            //    });
            //}

            needle = needle.ToLowerInvariant();
            int? idNeedle = needle.ToNullable<int>();
            bool isv = Options.Get.IsSideView;

            if (needle.StartsWith("#"))
            {
                try
                {
                    int R, G, B;
                    string needleTrim = needle.Trim();

                    if (needleTrim.Length == 7)
                    {
                        R = Convert.ToByte(needleTrim.Substring(1, 2), 16);
                        G = Convert.ToByte(needleTrim.Substring(3, 2), 16);
                        B = Convert.ToByte(needleTrim.Substring(5, 2), 16);
                    }
                    else if (needleTrim.Length == 4)
                    {
                        R = Convert.ToByte(needleTrim.Substring(1, 1) + needleTrim.Substring(1, 1), 16);
                        G = Convert.ToByte(needleTrim.Substring(2, 1) + needleTrim.Substring(2, 1), 16);
                        B = Convert.ToByte(needleTrim.Substring(3, 1) + needleTrim.Substring(3, 1), 16);
                    }
                    else
                    {
                        return;
                    }

                    Color cNeedle = Color.FromArgb(255, R, G, B);
                    var found = Materials.List
                        .Where(x => x.IsVisible)
                        .OrderBy(m => m.getAverageColor(isv).GetColorDistance(cNeedle))
                        .Take(20).ToList();
                    SetVisibleMaterials(found);
                }
                catch (Exception) { }

                return;
            }

            var foo = Materials.List.SelectMany(x => x.Tags).Distinct().ToArray();

            var newList = Materials.List.Where(x =>
            {
                if (!x.IsVisible) return false;

                if (needle == "on" || needle == "enabled" || needle == "active")
                {
                    return x.IsEnabled;
                }

                if (needle == "off" || needle == "disabled" || needle == "inactive")
                {
                    return !x.IsEnabled;
                }

                if (string.IsNullOrWhiteSpace(needle)) return true;
                if (x.Label.StartsWithOrContains(needle, 3)) return true;
                if (x.MinimumSupportedMinecraftVersion.StartsWithOrContains(needle, 3)) return true;
                if (x.Category.StartsWithOrContains(needle, 2)) return true;
                if (needle.Length > 1 && x.Tags.Any(t => t.ToLowerInvariant().StartsWith(needle))) return true;
                if (idNeedle != null && idNeedle == x.BlockID) return true;

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

            int cnt = newList.Count;
            SetVisibleMaterials(newList);
            //categoryRefs.ToList().ForEach(x =>
            //{
            //    bool showIt = !(x.Key == "*" ^ needle.Length > 0); // Must both be TRUE, or both FALSE.
            //                                                       // * && key === 0 ?
            //    x.Value.TilePanel.Visible = showIt;
            //    x.Value.Checkbox.Visible = showIt;
            //});
        }

        public void SetVisibleMaterials(List<Material> mats)
        {
            this.materialPanel.SuspendLayout();

            List<Control> controls = new List<Control>();

            foreach (var kvp in this.materialTiles)
            {
                if (mats.Any(x => x.PixelStackerID == kvp.Value.Material.PixelStackerID && x.IsVisible))
                {
                    kvp.Value.Visible = true;
                }
                else
                {
                    kvp.Value.Visible = false;
                }
            }

            //this.flowLayout.Controls.Clear();

            mats
                .Where(x => this.materialTiles.ContainsKey(x.PixelStackerID) && x.IsVisible)
                .Select(x => this.materialTiles[x.PixelStackerID])
                .ToList()
                .ForEach(x =>
                {
                    x.Visible = true;
                    //this.flowLayout.Controls.Add(x);
                });

            foreach (var cat in categoryRefs)
            {
                if (!cat.Value.Tiles.Any(x => x.Visible))
                {
                    this.materialPanel.Controls.Remove(cat.Value.Checkbox);
                    this.materialPanel.Controls.Remove(cat.Value.TilePanel);
                }
                else
                {
                    this.materialPanel.Controls.Add(cat.Value.Checkbox);
                    this.materialPanel.Controls.Add(cat.Value.TilePanel);
                }
            }

            //// Do it in order of input materials. Important.
            this.materialPanel.ResumeLayout();
            this.RefreshCheckboxStates();
            this.RepositionCheckboxes();
        }


        private void materialSelectGrid_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void cbxIsMultiLayer_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbx = (CheckBox)sender;
            bool isChecked = cbx.CheckState == CheckState.Checked;
            Options.Get.IsMultiLayer = isChecked;
            if (isChecked)
            {
                Options.Get.IsMultiLayer = true;
            }
            else
            {
                cbxRequire2ndLayer.Checked = false;
                Options.Get.IsMultiLayerRequired = false;
                Options.Get.IsMultiLayer = false;
            }
        }

        private void cbxRequire2ndLayer_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbx = (CheckBox)sender;
            bool isChecked = cbx.CheckState == CheckState.Checked;
            if (isChecked)
            {
                cbxIsMultiLayer.Checked = true;
                Options.Get.IsMultiLayer = true;
                Options.Get.IsMultiLayerRequired = true;
            }
            else
            {
                Options.Get.IsMultiLayerRequired = false;
            }
        }

        private void cbxIsSideView_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbx = (CheckBox)sender;
            bool isChecked = cbx.CheckState == CheckState.Checked;
            Options.Get.IsSideView = isChecked;

            this.materialTiles.ToList().ForEach(x =>
            {
                x.Value.Refresh();
            });
        }

        private void MaterialSelectWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.TryHide();
            }
        }

        private void MaterialSelectWindow_VisibleChanged(object sender, EventArgs e)
        {
            this.LoadFromSettings();
            SetVisibleMaterials(Materials.List ?? new List<Material>());
        }

        private void ddlColorProfile_SelectedValueChanged(object sender, EventArgs e)
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
                        if (material != null)
                        {
                            material.IsEnabled = mat.Value;
                        }
                        // This WOULD be faster, but what if I decide to change the format later... best to play it safe
                        // and stable. Time for this iteration takes barely any time at all anyways. Can always optimize
                        // the material list to become a dictionary later if needed.
                        //string key = "BLOCK_" + mat.Key;
                        //Options.Get.EnableStates[key] = mat.Value;
                    }
                }
            }

            tbxMaterialFilter.Text = "enabled";
            SetSearchFilter("enabled");
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
                    Materials = Materials.List.ToDictionary(k => k.PixelStackerID, v => v.IsEnabled)
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

        public void ApplyLocalization(CultureInfo locale)
        {
            this.Text = Resources.Text.MaterialSelect_Title;
            lblColorProfile.Text = Resources.Text.MaterialSelect_ColorProfile;
            lblFilter.Text = Resources.Text.Action_Filter;
            btnEditColorProfiles.Text = Resources.Text.Action_Save;
            cbxIsMultiLayer.Text = Resources.Text.MaterialSelect_IsMultiLayer;
            cbxIsSideView.Text = Resources.Text.MaterialSelect_IsSideView;
            cbxRequire2ndLayer.Text = Resources.Text.MaterialSelect_IsMultiLayerRequired;
        }


        private void MaterialSelectWindow_Resize(object sender, EventArgs e)
            => RepositionCheckboxes();

        private void RepositionCheckboxes()
        {
            foreach (var cRef in this.categoryRefs.Values)
            {
                cRef.Checkbox.Location = new Point(10, 10 + cRef.TilePanel.Location.Y);
            }
        }

        private void MaterialSelectWindow_Load(object sender, EventArgs e)
        {
            RepositionCheckboxes();
            RefreshCheckboxStates();
        }
    }
}
