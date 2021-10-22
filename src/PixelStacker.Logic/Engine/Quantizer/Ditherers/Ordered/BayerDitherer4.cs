namespace PixelStacker.Logic.Engine.Quantizer.Ditherers.Ordered
{
    public class BayerDitherer4 : BaseOrderedDitherer
    {
        /// <summary>
        /// See <see cref="BaseColorDitherer.CreateCoeficientMatrix"/> for more details.
        /// </summary>
        protected override byte[,] CreateCoeficientMatrix()
        {
            return new byte[,]
            {
                {  1,  9,  3, 11 },
                { 13,  5, 15,  7 },
                {  4, 12,  2, 10 },
                { 16,  8, 14,  6 }
            };
        }

        /// <summary>
        /// See <see cref="BaseOrderedDitherer.MatrixWidth"/> for more details.
        /// </summary>
        protected override byte MatrixWidth
        {
            get { return 4; }
        }

        /// <summary>
        /// See <see cref="BaseOrderedDitherer.MatrixHeight"/> for more details.
        /// </summary>
        protected override byte MatrixHeight
        {
            get { return 4; }
        }
    }
}
