using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using SkiaSharp;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace PixelStacker.Logic.Model
{
    public class MaterialCombinationHelper
    {
        private enum TransparencyLevel
        {
            Invisible,
            SemiTransparent,
            Opaque
        }

        private static TransparencyLevel GetTransparencyLevel_Bottom(Material m, int? zLayerFilter, bool isGhost, bool showGhosts)
        {
            if (m.IsAir) return TransparencyLevel.Invisible;
            if (!m.CanBeUsedAsBottomLayer) return TransparencyLevel.Invisible;
            if (zLayerFilter == 1) return TransparencyLevel.Invisible;
            if (isGhost) return showGhosts ? TransparencyLevel.SemiTransparent : TransparencyLevel.Invisible;
            return TransparencyLevel.Opaque;
        }

        private static TransparencyLevel GetTransparencyLevel_Top(Material m, int? zLayerFilter, bool isGhost, bool showGhosts)
        {
            if (m.IsAir) return TransparencyLevel.Invisible;
            if (!m.CanBeUsedAsTopLayer) return TransparencyLevel.Invisible;
            if (zLayerFilter == 0) return TransparencyLevel.Invisible;
            if (isGhost) return showGhosts ? TransparencyLevel.SemiTransparent : TransparencyLevel.Invisible;
            return TransparencyLevel.SemiTransparent;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DrawRects(SKCanvas canvas, IEnumerable<SKRect> rects, SKPaint paint)
        {
            foreach (var tileRect in rects)
            {
                canvas.DrawRect(tileRect, paint);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DrawImages(SKCanvas canvas, IEnumerable<SKRect> rects, SKImage image)
        {
            using var paint = new SKPaint { BlendMode = SKBlendMode.SrcOver };
            foreach (var tileRect in rects)
            {
                canvas.DrawImage(image, tileRect, Constants.SAMPLE_OPTS_NONE, paint);
            }
        }

        // TODO: Remove code in materialcombination file AFTER confirming   its logic matches the new logic. (It doesn't)

        public static void PaintOntoCanvas(SKCanvas canvas, IEnumerable<SKRect> rects, MaterialCombination mc, bool isSide, IReadonlyCanvasViewerSettings srs, bool forcePaintEmpty)
        {
            bool isTopGhost = srs.VisibleMaterialsFilter.Count > 0 && !srs.VisibleMaterialsFilter.Contains(mc.Top.PixelStackerID);
            bool isBottomGhost = srs.VisibleMaterialsFilter.Count > 0 && !srs.VisibleMaterialsFilter.Contains(mc.Bottom.PixelStackerID);
            bool showIfGhost = srs.ShowFilteredMaterials;
            var transTop = GetTransparencyLevel_Top(mc.Top, srs.ZLayerFilter, isTopGhost, showIfGhost);
            var transBottom = GetTransparencyLevel_Bottom(mc.Bottom, srs.ZLayerFilter, isBottomGhost, showIfGhost);
            bool paintEmpty = transTop == TransparencyLevel.Invisible && transBottom == TransparencyLevel.Invisible;

            if (paintEmpty || forcePaintEmpty)
            {
                using var primer = new SKPaint() { BlendMode = SKBlendMode.Src, Color = new SKColor(0, 0, 0, 0) };
                DrawRects(canvas, rects, primer);
                if (paintEmpty)
                    return;
            }

            if (TransparencyLevel.Opaque == transBottom && TransparencyLevel.SemiTransparent == transTop)
            {
                if (srs.IsSolidColors)
                {
                    var color = mc.GetAverageColor(isSide);
                    using var paint = new SKPaint() { BlendMode = SKBlendMode.Src, Color = color };
                    DrawRects(canvas, rects, paint);
                }
                else
                {
                    using var image = SKImage.FromBitmap(mc.GetImage(isSide));
                    DrawImages(canvas, rects, image);
                }

                return;
            }

            bool usePrimer = !paintEmpty && !mc.Top.IsAir && !(transTop == TransparencyLevel.Opaque || transBottom == TransparencyLevel.Opaque);
            if (usePrimer)
            {
                using var primer = new SKPaint() { BlendMode = SKBlendMode.Src, Color = new SKColor(207, 207, 207) };
                DrawRects(canvas, rects, primer);
            }

            if (TransparencyLevel.Invisible != transBottom)
            {
                if (srs.IsSolidColors || isBottomGhost)
                {
                    var color = mc.Bottom.GetAverageColor(isSide);
                    if (isBottomGhost) color = color.ToGhostColor();
                    using var paint = new SKPaint() { BlendMode = SKBlendMode.SrcOver, Color = color };
                    DrawRects(canvas, rects, paint);
                }
                else
                {
                    using var image = SKImage.FromBitmap(mc.Bottom.GetImage(isSide));
                    DrawImages(canvas, rects, image);
                }
            }

            if (TransparencyLevel.Invisible != transTop)
            {
                if (srs.IsSolidColors || isTopGhost)
                {
                    var color = mc.Top.GetAverageColor(isSide);
                    if (isTopGhost) color = color.ToGhostColor();
                    using var paint = new SKPaint() { BlendMode = SKBlendMode.SrcOver, Color = color };
                    DrawRects(canvas, rects, paint);
                }
                else
                {
                    using var image = SKImage.FromBitmap(mc.Top.GetImage(isSide));
                    DrawImages(canvas, rects, image);
                }
            }
        }
    }
}
