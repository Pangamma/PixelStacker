﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Utilities
{
    public class ComboBoxItem
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public ComboBoxItem() { }
        public ComboBoxItem(string value, string text)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text.ToString();
        }
    }
}