using PixelStacker.Core.Model.Drawing;

namespace PixelStacker.Core.Model.Mats
{
    [Serializable]
    //[JsonConverter(typeof(MaterialCombinationJsonConverter))]
    public class MaterialCombination : IEquatable<MaterialCombination>
    {
        #region Constructors
        public MaterialCombination(string pixelStackerID) : this(Materials.FromPixelStackerID(pixelStackerID)) { }

        public MaterialCombination(string pixelStackerIdBottom, string pixelStackerIdTop)
            : this(Materials.FromPixelStackerID(pixelStackerIdBottom), Materials.FromPixelStackerID(pixelStackerIdTop)) { }

        public MaterialCombination(Material m) : this(m, m) { }

        public MaterialCombination(Material mBottom, Material mTop)
        {
            Top = mTop;
            Bottom = mBottom;
            IsMultiLayer = !ReferenceEquals(Top, Bottom); //Top?.PixelStackerID != Bottom?.PixelStackerID;
            if (mBottom == null) throw new ArgumentNullException(nameof(mBottom));
            if (mTop == null) throw new ArgumentNullException(nameof(mTop));
        }
        #endregion Constructors

        public bool IsMultiLayer { get; }
        public Material Top { get; }
        public Material Bottom { get; }

        private PxColor? _GetAverageColorSide;
        private PxColor? _GetAverageColorTop;
        public PxColor GetAverageColor(bool isSide)
        {
            if (isSide)
            {
                _GetAverageColorSide ??= SideImage.GetAverageColor(1);
                return _GetAverageColorSide.Value;
            }
            else
            {
                _GetAverageColorTop ??= TopImage.GetAverageColor(1);
                return _GetAverageColorTop.Value;
            }
        }


        public int GetAverageColorDistance(bool isSide, PxColor target)
        {
            var colors = this.GetColorsInImage(isSide);

            long r = 0;
            long t = 0;

            foreach (var c in colors)
            {
                int dist = target.GetColorDistance(c.Item1);
                r += dist * c.Item2;
                t += c.Item2;
            }

            r /= t;
            return (int)r;
        }

        private List<Tuple<PxColor, int>> _ColorsInImageSide;
        private List<Tuple<PxColor, int>> _ColorsInImageTop;
        private List<Tuple<PxColor, int>> GetColorsInImage(bool isSide)
        {
            if (isSide)
            {
                _ColorsInImageSide ??= SideImage.ToList()
                    .GroupBy(x => x.Data)
                    .Select(x => new Tuple<PxColor, int>(x.Key, x.Count()))
                    .ToList();
                return _ColorsInImageSide;
            }
            else
            {
                _ColorsInImageTop ??= TopImage.ToList()
                    .GroupBy(x => x.Data)
                    .Select(x => new Tuple<PxColor, int>(x.Key, x.Count()))
                    .ToList();
                return _ColorsInImageTop;
            }
        }

        public PxBitmap GetImage(bool isSide) => isSide ? SideImage : TopImage;

        private PxBitmap _TopImage;
        public PxBitmap TopImage
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
                        _TopImage = Bottom.TopImage; //.To32bppBitmap();
                    }
                }

                return _TopImage;
            }
        }

        private PxBitmap _SideImage;
        public PxBitmap SideImage
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
                        _SideImage = Bottom.SideImage; //.To32bppBitmap();
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
            if (x is not null ^ y is not null) return false;
            if (x is null && y is null) return true;
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
            if (obj is MaterialCombination mc) return Equals(mc);
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
