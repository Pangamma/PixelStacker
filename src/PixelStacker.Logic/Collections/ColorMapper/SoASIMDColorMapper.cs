using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using PixelStacker.Logic.Model;
using SkiaSharp;
using PixelStacker.Extensions;
using System.Runtime.InteropServices;

namespace PixelStacker.Logic.Collections.ColorMapper
{
    public sealed class SoASIMDColorMapper : IColorMapper
    {
        public string AlgorithmTitle => "SoASIMD";
        private List<MaterialCombination> combos;
        private float[] R;
        private float[] G;
        private float[] B;

        public SoASIMDColorMapper()
        { }


        public double AccuracyRating => 51.389;

        public double SpeedRating => 100.5;

        private bool isSeeded = false;

        private int ClosestMatch(Vector3 colour)
        {
            ref float rRef = ref MemoryMarshal.GetArrayDataReference(R);
            ref float gRef = ref MemoryMarshal.GetArrayDataReference(G);
            ref float bRef = ref MemoryMarshal.GetArrayDataReference(B);
            var inputR = new Vector<float>(colour.X);
            var inputG = new Vector<float>(colour.Y);
            var inputB = new Vector<float>(colour.Z);


            int closestIndex = 0;
            float smallestDiff = float.NaN;

            int i = 0;

            Span<float> dist = stackalloc float[Vector<float>.Count];

            for (; i < (R.Length & ~(Vector<float>.Count - 1)); i += Vector<float>.Count)
            {
                var r = Unsafe.As<float, Vector<float>>(ref Unsafe.Add(ref rRef, i));
                var g = Unsafe.As<float, Vector<float>>(ref Unsafe.Add(ref gRef, i));
                var b = Unsafe.As<float, Vector<float>>(ref Unsafe.Add(ref bRef, i));

                var diffR = inputR - r;
                diffR *= diffR;

                var diffG = inputG - g;
                diffG *= diffG;

                var diffB = inputB - b;
                diffB *= diffB;

                Vector<float> distVec = diffR + diffG + diffB;



                Unsafe.As<float, Vector<float>>(ref dist[0]) = distVec;

                for (int j = 0; j < dist.Length; j++)
                {
                    var d = dist[j];
                    if (!(d >= smallestDiff))
                    {
                        smallestDiff = d;
                        closestIndex = i + j;
                    }
                }
            }

            for (; i < R.Length; i++)
            {
                Vector3 paletteColor = new(R[i], G[i], B[i]);

                var d = Vector3.DistanceSquared(colour, paletteColor);
                if (!(d >= smallestDiff))
                {
                    smallestDiff = d;
                    closestIndex = i;
                }
            }

            return closestIndex;
        }

        public MaterialCombination FindBestMatch(SKColor c)
        {
            return combos[ClosestMatch(new Vector3(c.Red, c.Green, c.Blue) / new Vector3(255))];
        }

       

        public void SetSeedData(List<MaterialCombination> combos, MaterialPalette mats, bool isSideView)
        {
            this.combos = combos;

            var l = combos.Count;
            (R, G, B) = (new float[l], new float[l], new float[l]);

            if (isSideView)
            {
                for (int i = 0; i < l; i++)
                {
                    var combo = combos[i];
                    var c = combo.SideImage.GetAverageColor();
                    var vecColor = new Vector3(c.Red, c.Green, c.Blue) / new Vector3(255);
                    (R[i], G[i], B[i]) = (vecColor.X, vecColor.Y, vecColor.Z);
                }
            }
            else
            {
                for (int i = 0; i < l; i++)
                {
                    var combo = combos[i];
                    var c = combo.TopImage.GetAverageColor();
                    var vecColor = new Vector3(c.Red, c.Green, c.Blue) / new Vector3(255);
                    (R[i], G[i], B[i]) = (vecColor.X, vecColor.Y, vecColor.Z);
                }
            }

            isSeeded = true;
        }
        
        public List<MaterialCombination> FindBestMatches(SKColor c, int maxMatches)
        {
            throw new NotImplementedException();
        }

        public bool IsSeeded()
        {
            return isSeeded;
        }
    }
}
