//using SkiaSharp;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PixelStacker.Extensions
//{
//    public static class SkiaExtensions
//    {
//        //public static SKBitmap ToSKBitmap(this Bitmap trash)
//        //{
//        //    using (var ms = new MemoryStream())
//        //    {
//        //        trash.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
//        //        ms.Seek(0, SeekOrigin.Begin);
//        //        var bm = SKBitmap.Decode(ms);
//        //        return bm;
//        //    }
//        //}

//        public static SKRect ToRectangle(this SKPoint L, SKPoint R)
//        {
//            return new SKRect(
//                left: Math.Min(L.X, R.X),
//                top: Math.Min(L.Y, R.Y),
//                right: Math.Max(L.X, R.X),
//                bottom: Math.Max(L.Y, R.Y));
//        }
//    }
//}
