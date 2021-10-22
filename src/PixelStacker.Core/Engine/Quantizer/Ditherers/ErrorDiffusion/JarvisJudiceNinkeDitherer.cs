using PixelStacker.Core.Engine.Quantizer.Ditherers;
using System;

namespace PixelStacker.Core.Engine.Quantizer.Ditherers.ErrorDiffusion
{
    public class JarvisJudiceNinkeDitherer : BaseErrorDistributionDitherer
    {
        /// <summary>
        /// See <see cref="BaseColorDitherer.CreateCoeficientMatrix"/> for more details.
        /// </summary>
        protected override byte[,] CreateCoeficientMatrix()
        {
            return new byte[,]
            {
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 7, 5 },
                { 3, 5, 7, 5, 3 },
                { 1, 3, 5, 3, 1 }
            };
        }

        /// <summary>
        /// See <see cref="BaseErrorDistributionDitherer.MatrixSideWidth"/> for more details.
        /// </summary>
        protected override int MatrixSideWidth
        {
            get { return 2; }
        }

        /// <summary>
        /// See <see cref="BaseErrorDistributionDitherer.MatrixSideHeight"/> for more details.
        /// </summary>
        protected override int MatrixSideHeight
        {
            get { return 2; }
        }
    }
}
