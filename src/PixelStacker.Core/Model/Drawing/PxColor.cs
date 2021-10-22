using System.Text;

namespace PixelStacker.Core.Model.Drawing
{
    public struct PxColor
    {
        /// <include file='doc\Color.uex' path='docs/doc[@for="Color.Empty"]/*' />
        /// <devdoc>
        ///    <para>[To be supplied.]</para>
        /// </devdoc>
        public static readonly PxColor Empty = new PxColor();

        //private static short StateKnownColorValid = 0x0001;
        private static short StateARGBValueValid = 0x0002;
        public static PxColor Transparent = PxColor.FromArgb(0, 255, 255, 255);

        //private static short StateValueMask = (short)(StateARGBValueValid);
        //private static short StateNameValid = 0x0008;
        //private static long NotDefinedValue = 0;

        /**
         * Shift count and bit mask for A, R, G, B components in ARGB mode!
         */
        private const int ARGBAlphaShift = 24;
        private const int ARGBRedShift = 16;
        private const int ARGBGreenShift = 8;
        private const int ARGBBlueShift = 0;


        // will contain standard 32bit sRGB (ARGB)
        //
        private readonly long value;


        private PxColor(long value)
        {
            this.value = value;
        }

        public byte R => (byte)(Value >> ARGBRedShift & 0xFF);
        public byte G => (byte)(Value >> ARGBGreenShift & 0xFF);
        public byte B => (byte)(Value >> ARGBBlueShift & 0xFF);
        public byte A => (byte)(Value >> ARGBAlphaShift & 0xFF);

        /// <include file='doc\Color.uex' path='docs/doc[@for="Color.Value"]/*' />
        /// <devdoc>
        ///     Actual color to be rendered.
        /// </devdoc>
        private long Value
        {
            get
            {
                return value;
            }
        }

        private static void CheckByte(int value, string name)
        {
            if (value < 0 || value > 255)
                throw new ArgumentOutOfRangeException(nameof(name), value, "Invalid value.");
        }

        /// <include file='doc\Color.uex' path='docs/doc[@for="Color.MakeArgb"]/*' />
        /// <devdoc>
        ///     Encodes the four values into ARGB (32 bit) format.
        /// </devdoc>
        private static long MakeArgb(byte alpha, byte red, byte green, byte blue)
        {
            return (long)unchecked((uint)(red << ARGBRedShift |
                         green << ARGBGreenShift |
                         blue << ARGBBlueShift |
                         alpha << ARGBAlphaShift)) & 0xffffffff;
        }

        /// <include file='doc\Color.uex' path='docs/doc[@for="Color.FromArgb"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Creates a Color from its 32-bit component
        ///       (alpha, red, green, and blue) values.
        ///    </para>
        /// </devdoc>
        public static PxColor FromArgb(int argb)
        {
            return new PxColor(argb & 0xffffffff);
        }

        /// <include file='doc\Color.uex' path='docs/doc[@for="Color.FromArgb1"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Creates a Color from its 32-bit component (alpha, red,
        ///       green, and blue) values.
        ///    </para>
        /// </devdoc>
        public static PxColor FromArgb(int alpha, int red, int green, int blue)
        {
            CheckByte(alpha, "alpha");
            CheckByte(red, "red");
            CheckByte(green, "green");
            CheckByte(blue, "blue");
            return new PxColor(MakeArgb((byte)alpha, (byte)red, (byte)green, (byte)blue));
        }

        /// <include file='doc\Color.uex' path='docs/doc[@for="Color.FromArgb3"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Creates a <see cref='PixelStacker.Core.Model.Drawing.PxColor'/> from the specified red, green, and
        ///       blue values.
        ///    </para>
        /// </devdoc>
        public static PxColor FromArgb(int red, int green, int blue)
        {
            return FromArgb(255, red, green, blue);
        }

        /// <summary>
        ///       Returns the Hue-Saturation-Lightness (HSL) lightness
        ///       for this <see cref='PixelStacker.Core.Model.Drawing.PxColor'/> .
        /// </summary>
        public float GetBrightness()
        {
            float r = R / 255.0f;
            float g = G / 255.0f;
            float b = B / 255.0f;

            float max, min;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            return (max + min) / 2;
        }

        /// <summary>
        ///       Returns the Hue-Saturation-Lightness (HSL) hue
        ///       value, in degrees, for this <see cref='PixelStacker.Core.Model.Drawing.PxColor'/> .  
        ///       If R == G == B, the hue is meaningless, and the return value is 0.
        /// </summary>
        public float GetHue()
        {
            if (R == G && G == B)
                return 0; // 0 makes as good an UNDEFINED value as any

            float r = R / 255.0f;
            float g = G / 255.0f;
            float b = B / 255.0f;

            float max, min;
            float delta;
            float hue = 0.0f;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            delta = max - min;

            if (r == max)
            {
                hue = (g - b) / delta;
            }
            else if (g == max)
            {
                hue = 2 + (b - r) / delta;
            }
            else if (b == max)
            {
                hue = 4 + (r - g) / delta;
            }
            hue *= 60;

            if (hue < 0.0f)
            {
                hue += 360.0f;
            }
            return hue;
        }

        /// <summary>
        ///       Returns the Hue-Saturation-Lightness (HSL) hue
        ///       value, in degrees, for this <see cref='PixelStacker.Core.Model.Drawing.PxColor'/> .  
        ///       If R == G == B, the hue is meaningless, and the return value is 0.
        /// </summary>
        public float GetHue(byte r, byte g, byte b)
        {
            if (r == g && g == b)
                return 0; // 0 makes as good an UNDEFINED value as any

            //float r = (float)R / 255.0f;
            //float g = (float)G / 255.0f;
            //float b = (float)B / 255.0f;

            float max, min;
            float delta;
            float hue = 0.0f;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            delta = max - min;

            if (r == max)
            {
                hue = (g - b) / delta;
            }
            else if (g == max)
            {
                hue = 2 + (b - r) / delta;
            }
            else if (b == max)
            {
                hue = 4 + (r - g) / delta;
            }
            hue *= 60;

            if (hue < 0.0f)
            {
                hue += 360.0f;
            }
            return hue;
        }

        /// <summary>
        ///   The Hue-Saturation-Lightness (HSL) saturation for this
        ///    <see cref='PixelStacker.Core.Model.Drawing.PxColor'/>
        /// </summary>
        public float GetSaturation()
        {
            float r = R / 255.0f;
            float g = G / 255.0f;
            float b = B / 255.0f;

            float max, min;
            float l, s = 0;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            // if max == min, then there is no color and
            // the saturation is zero.
            //
            if (max != min)
            {
                l = (max + min) / 2;

                if (l <= .5)
                {
                    s = (max - min) / (max + min);
                }
                else
                {
                    s = (max - min) / (2 - max - min);
                }
            }
            return s;
        }

        public int GetColorDistanceFaster(PxColor right)
        {
            int dR = R - right.R;
            int dG = G - right.G;
            int dB = B - right.B;
            float dHue = GetDegreeDistance(GetHue(), right.GetHue());
            return dR * dR
                + dG * dG
                + dB * dB
                + (int)(dHue * dHue * dHue / (dHue * dHue));
        }

        /// <include file='doc\Color.uex' path='docs/doc[@for="Color.ToArgb"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Returns the ARGB value of this <see cref='PixelStacker.Core.Model.Drawing.PxColor'/> .
        ///    </para>
        /// </devdoc>
        public int ToArgb()
        {
            return unchecked((int)Value);
        }

        /// <include file='doc\Color.uex' path='docs/doc[@for="Color.ToString"]/*' />
        /// <devdoc>
        ///    Converts this <see cref='PixelStacker.Core.Model.Drawing.PxColor'/> to a human-readable
        ///    string.
        /// </devdoc>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(32);
            sb.Append(GetType().Name);
            sb.Append(" [");
            sb.Append("A=");
            sb.Append(A);
            sb.Append(", R=");
            sb.Append(R);
            sb.Append(", G=");
            sb.Append(G);
            sb.Append(", B=");
            sb.Append(B);

            sb.Append("]");

            return sb.ToString();
        }

        private static float GetDegreeDistance(float alpha, float beta)
        {
            float phi = Math.Abs(beta - alpha) % 360;       // This is either the distance or 360 - distance
            float distance = phi > 180 ? 360 - phi : phi;
            return distance;
        }

        private static double GetDegreeDistance(double alpha, double beta)
        {
            double phi = Math.Abs(beta - alpha) % 360;       // This is either the distance or 360 - distance
            double distance = phi > 180 ? 360 - phi : phi;
            return distance;
        }

        public int GetColorDistance(PxColor right)
        {
            return this - right;
        }

        /// <summary>
        /// Overlay the TOP color ontop of the BOTTOM color
        /// </summary>
        /// <param name="RGBA2_Bottom"></param>
        /// <param name="RGBA1_Top"></param>
        /// <returns></returns>
        public PxColor OverlayColor(PxColor RGBA1_Top)
        {
            PxColor RGBA2_Bottom = this;
            double alpha = Convert.ToDouble(RGBA1_Top.A) / 255;
            int R = (int)(RGBA1_Top.R * alpha + RGBA2_Bottom.R * (1.0 - alpha));
            int G = (int)(RGBA1_Top.G * alpha + RGBA2_Bottom.G * (1.0 - alpha));
            int B = (int)(RGBA1_Top.B * alpha + RGBA2_Bottom.B * (1.0 - alpha));
            return FromArgb(255, R, G, B);
        }


        /// <summary>
        /// Does not normalize alpha channels
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public PxColor Normalize(int rgbBucketSize)
        {
            if (rgbBucketSize < 2)
            {
                return this;
            }

            int F = rgbBucketSize;
            int R = (int)Math.Min(255, Math.Round(Convert.ToDecimal(this.R) / F, 0) * F);
            int G = (int)Math.Min(255, Math.Round(Convert.ToDecimal(this.G) / F, 0) * F);
            int B = (int)Math.Min(255, Math.Round(Convert.ToDecimal(this.B) / F, 0) * F);

            return FromArgb(A, R, G, B);
        }








        public static int operator -(PxColor left, PxColor right)
        {
            if (left.value == right.value)
            {
                return 0;
            }

            int dR = left.R - right.R;
            int dG = left.G - right.G;
            int dB = left.B - right.B;
            int dHue = (int)GetDegreeDistance(left.GetHue(), right.GetHue());

            int diff =
                dR * dR
                + dG * dG
                + dB * dB
                + (int)Math.Sqrt(dHue * dHue * dHue)
                ;

            return diff;
        }

        public static bool operator ==(PxColor left, PxColor right)
        {
            if (left.value == right.value)
            {
                return true;
            }

            return false;
        }

        /// <include file='doc\Color.uex' path='docs/doc[@for="Color.operator!="]/*' />
        /// <devdoc>
        ///    <para>
        ///       Tests whether two specified <see cref='PixelStacker.Core.Model.Drawing.PxColor'/> objects
        ///       are equivalent.
        ///    </para>
        /// </devdoc>
        public static bool operator !=(PxColor left, PxColor right)
        {
            return !(left == right);
        }

        /// <include file='doc\Color.uex' path='docs/doc[@for="Color.Equals"]/*' />
        /// <devdoc>
        ///    Tests whether the specified object is a
        /// <see cref='PixelStacker.Core.Model.Drawing.PxColor'/> 
        /// and is equivalent to this <see cref='PixelStacker.Core.Model.Drawing.PxColor'/>.
        /// </devdoc>
        public override bool Equals(object obj)
        {
            if (obj is PxColor)
            {
                PxColor right = (PxColor)obj;
                if (value == right.value)
                {
                    return true;
                }
            }
            return false;
        }

        /// <include file='doc\Color.uex' path='docs/doc[@for="Color.GetHashCode"]/*' />
        /// <devdoc>
        ///    <para>[To be supplied.]</para>
        /// </devdoc>
        public override int GetHashCode()
        {
            return unchecked(value.GetHashCode());
        }
    }
}