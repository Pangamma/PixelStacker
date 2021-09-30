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
    public class BSTTest
    {

        [TestMethod]
        [TestCategory("TDD")]
        public void NewHighestItem()
        {
        }

        [TestMethod]
        [TestCategory("TDD")]
        public void N()
        {
            for (int n = 0; n < 20; n++)
                Assert.AreEqual(SumOf1ThroughN(n), SumOf1ThroughNFast(n), $"N =" + n + " FAIL");
        }

        public int SumOf1ThroughN(int n)
        {
            if (n == 0) return 0;
            int m = n + SumOf1ThroughN(n - 1);
            return m;
        }

        public int SumOf1ThroughNFast(int n)
        {
            int m = (n * (n + 1)) / 2;
            return m;
        }

        [TestMethod]
        [TestCategory("TDD")]
        public void SearchTest()
        {
            int sum = 0;
            int guesses = 0;
            CustomBinarySearch(1, 8, ref sum, ref guesses);
            for (int n = 100; n < 101; n++)
            {
                sum = 0;
                guesses = 0;
                CustomBinarySearch(1, n, ref sum, ref guesses);
            }
        }

        public void CustomBinarySearch(int L, int R, ref int sum, ref int numGuesses)
        {
            while (L <= R)
            {
                int mid = (L + R) / 2;
                int vMid = mid;
                int key = R;
                if (key == vMid)
                {
                    return;
                    //?? Why?
                    //return ++mid;
                } 
                else if (key >= vMid)
                {
                    numGuesses++;
                    sum += mid;
                    L = mid + 1;
                }
                else
                {
                    numGuesses++;
                    sum += mid;
                    R = mid - 1;
                }
            }
        }


        //public float GetClosestHueSlow(float f)
        //{
        //    return this.Hues.MinBy(x => GetDegreeDistance(f, x));
        //}

        //public float GetClosestHueFast(float fNeedle)
        //{
        //    if (this.Hues.Count < 2)
        //    {
        //        // Intended explosion if input array is empty.
        //        return this.Hues.First();
        //    }

        //    // Find location to insert using binary search
        //    int indexF = this.Hues.BinarySearch(fNeedle);

        //    // Exact match found? Weird. Accept it though.
        //    if (indexF >= 0) return this.Hues[indexF];

        //    float vRight; float vLeft;
        //    indexF = Math.Abs(indexF) - 1; // undo "compliment"

        //    if (indexF == this.Hues.Count || indexF == 0)
        //    {
        //        // LOWEST (or highest) VALUE!
        //        // Either it fell off the TOP, or it fell off the BOTTOM.
        //        vRight = this.Hues.First();
        //        vLeft = this.Hues.Last();
        //    }
        //    else
        //    {
        //        vRight = this.Hues[indexF];
        //        vLeft = this.Hues[indexF - 1];
        //    }

        //    if  (GetDegreeDistance(vRight, fNeedle) < GetDegreeDistance(vLeft, fNeedle))
        //    {
        //        return vRight;
        //    }
        //    else
        //    {
        //        return vLeft;
        //    }
        //}

    }
}
