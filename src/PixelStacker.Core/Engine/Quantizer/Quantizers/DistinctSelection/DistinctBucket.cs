﻿using System;
using System.Collections.Generic;
using PixelStacker.Core.Model.Drawing;
using System.Linq;

namespace PixelStacker.Core.Engine.Quantizer.Quantizers.DistinctSelection
{
    public class DistinctBucket
    {
        public DistinctColorInfo ColorInfo { get; private set; }
        public DistinctBucket[] Buckets { get; private set; }

        public DistinctBucket()
        {
            Buckets = new DistinctBucket[16];
        }

        public void StoreColor(PxColor color)
        {
            int redIndex = color.R >> 5;
            DistinctBucket redBucket = Buckets[redIndex];

            if (redBucket == null)
            {
                redBucket = new DistinctBucket();
                Buckets[redIndex] = redBucket;
            }

            int greenIndex = color.G >> 5;
            DistinctBucket greenBucket = redBucket.Buckets[greenIndex];

            if (greenBucket == null)
            {
                greenBucket = new DistinctBucket();
                redBucket.Buckets[greenIndex] = greenBucket;
            }

            int blueIndex = color.B >> 5;
            DistinctBucket blueBucket = greenBucket.Buckets[blueIndex];

            if (blueBucket == null)
            {
                blueBucket = new DistinctBucket();
                greenBucket.Buckets[blueIndex] = blueBucket;
            }

            DistinctColorInfo colorInfo = blueBucket.ColorInfo;

            if (colorInfo == null)
            {
                colorInfo = new DistinctColorInfo(color);
                blueBucket.ColorInfo = colorInfo;
            }
            else
            {
                colorInfo.IncreaseCount();
            }
        }

        public List<DistinctColorInfo> GetValues()
        {
            return Buckets.Where(red => red != null).
                SelectMany(redBucket => redBucket.Buckets.
                Where(green => green != null), (redBucket, greenBucket) => greenBucket).
                SelectMany(greenBucket => greenBucket.Buckets.
                Where(blue => blue != null), (greenBucket, blueBucket) => blueBucket.ColorInfo).
                ToList();
        }
    }
}