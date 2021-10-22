namespace PixelStacker.Core.Model.Drawing
{
    public class PxPoint : IEquatable<PxPoint>
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public PxPoint(int x, int y)
        {
            X = x; Y = y;
        }

        public bool Equals(PxPoint other)
        {
            return X == other?.X && Y == other?.Y;
        }
    }
}
