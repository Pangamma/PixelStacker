using Newtonsoft.Json;
using PixelStacker.Extensions;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.IO.JsonConverters;
using PixelStacker.Resources;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelStacker.Logic.Model
{
    [Serializable]
    [JsonConverter(typeof(MaterialCombinationJsonConverter))]
    public class MaterialCombination : IEquatable<MaterialCombination>, IDisposable
    {
        #region Constructors
        [Obsolete("Do not create new mat combos willy nilly. It can cause a memory leak with the loading of images.", false)]
        public MaterialCombination(string pixelStackerID) : this(Materials.FromPixelStackerID(pixelStackerID)) { }

        [Obsolete("Do not create new mat combos willy nilly. It can cause a memory leak with the loading of images.", false)]
        public MaterialCombination(string pixelStackerIdBottom, string pixelStackerIdTop)
            : this(Materials.FromPixelStackerID(pixelStackerIdBottom), Materials.FromPixelStackerID(pixelStackerIdTop)) { }

        [Obsolete("Do not create new mat combos willy nilly. It can cause a memory leak with the loading of images.", false)]
        public MaterialCombination(Material m) : this(m, m) { }

        [Obsolete("Do not create new mat combos willy nilly. It can cause a memory leak with the loading of images.", false)]
        public MaterialCombination(Material mBottom, Material mTop)
        {
            this.Top = mTop;
            this.Bottom = mBottom;
            this.IsMultiLayer = !ReferenceEquals(Top, Bottom); //Top?.PixelStackerID != Bottom?.PixelStackerID;
            CheckValidity(mBottom, mTop);
        }
        #endregion Constructors

        private void CheckValidity(Material mBottom, Material mTop)
        {
            if (mBottom == null) throw new ArgumentNullException(nameof(mBottom));
            if (mTop == null) throw new ArgumentNullException(nameof(mTop));
            if (mTop.IsAir != mBottom.IsAir) throw new ArgumentException("If top or bottom are AIR, both must be AIR.");
            if (!mTop.CanBeUsedAsTopLayer && mBottom.PixelStackerID != mTop.PixelStackerID)
            {
                throw new ArgumentException("If top layer (glass) cannot be used as a top layer block, it means that both top and bottom " +
                    "should be the same type of material because the material combination represents both top and bottom layers " +
                    "being a bottom block material type.", nameof(mTop));
            }

            if (!mBottom.CanBeUsedAsBottomLayer && mBottom.PixelStackerID != mTop.PixelStackerID)
            {
                throw new ArgumentException("If bottom layer (dirt) cannot be used as a bottom layer block, it means that both top and bottom " +
                    "should be the same type of material because the material combination represents both top and bottom layers " +
                    "being a top block material type.", nameof(mBottom));
            }

            if (mBottom.CanBeUsedAsTopLayer == mTop.CanBeUsedAsTopLayer && mBottom.PixelStackerID != mTop.PixelStackerID)
            {
                throw new ArgumentException("Both top and bottom material types should match if they are both to go on the same layer.", nameof(mBottom));
            }

            if (mBottom.CanBeUsedAsBottomLayer == mTop.CanBeUsedAsBottomLayer && mBottom.PixelStackerID != mTop.PixelStackerID)
            {
                throw new ArgumentException("Both top and bottom material types should match if they are both to go on the same layer.", nameof(mBottom));
            }
        }
        public bool IsEnabled(Options opts)
        {
            return Top.IsEnabledF(opts) && Bottom.IsEnabledF(opts);
        }

        [Obsolete("This is a bad way to describe things.", false)]
        public bool IsMultiLayer { get; }
        public Material Top { get; }
        public Material Bottom { get; }


        private MaterialHeight? _ShadowHeight;
        public MaterialHeight GetShadowHeight()
        {
            if (this._ShadowHeight != null)
                return _ShadowHeight.Value;

            if (this.Bottom.IsAir)
                return (_ShadowHeight = MaterialHeight.L0_EMPTY).Value;

            if (this.Top.CanBeUsedAsTopLayer)
                return (_ShadowHeight = MaterialHeight.L2_MULTI).Value;

            return (_ShadowHeight = MaterialHeight.L1_SOLID).Value;
        }


        public SKColor GetAverageColor(bool isSide, SpecialCanvasRenderSettings specialRenderSettings)
        {
            int? Z = specialRenderSettings.ZLayerFilter;
            if (Z == null)
                return this.GetAverageColor(isSide);
            else if (Z == 0)
                return this.Bottom.GetAverageColor(isSide);
            else if (Z == 1)
                return this.Top.GetAverageColor(isSide);
            else
                return this.GetAverageColor(isSide);
        }


        private SKColor? _GetAverageColorSide;
        private SKColor? _GetAverageColorTop;
        public SKColor GetAverageColor(bool isSide)
        {
            if (isSide)
            {
                _GetAverageColorSide ??= SideImage.GetAverageColor();
                return _GetAverageColorSide.Value;
            }
            else
            {
                _GetAverageColorTop ??= TopImage.GetAverageColor();
                return _GetAverageColorTop.Value;
            }
        }

        private List<Tuple<SKColor, int>> _ColorsInImageSide;
        private List<Tuple<SKColor, int>> _ColorsInImageTop;
        public List<Tuple<SKColor, int>> GetColorsInImage(bool isSide)
        {
            if (isSide)
            {
                _ColorsInImageSide ??= SideImage.GetColorsInImage()
                    .GroupBy(x => x)
                    .Select(x => new Tuple<SKColor, int>(x.Key, x.Count()))
                    .ToList();
                return _ColorsInImageSide;
            }
            else
            {
                _ColorsInImageTop ??= TopImage.GetColorsInImage()
                    .GroupBy(x => x)
                    .Select(x => new Tuple<SKColor, int>(x.Key, x.Count()))
                    .ToList();
                return _ColorsInImageTop;
            }
        }

        public SKBitmap GetImage(bool isSide) => isSide ? this.SideImage : this.TopImage;


        public SKBitmap GetImage(bool isSide, SpecialCanvasRenderSettings srs)
        {
            if (srs.ZLayerFilter == null) return isSide ? this.SideImage : this.TopImage;
            else if (srs.ZLayerFilter == 1) return isSide ? this.Top.SideImage : this.Top.TopImage;
            else if (srs.ZLayerFilter == 0) return isSide ? this.Bottom.SideImage : this.Bottom.TopImage;
            else return isSide ? this.SideImage : this.TopImage;
        }

        private SKBitmap _TopImage;
        public SKBitmap TopImage
        {
            get
            {
                if (_TopImage == null)
                {
                    if (IsMultiLayer)
                    {
                        _TopImage = Bottom.TopImage.ToMergeStream(Top.TopImage, null, (x, y, cLower, cUpper) => cLower.OverlayColor(cUpper));
                    }
                    else
                    {
                        _TopImage = Bottom.TopImage.Copy();// To32bppBitmap();
                    }
                }

                return _TopImage;
            }
        }

        private SKBitmap _SideImage;
        private bool disposedValue;

        public SKBitmap SideImage
        {
            get
            {
                if (_SideImage == null)
                {
                    if (IsMultiLayer)
                    {
                        _SideImage = Bottom.SideImage.ToMergeStream(Top.SideImage, null, (x, y, cLower, cUpper) => cLower.OverlayColor(cUpper));
                    }
                    else
                    {
                        _SideImage = Bottom.SideImage.Copy(); //  To32bppBitmap();
                    }
                }

                return _SideImage;
            }
        }


        #region Equality/ Override methods
        public bool Equals(MaterialCombination y)
        {
            return this == y;
        }

        public static bool operator ==(MaterialCombination x, MaterialCombination y)
        {
            if ((x is not null) ^ (y is not null)) return false;
            if ((x is null) && (y is null)) return true;
            if (x.Top.PixelStackerID != y.Top.PixelStackerID) return false;
            if (x.Bottom.PixelStackerID != y.Bottom.PixelStackerID) return false;
            return true;
        }

        public static bool operator !=(MaterialCombination a, MaterialCombination b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is MaterialCombination mc) return this.Equals(mc);
            return false;
        }

        public int GetHashCode(MaterialCombination x)
        {
            return (x.Top.PixelStackerID + "::" + x.Bottom.PixelStackerID).GetHashCode();
        }

        public override string ToString()
        {
            return $"{Bottom.PixelStackerID}::{Top.PixelStackerID}";
        }

        public override int GetHashCode()
        {
            return GetHashCode(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects)
                    this._TopImage.DisposeSafely();
                    this._TopImage = null;

                    this._SideImage.DisposeSafely();
                    this._SideImage = null;
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                this._ColorsInImageSide = null;
                this._ColorsInImageTop = null;
                this._GetAverageColorSide = null;
                this._GetAverageColorTop = null;
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MaterialCombination()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion




        /// <summary>
        /// Can very easily return null. Be ready for it.
        /// Revise this method if we modify the palette
        /// to include glass only.
        /// 
        /// Replaces the current color on a canvas with the paint brush's color. However, it also takes into account z layer filters that affect
        /// which layer the paint will be applied to.
        /// 
        /// If z layer is at top layer, 
        /// </summary>
        /// <param name="layerFilter"></param>
        /// <param name="palette"></param>
        /// <param name="primaryColor"></param>
        /// <param name="toReplace"></param>
        /// <returns></returns>
        public static MaterialCombination GetMcToPaintWith(ZLayer layerFilter, MaterialPalette palette, MaterialCombination primaryColor, MaterialCombination toReplace)
        {
            // Change both
            if (layerFilter == ZLayer.Both)
            {
                return primaryColor;
            }

            // No change
            if (toReplace.Top == primaryColor.Top && toReplace.Bottom == primaryColor.Bottom)
            {
                return toReplace;
            }

            Material nTop = toReplace.Top;
            Material nBottom = toReplace.Bottom;


            if (ZLayer.Top == layerFilter)
            {
                if (primaryColor.Top.CanBeUsedAsTopLayer)
                {
                    nTop = primaryColor.Top;
                }
                else if (primaryColor.Top.IsAir || !primaryColor.Top.CanBeUsedAsTopLayer)
                {
                    nTop = Materials.Air;
                }
                else
                {
                    throw new Exception("Unhandled.");
                }

                if (!toReplace.Bottom.CanBeUsedAsBottomLayer && !toReplace.Bottom.IsAir)
                {
                    nBottom = nTop;
                }
            }

            if (ZLayer.Bottom == layerFilter)
            {
                if (primaryColor.Bottom.CanBeUsedAsBottomLayer)
                {
                    nBottom = primaryColor.Bottom;
                }
                else if (primaryColor.Bottom.IsAir || !primaryColor.Bottom.CanBeUsedAsBottomLayer)
                {
                    nBottom = Materials.Air;
                }
                else
                {
                    throw new Exception("Unhandled bottom layer if/else condition.");
                }

                if (!toReplace.Top.CanBeUsedAsTopLayer && !toReplace.Bottom.IsAir)
                {
                    nTop = nBottom;
                }
            }

            // Copy non-air block to other slot.
            if (nTop.IsAir != nBottom.IsAir)
            {
                if (nTop.IsAir)
                {
                    nTop = nBottom;
                }
                else
                {
                    nBottom = nTop;
                }
            }

            return palette.GetMaterialCombinationByMaterials(nBottom, nTop);
        }
    }
}
