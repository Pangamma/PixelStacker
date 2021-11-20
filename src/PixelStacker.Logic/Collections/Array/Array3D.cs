using System;
using System.Collections;
using System.Collections.Generic;

namespace PixelStacker.Logic.Collections.Array
{
    /// <summary>
    /// https://discord.com/channels/143867839282020352/312132327348240384/895799131665215508
    /// </summary>
    [Obsolete]
    public struct Array3D<T> : IEnumerable<T>
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }
        private T[] Data;

        public Array3D(int d1, int d2, int d3)
        {
            Y = d1;
            X = d2;
            Z = d3;
            Data = new T[d1 * d2 * d3];
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var foo in Data)
            {
                yield return foo;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        public int GetLength(int n)
        {
            switch (n)
            {
                case 0: return X;
                case 1: return Y;
                case 2: return Z;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public static implicit operator Array3D<T>(T[,,] b)
        {
            int i = 0;
            var arr = new Array3D<T>(b.GetLength(0), b.GetLength(1), b.GetLength(2));
            foreach (var f in b)
                arr.Data[i++] = f;

            return arr;
        }

        public ref T this[int x, int y, int z] => ref Data[
            x
            + y * X
            + z * Y * X
            ];
    }
}
