using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using PixelStacker.Logic.Extensions;
using System.Threading;
using System.Globalization;
using System.Linq;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace PixelStacker.Tools
{
    [TestClass]
    public class ColorMatchTester
    {

        List<float> Hues = new List<float>();

        [TestInitialize]
        [TestCategory("TDD")]
        public void Init()
        {
            Hues.Add(0.5F);
            Hues.Add(5);
            Hues.Add(90);
            Hues.Add(179);
            Hues.Add(180);
            Hues.Add(181);
            Hues.Add(359);
        }

        [TestMethod]
        [TestCategory("TDD")]
        public void Test()
        {
            var r = new Random();
            for (int i = 0; i < 2000; i++)
            {
                float fTest = (float) r.NextDouble() * 360;
                TestHelper(fTest);
            }
        }

        private void TestHelper(float fTest)
        {
            var expected = GetClosestHueFast(fTest);
            var actual = GetClosestHueSlow(fTest);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("TDD")]
        public void NewHighestItem()
        {
            TestHelper(360);
        }

        [TestMethod]
        [TestCategory("TDD")]
        public void NewLowestItem()
        {
            TestHelper(0);
        }

        [TestMethod]
        [TestCategory("TDD")]
        public void IndexOne()
        {
            TestHelper(1);
        }

        public float GetClosestHueSlow(float f)
        {
            return this.Hues.MinBy(x => GetDegreeDistance(f, x));
        }

        public float GetClosestHueFast(float fNeedle)
        {
            if (this.Hues.Count < 2)
            {
                // Intended explosion if input array is empty.
                return this.Hues.First();
            }

            // Find location to insert using binary search
            int indexF = this.Hues.BinarySearch(fNeedle);

            // Exact match found? Weird. Accept it though.
            if (indexF >= 0) return this.Hues[indexF];

            float vRight; float vLeft;
            indexF = Math.Abs(indexF) - 1; // undo "compliment"

            if (indexF == this.Hues.Count || indexF == 0)
            {
                // LOWEST (or highest) VALUE!
                // Either it fell off the TOP, or it fell off the BOTTOM.
                vRight = this.Hues.First();
                vLeft = this.Hues.Last();
            }
            else
            {
                vRight = this.Hues[indexF];
                vLeft = this.Hues[indexF - 1];
            }

            if  (GetDegreeDistance(vRight, fNeedle) < GetDegreeDistance(vLeft, fNeedle))
            {
                return vRight;
            }
            else
            {
                return vLeft;
            }
        }

        public static float GetDegreeDistance(float alpha, float beta)
        {
            float phi = Math.Abs(beta - alpha) % 360;       // This is either the distance or 360 - distance
            float distance = phi > 180 ? 360 - phi : phi;
            return distance;
        }
    }
}
