using KdTree;
using PixelStacker.Logic.Collections;
using PixelStacker.Logic.Model;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelStacker.Logic.Extensions;
using System;

namespace PixelStacker.Tests
{
    [TestClass]
    public class UpdateCheckerTests
    {
        public UpdateCheckerTests()
        {
        }

        [TestMethod("Equal versions. No update needed.")]
        [TestCategory("Unit")]
        public void TestEqualVersions()
        {
            Assert.IsFalse(IsNewerVersionAvailable("1.19.2b", "1.19.2b"));
        }

        [TestMethod("New version available. Require an update.")]
        [TestCategory("Unit")]
        public void TestNewerVersion()
        {
            string api = "1.19.2b";
            string cur = "1.19.2b";

            api = "1.19.03c";
            Assert.IsTrue(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            api = "1.19.2c";
            Assert.IsTrue(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            api = "1.19.3";
            Assert.IsTrue(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            api = "1.19.3c";
            Assert.IsTrue(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            api = "1.20.0";
            Assert.IsTrue(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            api = "1.20.1";
            Assert.IsTrue(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            api = "1.20.1a";
            Assert.IsTrue(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            api = "2.1.0";
            Assert.IsTrue(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            api = "2.1.2f";
            Assert.IsTrue(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            api = "2.1.3";
            Assert.IsTrue(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            api = "2.01.3";
            Assert.IsTrue(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");
        }

        [TestMethod("Already on latest version. Skip this update.")]
        [TestCategory("Unit")]
        public void TestOlderVersion()
        {
            string api = "1.19.2b";
            string cur = "1.19.2b";

            cur = "1.19.03c";
            Assert.IsFalse(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            cur = "1.19.2c";
            Assert.IsFalse(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            cur = "1.19.3";
            Assert.IsFalse(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            cur = "1.19.3c";
            Assert.IsFalse(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            cur = "1.20.0";
            Assert.IsFalse(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            cur = "1.20.1";
            Assert.IsFalse(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            cur = "1.20.1a";
            Assert.IsFalse(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            cur = "2.1.0";
            Assert.IsFalse(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            cur = "2.1.2f";
            Assert.IsFalse(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            cur = "2.1.3";
            Assert.IsFalse(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            cur = "2.01.3";
            Assert.IsFalse(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");
        }

        [TestMethod("Bad API data")]
        [TestCategory("Unit")]
        public void TestBadInputForNewVersion()
        {
            string api = "";
            string cur = "2.01.3";
            Assert.IsFalse(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            api = "<some failure>";
            Assert.IsFalse(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");

            api = null;
            Assert.IsFalse(IsNewerVersionAvailable(api, cur), $"{api} > {cur}");
        }

        /// <sumapiNumry>
        /// This code is derived from the windows forms project's UpdateChecker class.
        /// </sumapiNumry>
        /// <param name="vApi"></param>
        /// <param name="vCurrent"></param>
        /// <returns></returns>
        private static bool IsNewerVersionAvailable(string vApi, string vCurrent)
        {
            if (string.IsNullOrWhiteSpace(vApi)) return false; // Bad NEW version? Better wait for the next one.
            if (string.IsNullOrWhiteSpace(vCurrent)) return true; // Bad version? Yikes. Recommend an upgrade.

            var api_arr = vApi.Split(".", StringSplitOptions.RemoveEmptyEntries);
            var cur_arr = vCurrent.Split(".", StringSplitOptions.RemoveEmptyEntries);

            // [1].19.2c
            if (api_arr.Length > 0 && cur_arr.Length > 0)
            {
                int apiNum = api_arr[0].ToNullable<int>() ?? 0;
                int curNum = cur_arr[0].ToNullable<int>() ?? 0;
                if (apiNum < curNum) return false;
                if (apiNum > curNum) return true;
            }

            // 1.[19].2c
            if (api_arr.Length > 1 && cur_arr.Length > 1)
            {
                int apiNum = api_arr[1].ToNullable<int>() ?? 0;
                int curNum = cur_arr[1].ToNullable<int>() ?? 0;
                if (apiNum < curNum) return false;
                if (apiNum > curNum) return true;
            }

            // 1.19.[2c]
            if (api_arr.Length > 2 && cur_arr.Length > 2)
            {
                int apiNum = new string(api_arr[2].Where(c => char.IsDigit(c)).ToArray()).ToNullable<int>() ?? 0;
                int curNum = new string(cur_arr[2].Where(c => char.IsDigit(c)).ToArray()).ToNullable<int>() ?? 0;
                if (apiNum < curNum) return false;
                if (apiNum > curNum) return true;

                string apiLetter = new string(api_arr[2].Where(c => char.IsLetter(c)).ToArray());
                string curLetter = new string(cur_arr[2].Where(c => char.IsLetter(c)).ToArray());
                int mC = apiLetter.CompareTo(curLetter);
                if (mC > 0) return true;
            }

            return false;
        }
    }
}
