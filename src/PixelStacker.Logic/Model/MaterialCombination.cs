using Newtonsoft.Json;
using PixelStacker.Extensions;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.JsonConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using PixelStacker.Logic.IO.Config;

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
            if (mBottom == null) throw new ArgumentNullException(nameof(mBottom));
            if (mTop == null) throw new ArgumentNullException(nameof(mTop));
        }
        #endregion Constructors

        public bool IsEnabled(Options opts)
        {
            return Top.IsEnabledF(opts) && Bottom.IsEnabledF(opts);
        }

        public bool IsMultiLayer { get; }
        public Material Top { get; }
        public Material Bottom { get; }

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


        public SKBitmap GetImage(bool isSide, SpecialCanvasRenderSettings srs) {
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

        protected virtual void Dispose(bool disposing)
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
    }
}
