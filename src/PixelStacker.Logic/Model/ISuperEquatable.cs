using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Model
{
    public interface ISuperEquatable<T> : IEquatable<T>
    {
        public bool Equals(object obj);

        public int GetHashCode();
    }
}
