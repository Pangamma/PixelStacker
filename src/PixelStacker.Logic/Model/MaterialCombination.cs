using Newtonsoft.Json;
using PixelStacker.Extensions;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.JsonConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace PixelStacker.Logic.Model
{
    [Serializable]
    [JsonConverter(typeof(MaterialCombinationJsonConverter))]
    public class MaterialCombination : IEquatable<MaterialCombination>
    {
        #region Constructors
        public MaterialCombination(string pixelStackerID) : this(Materials.FromPixelStackerID(pixelStackerID)) { }

        public MaterialCombination(string pixelStackerIdBottom, string pixelStackerIdTop)
            : this(Materials.FromPixelStackerID(pixelStackerIdBottom), Materials.FromPixelStackerID(pixelStackerIdTop)) { }

        public MaterialCombination(Material m) : this(m, m) { }

        public MaterialCombination(Material mBottom, Material mTop)
        {
            this.Top = mTop;
            this.Bottom = mBottom;
            this.IsMultiLayer = !ReferenceEquals(Top, Bottom); //Top?.PixelStackerID != Bottom?.PixelStackerID;
            if (mBottom == null) throw new ArgumentNullException(nameof(mBottom));
            if (mTop == null) throw new ArgumentNullException(nameof(mTop));
        }
        #endregion Constructors

        public bool IsMultiLayer { get; }
        public Material Top { get; }
        public Material Bottom { get; }

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

        #endregion
    }
}
