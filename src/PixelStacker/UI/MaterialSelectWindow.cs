using Newtonsoft.Json;
using PixelStacker.Logic;
using PixelStacker.Logic.Collections;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO;
using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    // TODO: Fix weird display when going to narrow viewing windows
    public partial class MaterialSelectWindow : Form, ILocalized
    {
        private Regex regexMatName = new Regex(@"minecraft:([a-zA-Z_09]+)(.*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private OrderedDictionary<string, MaterialSelectTile> materialTiles = new OrderedDictionary<string, MaterialSelectTile>();

        public MaterialSelectWindow()
        {
            InitializeComponent();
            ApplyLocalization(System.Threading.Thread.CurrentThread.CurrentUICulture);
            this.flowLayout.OnCommandKey = (Message msg, Keys keyData) => this.ProcessCmdKeyFromTileGrid(msg, keyData);

            bool isv = Options.Get.IsSideView;
            this.InitializeAutoComplete();
            this.InitializeMaterialTiles();
            SetVisibleMaterials(Materials.List ?? new List<Material>());
            this.LoadFromSettings();
        }

        private void InitializeMaterialTiles()
        {
            this.flowLayout.Controls.Clear();
            foreach (var m in Materials.List.Where(m => m.PixelStackerID != "AIR"))
            {
                this.materialTiles[m.PixelStackerID] = new MaterialSelectTile()
                {
                    Material = m,
                    Visible = true
                };


                this.materialTiles[m.PixelStackerID].MouseEnter += this.OnMouseEnter_Tile;
                this.materialTiles[m.PixelStackerID].MouseLeave += this.OnMouseLeave_Tile;
                this.materialTiles[m.PixelStackerID].MouseClick += this.OnMouseClick_Tile;


                this.flowLayout.Controls.Add(this.materialTiles[m.PixelStackerID]);
            }
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
            this.lblInfo.Text = ((MaterialSelectTile) sender).Material.Label;
        }

        private MaterialSelectTile previouslyClickedTile = null;
        private void OnMouseClick_Tile(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var currentTile = (MaterialSelectTile) sender;
                if (!Control.ModifierKeys.HasFlag(Keys.Shift) || previouslyClickedTile == null)
                {
                    previouslyClickedTile = currentTile;
                }
                else
                {
                    // go time.
                    int idxA = this.materialTiles.IndexOf(previouslyClickedTile.Material.PixelStackerID);
                    int idxB = this.materialTiles.IndexOf(currentTile.Material.PixelStackerID);
                    int minI = Math.Min(idxA, idxB);
                    int maxI = Math.Max(idxA, idxB);
                    this.materialTiles.Skip(minI).Take(maxI - minI).ToList()
                        .ForEach(x =>
                        {
                            x.Value.Material.IsEnabled = !Control.ModifierKeys.HasFlag(Keys.Control);
                            x.Value.Refresh();
                        });
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

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected bool ProcessCmdKeyFromTileGrid(Message msg, Keys keyData)
        {
            MainForm.Self.konamiWatcher.ProcessKey(keyData);
            if (keyData.HasFlag(Keys.A) && keyData.HasFlag(Keys.Control))
            {
                this.materialTiles.Where(kvp => kvp.Value.Visible).ToList().ForEach(kvp =>
                {
                    kvp.Value.Material.IsEnabled = true;
                    kvp.Value.Refresh();
                });

                return true;
            }

            if (keyData.HasFlag(Keys.D) && keyData.HasFlag(Keys.Control))
            {
                this.materialTiles.Where(kvp => kvp.Value.Visible).ToList().ForEach(kvp =>
                {
                    kvp.Value.Material.IsEnabled = false;
                    kvp.Value.Refresh();
                });
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
                if (x.Label.ToLowerInvariant().Contains(needle)) return true;
                if (x.MinimumSupportedMinecraftVersion.ToLowerInvariant().Contains(needle)) return true;
                if (x.Category.ToLowerInvariant().Contains(needle)) return true;
                if (x.Tags.Any(t => t.ToLowerInvariant().Contains(needle))) return true;
                if (idNeedle != null && idNeedle == x.BlockID) return true;


                string blockIdAndNBT = x.GetBlockNameAndData(false).ToLowerInvariant();
                var match = regexMatName.Match(blockIdAndNBT);
                if (match.Success)
                {
                    if (match.Groups[1].Value.ToLowerInvariant().Contains(needle))
                    {
                        return true;
                    }
                }

                return false;
            }).ToList();

            SetVisibleMaterials(newList);
        }

        public void SetVisibleMaterials(List<Material> mats)
        {
            this.flowLayout.SuspendLayout();

            List<Control> controls = new List<Control>();

            foreach (var kvp in this.materialTiles)
            {
                if (mats.Any(x => x.PixelStackerID == kvp.Key && x.IsVisible))
                {
                    kvp.Value.Visible = true;
                }
                else
                {
                    kvp.Value.Visible = false;
                }
            }

            this.flowLayout.Controls.Clear();

            mats
                .Where(x => this.materialTiles.ContainsKey(x.PixelStackerID) && x.IsVisible)
                .Select(x => this.materialTiles[x.PixelStackerID])
                .ToList()
                .ForEach(x => {
                    x.Visible = true;
                    this.flowLayout.Controls.Add(x);
                });

            // Do it in order of input materials. Important.
            this.flowLayout.ResumeLayout();
        }


        private void materialSelectGrid_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void cbxIsMultiLayer_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbx = (CheckBox) sender;
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
            CheckBox cbx = (CheckBox) sender;
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
            CheckBox cbx = (CheckBox) sender;
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
            string item = (string) ddlColorProfile.SelectedItem;
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
    }
}
