using PixelStacker.Extensions;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using PixelStacker.UI.Controls.Pickers;
using PixelStacker.Utilities;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.UI.Forms
{
    partial class MaterialPickerForm
    {
        public RenderedCanvas Canvas { get; private set; }
        private DelayThrottle delayFilter = new DelayThrottle(TimeSpan.FromMilliseconds(250));

        private async void tbxSearchFilter_TextChanged(object sender, EventArgs e)
        {
            if (!(await delayFilter.CanWaitEntireDelayWithoutInteruptions()))
            {
                return;
            }

            var needle = tbxMaterialFilter.Text.ToLowerInvariant();
            await this.SetSearchFilterAsync(needle, this.pnlBottomMats);
            await this.SetSearchFilterAsync(needle, this.pnlTopMats);
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

        private void InitializeTabs()
        {
            bool isv = this.Options.IsSideView;
            {
                List<Material> matUpper = Materials.List.Where(x => x.Category == Constants.CatGlass)
                    .Where(x => x.IsVisibleF(this.Options))
                    .ToList();

                List<ImageButtonData> items = new List<ImageButtonData>();
                items.Add(new ImageButtonData()
                {
                    Data = Materials.Air,
                    Image = global::PixelStacker.Resources.Textures.disabled,
                    Text = Resources.Text.Nothing
                });
                items.AddRange(matUpper.Select(x => new ImageButtonData()
                {
                    Data = x,
                    Image = x.GetImage(isv),
                    Text = x.Label,
                }));

                pnlTopMats.InitializeButtons(items);
            }

            {
                List<Material> matLower = Materials.List.Where(x => x.CanBeOnBottom)
                    .Where(x => x.IsVisibleF(this.Options))
                    .ToList();

                List<ImageButtonData> items = new List<ImageButtonData>();
                items.Add(new ImageButtonData()
                {
                    Data = Materials.Air,
                    Image = global::PixelStacker.Resources.Textures.disabled,
                    Text = Resources.Text.Nothing
                });
                items.AddRange(matLower.Select(x => new ImageButtonData()
                {
                    Data = x,
                    Image = x.GetImage(isv),
                    Text = x.Label,
                }));

                pnlBottomMats.InitializeButtons(items);
            }
        }

        private Regex regexMatName = new Regex(@"minecraft:([a-zA-Z_09]+)(.*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private async Task SetSearchFilterAsync(string needle, ImageButtonPanel panel, CancellationToken? _worker = null)
        {
            bool isv = Options.IsSideView;
            if (string.IsNullOrWhiteSpace(needle))
            {
                panel.DoFilterTakeOrderByOperation(
                    orderBy: x => this.MaterialOrders[x.GetData<Material>().PixelStackerID]
                );
                return;
            }

            needle = needle.ToLowerInvariant();
            var pc = Options.Tools.PrimaryColor;
            if (needle.StartsWith("#"))
            {
                SKColor? cNeedle = needle.ToSKColor();
                if (cNeedle == null) return;

                panel.DoFilterTakeOrderByOperation(take: 20,
                    filter: x => x.GetData<Material>().IsVisibleF(this.Options),
                    orderBy: x => x.GetData<Material>().GetAverageColor(isv).GetColorDistance(cNeedle.Value)
                    );
                return;
                //await SetVisibleMaterials(found, _worker);
            }

            _worker?.SafeThrowIfCancellationRequested();
            await Task.Yield();

            int? idNeedle = needle.ToNullable<int>();
            //var foo = Materials.List.SelectMany(x => x.Tags).Distinct().ToArray();

            panel.DoFilterTakeOrderByOperation(
                filter: mb =>
                {
                    Material x = mb.GetData<Material>();

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
                },
                orderBy: x => this.MaterialOrders[x.GetData<Material>().PixelStackerID]
                );

            _worker?.SafeThrowIfCancellationRequested();
            await Task.Yield();


            //int cnt = newList.Count;
            //await SetVisibleMaterials(newList, _worker);
        }

        internal async Task SetCanvas(RenderedCanvas canvas)
        {
            this.Canvas = canvas;
            await SetSearchFilterAsync("", this.pnlBottomMats);
            await SetSearchFilterAsync("", this.pnlTopMats);
        }

        //private List<MaterialCombination> GetAllMaterials()
        //{
        //    if (this.Canvas == null)
        //    {
        //        return new List<MaterialCombination>();
        //    }

        //    var mp = this.Canvas.MaterialPalette;
        //    var mats = mp.ToCombinationList();

        //    return mats;
        //}

        //private List<MaterialCombination> GetMaterialsInCurrentCanvas()
        //{
        //    if (this.Canvas == null)
        //    {
        //        return new List<MaterialCombination>();
        //    }

        //    var cd = this.Canvas.CanvasData;
        //    var mp = this.Canvas.MaterialPalette;
        //    var mats = cd.Distinct(x => x.PaletteID)
        //        .ToList()
        //        .Select(cdData => mp[cdData.PaletteID])
        //        .ToList();

        //    return mats;
        //}
    }
}