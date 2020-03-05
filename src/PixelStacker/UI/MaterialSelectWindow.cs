using PixelStacker.Logic;
using PixelStacker.Logic.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

            this.InitializeAutoComplete();
            bool isv = Options.Get.IsSideView;
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

            SetVisibleMaterials(Materials.List ?? new List<Material>());
            this.LoadFromSettings();
        }

        private void LoadFromSettings()
        {
            cbxIsMultiLayer.Checked = Options.Get.IsMultiLayer;
            cbxIsSideView.Checked = Options.Get.IsSideView;
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

        private void SetVisibleMaterials(List<Material> mats)
        {
            this.flowLayout.SuspendLayout();

            foreach (var kvp in this.materialTiles)
            {
                if (mats.Any(x => x.PixelStackerID == kvp.Key))
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
        }
    }
}
