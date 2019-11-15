using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.PreRender.Extensions
{
    public static class ExtendMath
    {
        public static int Pow(this int num, int exp)
        {
            int result = 1;

            while (exp > 0)
            {
                if (exp % 2 == 1)
                    result *= num;
                exp >>= 1;
                num *= num;
            }

            return result;
        }

        public static int Pow2(this int num)
        {
            return num * num;
        }

        public static float Pow2(this float num)
        {
            return num * num;
        }
    }
}
