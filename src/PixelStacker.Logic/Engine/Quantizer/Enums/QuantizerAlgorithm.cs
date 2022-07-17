using SixLabors.ImageSharp.Processing;

namespace PixelStacker.Logic.Engine.Quantizer.Enums
{
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
