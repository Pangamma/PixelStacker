using System;
using System.Collections;
using System.Collections.Generic;

namespace PixelStacker.Logic.Collections.Array
{
    /// <summary>
    /// https://discord.com/channels/143867839282020352/312132327348240384/895799131665215508
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Obsolete]
    public struct Array2D<T> : IEnumerable<T>
    {
        public int A { get; }
        public int B { get; }
        private T[] Data;

        public Array2D(int d1, int d2)
        {
            B = d1;
            A = d2;
            Data = new T[d1 * d2];
        }

        public int GetLength(int n)
        {
            switch (n)
            {
                case 0: return B;
                case 1: return A;
                default: throw new ArgumentOutOfRangeException();
            }
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

        //public static implicit operator T[,](Array2D<T> b)
        //{
        //    int i = 0;
        //    T[,] o = new T[b.Width, b.Height];
        //    foreach(var v in b)
        //    {
        //        o[i++] = v;
        //    }

        //    var arr = new Array2D<T>(b.GetLength(0), b.GetLength(1));
        //    foreach (var f in b)
        //        arr.Data[i++] = f;

        //    return arr;
        //}


        public static implicit operator Array2D<T>(T[,] b)
        {
            int i = 0;
            var arr = new Array2D<T>(b.GetLength(0), b.GetLength(1));
            foreach (var f in b)
                arr.Data[i++] = f;

            return arr;
        }

        public ref T this[int x, int y] => ref Data[
            x
            + y * B
            ];
    }
}
