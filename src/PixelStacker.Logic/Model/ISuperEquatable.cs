using System;

namespace PixelStacker.Logic.Model
{
    public interface ISuperEquatable<T> : IEquatable<T>
    {
        public bool Equals(object obj);

        public int GetHashCode();
    }
}
