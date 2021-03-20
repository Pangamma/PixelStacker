using PixelStacker.Logic;
using PixelStacker.Logic.Collections;
using PixelStacker.Logic.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    partial class MainForm
    {
        private ColorPaletteStyle SelectedColorPaletteStyle { get; set; } = ColorPaletteStyle.DetailedGrid;
        private string loadedImageFilePath { get; set; }


        #region Color palette generation 

        private void dlgSaveColorPalette_FileOk_ColorPalettes(object sender, CancelEventArgs e)
        {
            if (this.LoadedBlueprint != null)
            {
                SaveFileDialog dlg = (SaveFileDialog) sender;
                string fName = dlg.FileName;
                ColorPaletteFormatter.WriteBlueprint(fName, this.LoadedBlueprint, SelectedColorPaletteStyle);
            }
        }

        private void openSaveForColorPalettes(int filterIndex)
        {
            string[] availableExtensions = new string[] { "Color Palette Graph | *.png", "Color Palette Brick|*.png", "Color Palette Square | *.png", "Color Palette All (compact)|*.png", "Color Palette All (detailed)|*.png" };
            dlgSaveColorPalette.Filter = availableExtensions[filterIndex];
            dlgSaveColorPalette.DefaultExt = availableExtensions[filterIndex].Substring(availableExtensions[filterIndex].LastIndexOf("*.") + 2);
            dlgSaveColorPalette.ShowDialog(this);
        }

        private void allItemsInColorPaletteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var mats = Materials.List.Where(x => x.IsEnabled);
            var map = new int[10, 1 + (mats.Count() / 10)];
            BlueprintPA print = new BlueprintPA()
            {
                MaxDepth = 1,
            };

            SchemFormatter.writeBlueprint("./io.schem", print);
        }

        private void graphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.LoadedBlueprint == null)
            {
                this.saveColorPaletteToolStripMenuItem.Enabled = false;
                return;
            }

            this.SelectedColorPaletteStyle = ColorPaletteStyle.CompactGraph;
            this.openSaveForColorPalettes(0);
        }

        private void brickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.LoadedBlueprint == null)
            {
                this.saveColorPaletteToolStripMenuItem.Enabled = false;
                return;
            }

            this.SelectedColorPaletteStyle = ColorPaletteStyle.CompactBrick;
            this.openSaveForColorPalettes(1);
        }

        private void squareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.LoadedBlueprint == null)
            {
                this.saveColorPaletteToolStripMenuItem.Enabled = false;
                return;
            }

            this.SelectedColorPaletteStyle = ColorPaletteStyle.CompactSquare;
            this.openSaveForColorPalettes(2);
        }

        private void allPossibilitiescompactToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.LoadedBlueprint == null)
            {
                this.saveColorPaletteToolStripMenuItem.Enabled = false;
                return;
            }

            this.SelectedColorPaletteStyle = ColorPaletteStyle.CompactGrid;
            this.openSaveForColorPalettes(3);
        }

        private void allColorsdetailedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.LoadedBlueprint == null)
            {
                this.saveColorPaletteToolStripMenuItem.Enabled = false;
                return;
            }

            this.SelectedColorPaletteStyle = ColorPaletteStyle.DetailedGrid;
            this.openSaveForColorPalettes(4);
        }

        #endregion

        #region Load 

        private void reOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.loadedImageFilePath != null)
            {
                using (Bitmap img = (Bitmap) Bitmap.FromFile(this.loadedImageFilePath))
                {
                    this.LoadedImage.DisposeSafely();
                    this.LoadedImage = img.To32bppBitmap();
                }
                MainForm.PanZoomSettings = null;
                this.imagePanelMain.SetImage(this.LoadedImage);
                ShowImagePanel();
                this.PreRenderedImage.DisposeSafely();
                this.PreRenderedImage = null;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskManager.Get.CancelTasks(() => {
                this.InvokeEx(c => c.dlgOpen.ShowDialog());
            });
        }

        private void dlgOpen_FileOk(object sender, CancelEventArgs e)
        {
            OpenFileDialog dialog = (OpenFileDialog) sender;
            this.loadedImageFilePath = dialog.FileName;
            this.reOpenToolStripMenuItem.Enabled = true;
            using (Bitmap img = (Bitmap) Bitmap.FromFile(this.loadedImageFilePath))
            {
                this.LoadedImage.DisposeSafely();
                this.LoadedImage = img.To32bppBitmap(); // creates a clone of the img, but in the 32bpp format.
            }

            MainForm.PanZoomSettings = null;
            this.imagePanelMain.SetImage(this.LoadedImage);
            this.PreRenderedImage.DisposeSafely();
            this.PreRenderedImage = null;
            this.History.Clear();
            ShowImagePanel();
        }
        #endregion

        #region Save Export regular

        private void saveMenuClick(object sender, EventArgs e)
        {
            if (this.LoadedBlueprint != null)
            {
                dlgSave.ShowDialog();
            }
            else
            {
                this.exportSchematicToolStripMenuItem.Enabled = false;
            }
        }

        private void dlgSave_FileOk(object sender, CancelEventArgs e)
        {
            if (this.LoadedBlueprint != null)
            {
                SaveFileDialog dlg = (SaveFileDialog) sender;
                string fName = dlg.FileName;
                int chosenFilterIndex = dlg.FilterIndex;

                if (fName.ToLower().EndsWith(".schem"))
                {
                    Schem2Formatter.writeBlueprint(fName, this.LoadedBlueprint);
                }
                else if (fName.ToLower().EndsWith(".schematic"))
                {
                    if (Materials.List.Any(x => x.IsEnabled && string.IsNullOrWhiteSpace(x.SchematicaMaterialName)))
                    {
                        DialogResult result = MessageBox.Show(this,
                            "Cannot create a 1.12 schematic when 1.13+ blocks are selected. Do you want to automatically " +
                            "disable invalid materials and restart from the beginning of the rendering process. This " +
                            "will remove any material choice overrides or post rendering changes.\n\nYou will need to " +
                            "re-render the image.",
                            ".schematic does not support 1.13+ blocks!"
                            , MessageBoxButtons.YesNo, MessageBoxIcon.Warning
                            );

                        if (result == DialogResult.Yes)
                        {
                            Materials.List.Where(x => x.IsEnabled && string.IsNullOrWhiteSpace(x.SchematicaMaterialName))
                                .ToList().ForEach(x => x.IsEnabled = false);
                            Options.Save();
                            this.InvokeEx(c => c.MaterialOptions.Refresh());

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                            TaskManager.Get.StartAsync((token) =>
                            {
                                ColorMatcher.Get.CompileColorPalette(token, true, Materials.List)
                                .ConfigureAwait(true).GetAwaiter().GetResult();
                            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                            return;
                        }
                    }
                    else
                    {
                        SchematicFormatter.writeBlueprint(fName, this.LoadedBlueprint);
                    }
                }
                else if (fName.ToLower().EndsWith(".png"))
                {
                    if (this.renderedImagePanel != null)
                    {
                        this.renderedImagePanel.SaveToPNG(fName);
                    }
                }
                else if (fName.ToLower().EndsWith(".pxlzip"))
                {
                    PixelStackerProjectFormatter.SaveProject(fName);
                }
                else if (fName.ToLower().EndsWith(".csv"))
                {
                    Dictionary<Material, int> materialCounts = new Dictionary<Material, int>();
                    bool isv = Options.Get.IsSideView;
                    int xM = this.LoadedBlueprint.Mapper.GetXLength(isv);
                    int yM = this.LoadedBlueprint.Mapper.GetYLength(isv);
                    int zM = this.LoadedBlueprint.Mapper.GetZLength(isv);
                    for (int x = 0; x < xM; x++)
                    {
                        for (int y = 0; y < yM; y++)
                        {
                            for (int z = 0; z < zM; z++)
                            {
                                Material m = this.LoadedBlueprint.Mapper.GetMaterialAt(isv, x, y, z);
                                if (m != Materials.Air)
                                {
                                    if (!materialCounts.ContainsKey(m))
                                    {
                                        materialCounts.Add(m, 0);
                                    }

                                    materialCounts[m] = materialCounts[m] + 1;
                                }
                            }
                        }
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("\"Material\",\"Block Count\",\"Full Stacks needed\"");
                    sb.AppendLine("\"Total\"," + materialCounts.Values.Sum());

                    foreach (var kvp in materialCounts.OrderByDescending(x => x.Value))
                    {
                        sb.AppendLine($"\"{kvp.Key.GetBlockNameAndData(isv).Replace("\"", "\"\"")}\",{kvp.Value},{kvp.Value / 64} stacks and {kvp.Value % 64} remaining blocks");
                    }

                    File.WriteAllText(fName, sb.ToString());
                }
            }
            else
            {
                this.exportSchematicToolStripMenuItem.Enabled = false;
            }
        }

        #endregion

        private void exportSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgSaveSettings.ShowDialog();
        }

        private void dlgSaveSettings_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveFileDialog dlg = (SaveFileDialog) sender;
            Options.Export(dlg.FileName);
        }
    }
}
