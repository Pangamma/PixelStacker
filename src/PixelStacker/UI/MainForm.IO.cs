using PixelStacker.Extensions;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Formatters;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkiaSharp;

namespace PixelStacker.UI
{
    public partial class MainForm
    {
        private async void saveAsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            await TaskManager.Get.CancelTasks();
            dlgSave.ShowDialog(this);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loadedImageFilePath != null)
                if (!loadedImageFilePath.EndsWith(".pxlzip"))
                {
                    saveAsToolStripMenuItem_Click(sender, e);
                }
        }

        private async void dlgSave_FileOk(object sender, CancelEventArgs e)
        {
            if (this.RenderedCanvas != null)
            {
                SaveFileDialog dlg = (SaveFileDialog)sender;
                string fName = dlg.FileName;
                string ext = fName.GetFileExtension().ToLower();
                int chosenFilterIndex = dlg.FilterIndex;
                IExportFormatter formatter;
                switch (ext)
                {
                    case "png":
                        formatter = new PngFormatter();
                        break;
                    case "pxlzip":
                        formatter = new PixelStackerProjectFormatter();
                        break;
                    case "schem":
                        formatter = new Schem2Formatter();
                        break;
                    case "schematic":
                        throw new Exception("Not ready");
                    //formatter = new SchematicFormatter();
                    case "csv":
                        formatter = new BlockCountCsvFormatter();
                        break;
                    default:
                        throw new NotSupportedException($"That file format is not supported. Given: {ext}");
                }

                await Task.Run(() => TaskManager.Get.StartAsync(async (worker) =>
                {
                    await formatter.ExportAsync(fName, new PixelStackerProjectData(this.RenderedCanvas, this.Options), worker);
                }));

                //                if (fName.ToLower().EndsWith(".schem"))
                //                {
                //                    Schem2Formatter.writeBlueprint(fName, this.LoadedBlueprint);
                //                }
                //                else if (fName.ToLower().EndsWith(".schematic"))
                //                {
                //                    if (Materials.List.Any(x => x.IsEnabled && string.IsNullOrWhiteSpace(x.SchematicaMaterialName)))
                //                    {
                //                        DialogResult result = MessageBox.Show(this,
                //                            "Cannot create a 1.12 schematic when 1.13+ blocks are selected. Do you want to automatically " +
                //                            "disable invalid materials and restart from the beginning of the rendering process. This " +
                //                            "will remove any material choice overrides or post rendering changes.\n\nYou will need to " +
                //                            "re-render the image.",
                //                            ".schematic does not support 1.13+ blocks!"
                //                            , MessageBoxButtons.YesNo, MessageBoxIcon.Warning
                //                            );

                //                        if (result == DialogResult.Yes)
                //                        {
                //                            Materials.List.Where(x => x.IsEnabled && string.IsNullOrWhiteSpace(x.SchematicaMaterialName))
                //                                .ToList().ForEach(x => x.IsEnabled = false);
                //                            Options.Save();
                //                            this.InvokeEx(c => c.MaterialOptions.Refresh());

                //#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                //                            TaskManager.Get.StartAsync((token) =>
                //                            {
                //                                ColorMatcher.Get.CompileColorPalette(token, true, Materials.List)
                //                                .ConfigureAwait(true).GetAwaiter().GetResult();
                //                            });
                //#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                //                            return;
                //                        }
                //                    }
                //                    else
                //                    {
                //                        SchematicFormatter.writeBlueprint(fName, this.LoadedBlueprint);
                //                    }
                //                }
                //                else if (fName.ToLower().EndsWith(".csv"))
                //                {
                //                    Dictionary<Material, int> materialCounts = new Dictionary<Material, int>();
                //                    bool isv = Options.Get.IsSideView;
                //                    int xM = this.LoadedBlueprint.Mapper.GetXLength(isv);
                //                    int yM = this.LoadedBlueprint.Mapper.GetYLength(isv);
                //                    int zM = this.LoadedBlueprint.Mapper.GetZLength(isv);
                //                    for (int x = 0; x < xM; x++)
                //                    {
                //                        for (int y = 0; y < yM; y++)
                //                        {
                //                            for (int z = 0; z < zM; z++)
                //                            {
                //                                Material m = this.LoadedBlueprint.Mapper.GetMaterialAt(isv, x, y, z);
                //                                if (m != Materials.Air)
                //                                {
                //                                    if (!materialCounts.ContainsKey(m))
                //                                    {
                //                                        materialCounts.Add(m, 0);
                //                                    }

                //                                    materialCounts[m] = materialCounts[m] + 1;
                //                                }
                //                            }
                //                        }
                //                    }

                //                    StringBuilder sb = new StringBuilder();
                //                    sb.AppendLine("\"Material\",\"Block Count\",\"Full Stacks needed\"");
                //                    sb.AppendLine("\"Total\"," + materialCounts.Values.Sum());

                //                    foreach (var kvp in materialCounts.OrderByDescending(x => x.Value))
                //                    {
                //                        sb.AppendLine($"\"{kvp.Key.GetBlockNameAndData(isv).Replace("\"", "\"\"")}\",{kvp.Value},{kvp.Value / 64} stacks and {kvp.Value % 64} remaining blocks");
                //                    }

                //                    File.WriteAllText(fName, sb.ToString());
                //}
            }
            else
            {
                //this.exportSchematicToolStripMenuItem.Enabled = false;
            }
        }


        private string loadedImageFilePath;
        private void reOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.loadedImageFilePath != null)
            {
                this.LoadFileFromPath(this.loadedImageFilePath);
            }
        }

        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await TaskManager.Get.CancelTasks();
            dlgOpen.ShowDialog(this);
        }

        private void dlgOpen_FileOk(object sender, CancelEventArgs e)
        {
            OpenFileDialog dialog = (OpenFileDialog)sender;
            this.loadedImageFilePath = dialog.FileName;
            this.reOpenToolStripMenuItem.Enabled = true;
            this.LoadFileFromPath(this.loadedImageFilePath);
        }

        private PixelStackerProjectFormatter pxlzipFormatter = new PixelStackerProjectFormatter();
        private async void LoadFileFromPath(string _path)
        {
            string ext = _path.Split('.', StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
            if (ext == "pxlzip")
            {
                var self = this;
                // pixelstacker project handling
                await Task.Run(() => TaskManager.Get.StartAsync(async (worker) =>
                {
                    var proj = await pxlzipFormatter.ImportAsync(_path, worker);

                    worker.ThrowIfCancellationRequested();

                    // Prepare for generation. This step might be skippable.
                    this.ColorMapper.SetSeedData(proj.MaterialPalette.ToValidCombinationList(Options), proj.MaterialPalette, Options.Preprocessor.IsSideView);

                    worker.ThrowIfCancellationRequested();

                    await self.InvokeEx(async c =>
                    {
                        // Load the file into regular viewer
                        c.PreprocessedImage = proj.PreprocessedImage;
                        c.LoadedImage = proj.PreprocessedImage;
                        c.imageViewer.SetImage(c.PreprocessedImage, null);








                        ////----------

                        //// Super dubious and sketchy logic here. Might crash due to cross-context thread access issues
                        //self.RenderedCanvas = await engine.RenderCanvasAsync(worker, ref imgPreprocessed, this.ColorMapper, this.Palette);
                        //worker.ThrowIfCancellationRequested();
                        //await self.canvasEditor.SetCanvas(worker, self.RenderedCanvas, this.imageViewer.PanZoomSettings);

                        //ProgressX.Report(0, "Showing block plan in the viewing window.");
                        //self.InvokeEx(cc =>
                        //{
                        //    cc.ShowCanvasEditor();
                        //    cc.TS_OnRenderCanvas();
                        //});
                        ////-----------



                        var pz = c.imageViewer.PanZoomSettings;
                        await c.canvasEditor.SetCanvas(worker, proj, pz);
                        c.ShowCanvasEditor();
                    });
                }));
            }
            else
            {
                using (SKBitmap img = SKBitmap.Decode(_path))
                {
                    this.LoadedImage.DisposeSafely();
                    this.LoadedImage = img.Copy();
                }

                // creates a clone of the img, but in the 32bpp format.
                this.imageViewer.SetImage(this.LoadedImage, null);
                ShowImageViewer();
            }

            //this.PreRenderedImage.DisposeSafely();
            //this.PreRenderedImage = null;
            //this.History.Clear();
        }
    }
}
