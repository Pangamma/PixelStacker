using PixelStacker.Extensions;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.IO.Formatters;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using SkiaSharp;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            {
                if (!loadedImageFilePath.EndsWith(".pxlzip"))
                {
                    saveAsToolStripMenuItem_Click(sender, e);
                }
                else
                {
                    this.saveToFileName(loadedImageFilePath);
                }
            }

        }

        private void dlgSave_FileOk(object sender, CancelEventArgs e)
        {
            SaveFileDialog dlg = (SaveFileDialog)sender;
            string fName = dlg.FileName;
            this.saveToFileName(fName);
        }

        private async void saveToFileName(string fName)
        {
            if (this.RenderedCanvas != null)
            {
                string ext = fName.GetFileExtension().ToLower();
                IExportFormatter formatter;
                switch (ext)
                {
                    case "png":
                        if (fName.EndsWith(".sm.png")) formatter = new PngPreviewFormatter();
                        else formatter = new PngFormatter();
                        break;
                    case "pxlzip":
                        formatter = new PixelStackerProjectFormatter();
                        break;
                    case "nbt":
                        formatter = new StructureBlockFormatter();
                        break;
                    case "schem":
                        formatter = new Schem2Formatter();
                        break;
                    case "csv":
                        formatter = new BlockCountCsvFormatter();
                        break;
                    default:
                        throw new NotSupportedException($"That file format is not supported. Given: {ext}");
                }

                await Task.Run(() => TaskManager.Get.StartAsync(async (worker) =>
                {
                    ProgressX.Report(0, "Saving changes.");
                    PixelStackerProjectData proj = new PixelStackerProjectData(this.RenderedCanvas, this.Options);
                    if (formatter is IExportImageFormatter imgFormatter)
                    {
                        var srs = this.Options.ViewerSettings.ToReadonlyClone();
                        await imgFormatter.SaveToFile(fName, proj, srs, worker);
                    } 
                    else
                    {
                        await formatter.SaveToFile(fName, proj, worker);
                    }
                    ProgressX.Report(100, "File saved.");
                }));
            }
            else
            {
                //this.exportSchematicToolStripMenuItem.Enabled = false;
            }
        }


        private string loadedImageFilePath;
        private async void reOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.loadedImageFilePath != null)
            {
                await this.LoadFileFromPathAsync(this.loadedImageFilePath);
            }
            else
            {
                var toKill = this.LoadedImage;
                this.LoadedImage = this.LoadedImage.Copy();
                toKill.DisposeSafely();
                ShowImageViewer_OriginalImage();
            }
        }

        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await TaskManager.Get.CancelTasks();
            dlgOpen.ShowDialog(this);
        }

        private async void dlgOpen_FileOk(object sender, CancelEventArgs e)
        {
            OpenFileDialog dialog = (OpenFileDialog)sender;
            this.loadedImageFilePath = dialog.FileName;
            this.reOpenToolStripMenuItem.Enabled = true;
            await this.LoadFileFromPathAsync(this.loadedImageFilePath);
        }

        private PixelStackerProjectFormatter pxlzipFormatter = new PixelStackerProjectFormatter();
        private async Task LoadFileFromPathAsync(string _path)
        {
            string ext = _path.Split('.', StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
            if (ext == "pxlzip")
            {
                var self = this;
                // pixelstacker project handling
                await Task.Run(() => TaskManager.Get.StartAsync(async (worker) =>
                {
                    var proj = await pxlzipFormatter.ImportAsync(_path, worker);
                    proj.IsSideView = this.Options.IsSideView;

                    worker.ThrowIfCancellationRequested();

                    // Prepare for generation. This step might be skippable.
                    this.ColorMapper.SetSeedData(proj.MaterialPalette.ToValidCombinationList(Options), proj.MaterialPalette, Options.IsSideView);

                    worker.ThrowIfCancellationRequested();

                    await self.InvokeEx(async c =>
                    {
                        // Load the file into regular viewer
                        c.PreprocessedImage = proj.PreprocessedImage;
                        c.LoadedImage = proj.PreprocessedImage;
                        c.imageViewer.SetImage(c.PreprocessedImage, null);
                        c.RenderedCanvas = proj;

                        var pz = c.imageViewer.PanZoomSettings;
                        await c.canvasEditor.SetCanvas(worker, proj, () => c.imageViewer.PanZoomSettings, c.Options.ViewerSettings.ToReadonlyClone());
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
                ShowImageViewer_OriginalImage();
            }
        }
    }
}
