using System;

namespace PixelStacker.Logic.Model
{
    public class PxPoint : IEquatable<PxPoint>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public PxPoint() { }
        public PxPoint(SkiaSharp.SKPoint p) { X = (int)p.X; Y = (int)p.Y; }
        public PxPoint(int x, int y) { X = x; Y = y; }
        public PxPoint(float x, float y) { X = (int)x; Y = (int)y; }

        public PxPoint Clone() => new PxPoint(X, Y);

        public override bool Equals(object obj)
        {
            return obj is PxPoint point &&
                   X == point.X &&
                   Y == point.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public bool Equals(PxPoint other)
        {
            if (other is null) return false;
            return other.X == X && Y == other.Y;
        }
    }
}
