using PixelStacker.Extensions;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Utilities;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.UI.Forms
{
    partial class MaterialPickerForm
    {
        public RenderedCanvas Canvas { get; private set; }

        private void tbxSearchFilter_TextChanged(object sender, EventArgs e)
        {
            var needle = tbxMaterialFilter.Text.ToLowerInvariant();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            this.SetSearchFilter(needle);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            var self = this;
        }

        private async Task SetSearchFilter(string needle, CancellationToken? _worker = null)
        {
            //SetMaterialsCombinedMode(needle.Length > 0);
            //_worker?.SafeThrowIfCancellationRequested();
            //await Task.Yield();

            //needle = needle.ToLowerInvariant();
            //int? idNeedle = needle.ToNullable<int>();
            //bool isv = this.Options.IsSideView;

            //if (needle.StartsWith("#"))
            //{
            //    SKColor? cNeedle = needle.ToSKColor();
            //    if (cNeedle == null) return;
            //    var found = Materials.List
            //        .Where(x => x.IsVisibleF(this.Options))
            //        .OrderBy(m => m.GetAverageColor(isv).GetColorDistance(cNeedle.Value))
            //        .Take(20).ToList();

            //    await SetVisibleMaterials(found, _worker);
            //}

            //var foo = Materials.List.SelectMany(x => x.Tags).Distinct().ToArray();

            //var newList = Materials.List.Where(x =>
            //{
            //    if (!x.IsVisibleF(this.Options)) return false;

            //    if (needle == "on" || needle == "enabled" || needle == "active")
            //    {
            //        return x.IsEnabledF(this.Options);
            //    }

            //    if (needle == "off" || needle == "disabled" || needle == "inactive")
            //    {
            //        return !x.IsEnabledF(this.Options);
            //    }

            //    if (string.IsNullOrWhiteSpace(needle)) return true;
            //    if (x.Label.StartsWithOrContains(needle, 3)) return true;
            //    if (x.MinimumSupportedMinecraftVersion.StartsWithOrContains(needle, 3)) return true;
            //    if (x.Category.StartsWithOrContains(needle, 2)) return true;
            //    if (needle.Length > 1 && x.Tags.Any(t => t.ToLowerInvariant().StartsWith(needle))) return true;
            //    if (idNeedle != null && idNeedle == x.BlockID) return true;

            //    string blockIdAndNBT = x.GetBlockNameAndData(false).ToLowerInvariant();
            //    var match = regexMatName.Match(blockIdAndNBT);
            //    if (match.Success)
            //    {
            //        if (match.Groups[1].Value.StartsWithOrContains(needle, 3))
            //        {
            //            return true;
            //        }
            //    }

            //    return false;
            //}).ToList();

            //_worker?.SafeThrowIfCancellationRequested();
            //await Task.Yield();


            //int cnt = newList.Count;
            //await SetVisibleMaterials(newList, _worker);
        }

        internal Task SetCanvas(RenderedCanvas canvas)
        {
            this.Canvas = canvas;
            ShowMaterialsToChooseFrom();
            return Task.CompletedTask;
        }


        private void ShowMaterialsToChooseFrom()
        {
            List<MaterialCombination> pool;
            switch ((ddlColorPool.SelectedItem as ComboBoxItem)?.Value ?? "ERROR")
            {
                case "ALL": 
                    pool = this.GetAllMaterials();
                    break;
                case "FROM_CANVAS": 
                    pool = this.GetMaterialsInCurrentCanvas();
                    break;
                default:
                    pool = new List<MaterialCombination>();
                    throw new ArgumentException($"Unrecognized value for {nameof(ddlColorPool)}.");
            }

            if (this.Options.Tools.ZLayerFilter == ZLayer.Top)
            {

            }
        }

        private List<MaterialCombination> GetAllMaterials()
        {
            if (this.Canvas == null)
            {
                return new List<MaterialCombination>();
            }

            var mp = this.Canvas.MaterialPalette;
            var mats = mp.ToCombinationList();

            return mats;
        }

        private List<MaterialCombination> GetMaterialsInCurrentCanvas()
        {
            if (this.Canvas == null)
            {
                return new List<MaterialCombination>();
            }

            var cd = this.Canvas.CanvasData;
            var mp = this.Canvas.MaterialPalette;
            var mats = cd.Distinct(x => x.PaletteID)
                .ToList()
                .Select(cdData => mp[cdData.PaletteID])
                .ToList();

            return mats;
        }
    }
}