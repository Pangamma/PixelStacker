using Newtonsoft.Json;
using PixelStacker.Logic;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO;
using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    // TODO: Fix weird display when going to narrow viewing windows
    public partial class MaterialSelectWindow : Form
    {
        private Regex regexMatName = new Regex(@"minecraft:([a-zA-Z_09]+)(.*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private OrderedDictionary<string, MaterialSelectTile> materialTiles = new OrderedDictionary<string, MaterialSelectTile>();
        
        public MaterialSelectWindow()
        {
            InitializeComponent();
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

                this.ddlColorProfile.Text = "Select profile";
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
                        .ForEach(x => {
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected bool ProcessCmdKeyFromTileGrid(Message msg, Keys keyData)
        {
            MainForm.Self.konamiWatcher.ProcessKey(keyData);
            if (keyData.HasFlag(Keys.A) && keyData.HasFlag(Keys.Control))
            {
                this.materialTiles.Where(kvp => kvp.Value.Visible).ToList().ForEach(kvp => {
                    kvp.Value.Material.IsEnabled = true;
                    kvp.Value.Refresh();
                });

                return true;
            }

            if (keyData.HasFlag(Keys.D) && keyData.HasFlag(Keys.Control))
            {
                this.materialTiles.Where(kvp => kvp.Value.Visible).ToList().ForEach(kvp => {
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

            this.materialTiles.ToList().ForEach(x => {
                x.Value.Refresh();
            });
        }

        private async void MaterialSelectWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Options.Save();
            await TaskManager.Get.StartAsync((token) =>
            {
                Materials.CompileColorMap(token, true);
            });

            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
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
                    foreach(var mat in existingProfile.Materials)
                    {
                        var material = Materials.FromPixelStackerID(mat.Key);
                        material.IsEnabled = mat.Value;
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

                var profile = new ColorProfile() {
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
    }
}
