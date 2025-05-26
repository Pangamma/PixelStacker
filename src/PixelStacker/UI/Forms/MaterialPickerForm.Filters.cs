using PixelStacker.Extensions;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using PixelStacker.Resources;
using PixelStacker.UI.Controls;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.UI.Forms
{
    partial class MaterialPickerForm
    {
        public RenderedCanvas Canvas { get; private set; }

        private bool filterNeedsRefresh = false;
        private void tbxSearchFilter_TextChanged(object sender, EventArgs e)
        {
            filterNeedsRefresh = true;
        }

        private void InitializeAutoComplete()
        {
            tbxMaterialFilter.AutoCompleteCustomSource.Clear();

            tbxMaterialFilter.AutoCompleteCustomSource.AddRange(
                Materials.List.SelectMany(x => x.Tags).Distinct().ToArray()
            );

            tbxMaterialFilter.AutoCompleteCustomSource.AddRange(
                Materials.List.Where(x => !x.IsObsolete).Select(x => x.Category.ToLowerInvariant()).Distinct().ToArray()
            );

            tbxMaterialFilter.AutoCompleteCustomSource.AddRange(
                Materials.List.Where(x => !x.IsObsolete).Select(x => x.Label.ToLowerInvariant()).Distinct().ToArray()
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

        private List<ImageButtonData> GetImageButtonData_Upper()
        {
            bool isv = this.Options.IsSideView;
            List<Material> matUpper = Materials.List.Where(x => x.CanBeUsedAsTopLayer)
                .Where(x => x.IsVisibleF(this.Options))
                .ToList();

            List<ImageButtonData> items = new List<ImageButtonData>();
            items.Add(new ImageButtonData()
            {
                Data = Materials.Air,
                Image = (global::PixelStacker.Resources.Textures.disabled),
                Text = Resources.Text.Nothing
            });
            items.AddRange(matUpper.Select(x => new ImageButtonData()
            {
                Data = x,
                Image = (x.GetImage(isv)),
                Text = x.Label,
            }));

            return items;
        }

        private List<ImageButtonData> GetImageButtonData_Lower()
        {
            bool isv = this.Options.IsSideView;

            List<Material> matLower = Materials.List.Where(x => x.CanBeUsedAsBottomLayer)
                .Where(x => x.IsVisibleF(this.Options))
                .ToList();

            List<ImageButtonData> items = new List<ImageButtonData>();
            items.Add(new ImageButtonData()
            {
                Data = Materials.Air,
                Image = (global::PixelStacker.Resources.Textures.disabled),
                Text = Resources.Text.Nothing
            });
            items.AddRange(matLower.Select(x => new ImageButtonData()
            {
                Data = x,
                Image = (x.GetImage(isv)),
                Text = x.Label,
            }));

            return items;
        }

        public List<ImageButtonData> GetImageButtonData_Both()
        {
            int MAX_PULL = 50;
            bool isv = this.Options.IsSideView;
            var c = this.SelectedCombo.GetAverageColor(isv);

            List<MaterialCombination> mats = MaterialPalette.FromResx()
                .ToCombinationList().Where(x => !x.Bottom.IsAdvanced).ToList();
            
            var air = MaterialPalette.FromResx().GetMaterialCombinationByMaterials(Materials.Air, Materials.Air);

            var singleLayers = mats.Where(x => !x.IsMultiLayer).OrderBy(x => x.GetAverageColor(isv).GetColorDistanceSquared(c)).Take(40);
            var doubleLayers = mats.Where(x => x.IsMultiLayer).OrderBy(x => x.GetAverageColor(isv).GetColorDistanceSquared(c)).Take(10);

            mats = doubleLayers.Union(singleLayers)
                .Take(MAX_PULL)
                .ToList();

            List<ImageButtonData> items = new List<ImageButtonData>();
            items.Add(new ImageButtonData() {
                Data = air,
                Image = Textures.disabled,
                Text = Resources.Text.Nothing
            });
            items.AddRange(mats.Select(x => new ImageButtonData()
            {
                Data = x,
                Image = (x.GetImage(isv)),
                Text = x.ToString(),
            }));

            return items;
        }

        private void InitializeTabs()
        {
            pnlBottomMats.ImageButtons = GetImageButtonData_Lower();
            pnlTopMats.ImageButtons = GetImageButtonData_Upper();
            this.UpdateMaterialComboTab();
        }

        private Regex regexMatName = new Regex(@"minecraft:([a-zA-Z_09]+)(.*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private async Task SetSearchFilterAsync(string needle, ImageButtonContainer panel, IEnumerable<ImageButtonData> allTiles, CancellationToken? _worker = null)
        {
            bool isv = Options.IsSideView;
            bool isClosestMatchPanel = panel.Name == "pnlSimilarCombinations";
            if (isClosestMatchPanel)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(needle))
            {
                panel.DoFilterTakeOrderByOperation(
                    allTiles,
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

                panel.DoFilterTakeOrderByOperation(
                    allTiles, 
                    take: 20,
                    filter: x => x.GetData<Material>().IsVisibleF(this.Options) || x.Text == Resources.Text.Nothing,
                    orderBy: x => x.GetData<Material>().GetAverageColor(isv).GetColorDistanceSquared(cNeedle.Value)
                    );
                return;
                //await SetVisibleMaterials(found, _worker);
            }

            _worker?.SafeThrowIfCancellationRequested();
            await Task.Yield();

            int? idNeedle = needle.ToNullable<int>();
            //var foo = Materials.List.SelectMany(x => x.Tags).Distinct().ToArray();

            panel.DoFilterTakeOrderByOperation(
                allTiles,
                filter: mb =>
                {
                    Material x = mb.GetData<Material>();
                    if (x.IsAir) return true;

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
                },
                orderBy: x => this.MaterialOrders[x.GetData<Material>().PixelStackerID]
                );

            _worker?.SafeThrowIfCancellationRequested();
            await Task.Yield();
        }

        internal async Task SetCanvas(RenderedCanvas canvas)
        {
            this.Canvas = canvas;
            await SetSearchFilterAsync("", this.pnlBottomMats, GetImageButtonData_Lower());
            await SetSearchFilterAsync("", this.pnlTopMats, GetImageButtonData_Upper());
        }
    }
}