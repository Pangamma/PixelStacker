﻿using PixelStacker.Core.Engine.Quantizer.Ditherers;
using System;

namespace PixelStacker.Core.Engine.Quantizer.Ditherers.ErrorDiffusion
{
    public class FanDitherer : BaseErrorDistributionDitherer
    {
        /// <summary>
        /// See <see cref="BaseColorDitherer.CreateCoeficientMatrix"/> for more details.
        /// </summary>
        protected override byte[,] CreateCoeficientMatrix()
        {
            return new byte[,]
            {
                { 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 8, 0, 0 },
                { 1, 1, 2, 4, 0, 0, 0 }
            };
        }

        /// <summary>
        /// See <see cref="BaseErrorDistributionDitherer.MatrixSideWidth"/> for more details.
        /// </summary>
        protected override int MatrixSideWidth
        {
            get { return 3; }
        }

        /// <summary>
        /// See <see cref="BaseErrorDistributionDitherer.MatrixSideHeight"/> for more details.
        /// </summary>
        protected override int MatrixSideHeight
        {
            get { return 1; }
        }
    }
}