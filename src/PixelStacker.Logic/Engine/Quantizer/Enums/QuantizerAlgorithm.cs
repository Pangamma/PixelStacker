using SixLabors.ImageSharp.Processing;

namespace PixelStacker.Logic.Engine.Quantizer.Enums
{
    /// <summary>
    /// 
    /// </summary>
    //public class QuantizerAlgorithm
    //{
    //    public const string HslDistinctSelection = "HSL distinct selection";
    //    public const string UniformQuantizer = "Uniform quantization";
    //    public const string Popularity = "Popularity algorithm";
    //    public const string MedianCut = "Median cut algorithm";
    //    public const string WuColor = "Wu's color quantizer";
    //    public const string Octree = "Octree quantization";
    //    public const string Neural = "Neural quantizer";
    //    public const string OptimalPalette = "Optimal palette";

    //    public static readonly string[] Values = new string[] {


    //        HslDistinctSelection, UniformQuantizer, Popularity, MedianCut, WuColor, Octree, Neural, OptimalPalette
    //    };
    //}

    public class QuantizerAlgorithm
    {
        public const string Werner = nameof(KnownQuantizers.Werner);
        public const string WebSafe = nameof(KnownQuantizers.WebSafe);
        public const string WuColor = nameof(KnownQuantizers.Wu);
        public const string Octree = nameof(KnownQuantizers.Octree);

        public static readonly string[] Values = new string[] {
            //nameof(KnownQuantizers.Werner),
            nameof(KnownQuantizers.Wu),
            nameof(KnownQuantizers.Octree),
            nameof(KnownQuantizers.WebSafe)
        };
    }
}
