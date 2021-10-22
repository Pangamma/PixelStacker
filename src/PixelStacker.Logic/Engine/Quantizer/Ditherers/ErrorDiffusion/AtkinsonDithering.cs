namespace PixelStacker.Logic.Engine.Quantizer.Ditherers.ErrorDiffusion
{
    public class AtkinsonDithering : BaseErrorDistributionDitherer
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
                { 0, 0, 0, 1, 1 },
                { 0, 1, 1, 1, 0 },
                { 0, 0, 1, 0, 0 }
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
