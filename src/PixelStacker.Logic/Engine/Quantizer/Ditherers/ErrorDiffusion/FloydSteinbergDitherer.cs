namespace PixelStacker.Logic.Engine.Quantizer.Ditherers.ErrorDiffusion
{
    public class FloydSteinbergDitherer : BaseErrorDistributionDitherer
    {
        /// <summary>
        /// See <see cref="BaseColorDitherer.CreateCoeficientMatrix"/> for more details.
        /// </summary>
        protected override byte[,] CreateCoeficientMatrix()
        {
            return new byte[,]
            {
                { 0, 0, 0 },
                { 0, 0, 7 },
                { 3, 5, 1 }
            };
        }

        /// <summary>
        /// See <see cref="BaseErrorDistributionDitherer.MatrixSideWidth"/> for more details.
        /// </summary>
        protected override int MatrixSideWidth
        {
            get { return 1; }
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
