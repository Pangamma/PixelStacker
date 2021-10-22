using System;
using PixelStacker.Core.Engine.Quantizer.Helpers;
using PixelStacker.Core.Model.Drawing;

namespace PixelStacker.Core.Engine.Quantizer.Ditherers.ErrorDiffusion
{
    public abstract class BaseErrorDistributionDitherer : BaseColorDitherer
    {
        #region | Properties |

        /// <summary>
        /// Gets the width of the matrix side.
        ///
        ///         center
        ///           v --------> side width = 2
        /// | 0 | 1 | 2 | 3 | 4 |
        /// </summary>
        protected abstract int MatrixSideWidth { get; }

        /// <summary>
        /// Gets the height of the matrix side.
        /// 
        /// --- 
        ///  0  
        /// --- 
        ///  1  &lt;- center
        /// --- | 
        ///  2  | side height = 1
        /// --- v
        /// </summary>
        protected abstract int MatrixSideHeight { get; }

        #endregion

        #region | Methods |

        private void ProcessNeighbor(Pixel targetPixel, int x, int y, float factor, int redError, int greenError, int blueError)
        {
            PxColor oldColor = TargetBuffer.ReadColorUsingPixelFrom(targetPixel, x, y);
            oldColor = QuantizationHelper.ConvertAlpha(oldColor);
            int red = GetClampedColorElementWithError(oldColor.R, factor, redError);
            int green = GetClampedColorElementWithError(oldColor.G, factor, greenError);
            int blue = GetClampedColorElementWithError(oldColor.B, factor, blueError);
            PxColor newColor = PxColor.FromArgb(255, red, green, blue);
            TargetBuffer.WriteColorUsingPixelAt(targetPixel, x, y, newColor, Quantizer);
        }

        #endregion

        #region << BaseColorDitherer >>

        /// <summary>
        /// See <see cref="BaseColorDitherer.OnProcessPixel"/> for more details.
        /// </summary>
        protected override bool OnProcessPixel(Pixel sourcePixel, Pixel targetPixel)
        {
            // only process dithering when reducing from truecolor to indexed
            if (!TargetBuffer.IsIndexed) return false;

            // retrieves the colors
            PxColor sourceColor = SourceBuffer.GetColorFromPixel(sourcePixel);
            PxColor targetColor = TargetBuffer.GetColorFromPixel(targetPixel);

            // converts alpha to solid color
            sourceColor = QuantizationHelper.ConvertAlpha(sourceColor);

            // calculates the difference (error)
            int redError = sourceColor.R - targetColor.R;
            int greenError = sourceColor.G - targetColor.G;
            int blueError = sourceColor.B - targetColor.B;

            // only propagate non-zero error
            if (redError != 0 || greenError != 0 || blueError != 0)
            {
                // processes the matrix
                for (int shiftY = -MatrixSideHeight; shiftY <= MatrixSideHeight; shiftY++)
                    for (int shiftX = -MatrixSideWidth; shiftX <= MatrixSideWidth; shiftX++)
                    {
                        int targetX = sourcePixel.X + shiftX;
                        int targetY = sourcePixel.Y + shiftY;
                        byte coeficient = CachedMatrix[shiftY + MatrixSideHeight, shiftX + MatrixSideWidth];
                        float coeficientSummed = CachedSummedMatrix[shiftY + MatrixSideHeight, shiftX + MatrixSideWidth];

                        if (coeficient != 0 &&
                            targetX >= 0 && targetX < TargetBuffer.Width &&
                            targetY >= 0 && targetY < TargetBuffer.Height)
                        {
                            ProcessNeighbor(targetPixel, targetX, targetY, coeficientSummed, redError, greenError, blueError);
                        }
                    }
            }

            // pixels are not processed, only neighbors are
            return false;
        }

        #endregion

        #region << IColorDitherer >>

        /// <summary>
        /// See <see cref="IColorDitherer.IsInplace"/> for more details.
        /// </summary>
        public override bool IsInplace
        {
            get { return false; }
        }

        #endregion
    }
}
