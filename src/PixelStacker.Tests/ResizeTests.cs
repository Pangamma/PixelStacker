using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Tests
{
    [TestClass]
    public class ResizeTests
    {
        [TestMethod]
        public void NoResize()
        {
            int[] d = GetNewSize(200, 300, 400, 400);
            Assert.AreEqual(200, d[0]);
            Assert.AreEqual(300, d[1]);
        }

        [TestMethod]
        public void ProportionDownsize()
        {
            int[] d = GetNewSize(100, 200, 50, 100);
            Assert.AreEqual(50, d[0]);
            Assert.AreEqual(100, d[1]);
        }

        [TestMethod]
        public void Height()
        {
            int[] d = GetNewSize(100, 200, 50, 100);
            Assert.AreEqual(50, d[0]);
            Assert.AreEqual(100, d[1]);
        }


        public int[] GetNewSize(int Wdt, int Hgt, int Max_W, int Max_H)
        {
            int W_Result = Wdt;
            int H_Result = Hgt;
            if (Wdt > Max_W || Hgt > Max_H)
            {
                if (Max_W * Hgt < Max_H * Wdt)
                {

                    W_Result = Max_W;
                    H_Result = Hgt * Max_W / Wdt;
                }
                else
                {

                    W_Result = Wdt * Max_H / Hgt;
                    H_Result = Max_H;
                }
            }
            return new int[] { W_Result, H_Result };
        }
    }
}
