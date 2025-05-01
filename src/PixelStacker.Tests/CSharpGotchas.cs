using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;

namespace PixelStacker.Tests
{
    public class CustomData
    {
        private int _value = 0;
        public int Value { get => _value; set {
            Debug.WriteLine($"Value for {InstanceId} set to {value}");
            _value = value;
        } }

        private int InstanceId;
        private static int MaxInstanceId = 0;

        public CustomData()
        {
            this.InstanceId = MaxInstanceId++;
            Debug.WriteLine($"I was constructed. {this.InstanceId}");
        }
    }

    [TestClass]
    public class CSharpGotchas
    {
        [TestMethod]
        public void NoHintsGiven()
        {
            var list = new int[] { 3, 3, 3, 3 }.Select((n) => new CustomData() { Value = n });
            
            int dI = 0;
            foreach(var data in list)
            {
                // Make your modifications here
                data.Value = dI++;
            }

            int actual = list.First().Value;

            #region // What should the expected value be? And why?
            int EXPECTED = 3;
            #endregion

            Assert.AreEqual(EXPECTED, actual);
        }
    }
}
