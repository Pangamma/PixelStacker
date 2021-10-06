using System;
using System.Drawing;
using System.Collections.Generic;
using SimplePaletteQuantizer.Helpers;

namespace SimplePaletteQuantizer.Quantizers.XiaolinWu
{
    /// <summary>
    /// Author:	Xiaolin Wu
    /// Dept. of Computer Science
    /// Univ. of Western Ontario
    /// London, Ontario N6A 5B7
    /// wu@csd.uwo.ca
    /// </summary>
    public class WuColorQuantizer : BaseColorQuantizer
    {
        #region | Constants |
        private const int MaxColor = 512;
        private const int Red = 2;
        private const int Green = 1;
        private const int Blue = 0;
        private const int SideSize = 33;
        private const int MaxSideIndex = 32;
        private const int MaxVolume = SideSize * SideSize * SideSize;

        #endregion

        #region | Fields |

        private int[] reds;
        private int[] greens;
        private int[] blues;
        private int[] sums;
        private int[] indices;

        private long[,,] weights;
        private long[,,] momentsRed;
        private long[,,] momentsGreen;
        private long[,,] momentsBlue;
        private float[,,] moments;

        private int[] tag;
        private int[] quantizedPixels;
        private int[] table;
        private int[] pixels;

        private int imageWidth;
        private int imageSize;
        private int pixelIndex;

        private WuColorCube[] cubes;

        #endregion

        #region | Helper methods |

        /// <summary>
        /// Converts the histogram to a series of moments.
        /// </summary>
        private void CalculateMoments()
        {
            long[] area = new long[SideSize];
            long[] areaRed = new long[SideSize];
            long[] areaGreen = new long[SideSize];
            long[] areaBlue = new long[SideSize];
            float[] area2 = new float[SideSize];

            for (int redIndex = 1; redIndex <= MaxSideIndex; ++redIndex)
            {
                for (int index = 0; index <= MaxSideIndex; ++index)
                {
                    area[index] = 0;
                    areaRed[index] = 0;
                    areaGreen[index] = 0;
                    areaBlue[index] = 0;
                    area2[index] = 0;
                }

                for (int greenIndex = 1; greenIndex <= MaxSideIndex; ++greenIndex)
                {
                    long line = 0;
                    long lineRed = 0;
                    long lineGreen = 0;
                    long lineBlue = 0;
                    float line2 = 0.0f;

                    for (int blueIndex = 1; blueIndex <= MaxSideIndex; ++blueIndex)
                    {
                        line += weights[redIndex, greenIndex, blueIndex];
                        lineRed += momentsRed[redIndex, greenIndex, blueIndex];
                        lineGreen += momentsGreen[redIndex, greenIndex, blueIndex];
                        lineBlue += momentsBlue[redIndex, greenIndex, blueIndex];
                        line2 += moments[redIndex, greenIndex, blueIndex];

                        area[blueIndex] += line;
                        areaRed[blueIndex] += lineRed;
                        areaGreen[blueIndex] += lineGreen;
                        areaBlue[blueIndex] += lineBlue;
                        area2[blueIndex] += line2;

                        weights[redIndex, greenIndex, blueIndex] = weights[redIndex - 1, greenIndex, blueIndex] + area[blueIndex];
                        momentsRed[redIndex, greenIndex, blueIndex] = momentsRed[redIndex - 1, greenIndex, blueIndex] + areaRed[blueIndex];
                        momentsGreen[redIndex, greenIndex, blueIndex] = momentsGreen[redIndex - 1, greenIndex, blueIndex] + areaGreen[blueIndex];
                        momentsBlue[redIndex, greenIndex, blueIndex] = momentsBlue[redIndex - 1, greenIndex, blueIndex] + areaBlue[blueIndex];
                        moments[redIndex, greenIndex, blueIndex] = moments[redIndex - 1, greenIndex, blueIndex] + area2[blueIndex];
                    }
                }
            }
        }

        /// <summary>
        /// Computes the volume of the cube in a specific moment.
        /// </summary>
        private static long Volume(WuColorCube cube, long[,,] moment)
        {
            return moment[cube.RedMaximum, cube.GreenMaximum, cube.BlueMaximum] -
                   moment[cube.RedMaximum, cube.GreenMaximum, cube.BlueMinimum] -
                   moment[cube.RedMaximum, cube.GreenMinimum, cube.BlueMaximum] +
                   moment[cube.RedMaximum, cube.GreenMinimum, cube.BlueMinimum] -
                   moment[cube.RedMinimum, cube.GreenMaximum, cube.BlueMaximum] +
                   moment[cube.RedMinimum, cube.GreenMaximum, cube.BlueMinimum] +
                   moment[cube.RedMinimum, cube.GreenMinimum, cube.BlueMaximum] -
                   moment[cube.RedMinimum, cube.GreenMinimum, cube.BlueMinimum];
        }

        /// <summary>
        /// Computes the volume of the cube in a specific moment. For the floating-point values.
        /// </summary>
        private static float VolumeFloat(WuColorCube cube, float[,,] moment)
        {
            return moment[cube.RedMaximum, cube.GreenMaximum, cube.BlueMaximum] -
                   moment[cube.RedMaximum, cube.GreenMaximum, cube.BlueMinimum] -
                   moment[cube.RedMaximum, cube.GreenMinimum, cube.BlueMaximum] +
                   moment[cube.RedMaximum, cube.GreenMinimum, cube.BlueMinimum] -
                   moment[cube.RedMinimum, cube.GreenMaximum, cube.BlueMaximum] +
                   moment[cube.RedMinimum, cube.GreenMaximum, cube.BlueMinimum] +
                   moment[cube.RedMinimum, cube.GreenMinimum, cube.BlueMaximum] -
                   moment[cube.RedMinimum, cube.GreenMinimum, cube.BlueMinimum];
        }

        /// <summary>
        /// Splits the cube in given position, and color direction.
        /// </summary>
        private static long Top(WuColorCube cube, int direction, int position, long[,,] moment)
        {
            switch (direction)
            {
                case Red:
                    return (moment[position, cube.GreenMaximum, cube.BlueMaximum] -
                            moment[position, cube.GreenMaximum, cube.BlueMinimum] -
                            moment[position, cube.GreenMinimum, cube.BlueMaximum] +
                            moment[position, cube.GreenMinimum, cube.BlueMinimum]);

                case Green:
                    return (moment[cube.RedMaximum, position, cube.BlueMaximum] -
                            moment[cube.RedMaximum, position, cube.BlueMinimum] -
                            moment[cube.RedMinimum, position, cube.BlueMaximum] +
                            moment[cube.RedMinimum, position, cube.BlueMinimum]);

                case Blue:
                    return (moment[cube.RedMaximum, cube.GreenMaximum, position] -
                            moment[cube.RedMaximum, cube.GreenMinimum, position] -
                            moment[cube.RedMinimum, cube.GreenMaximum, position] +
                            moment[cube.RedMinimum, cube.GreenMinimum, position]);

                default:
                    return 0;
            }
        }

        /// <summary>
        /// Splits the cube in a given color direction at its minimum.
        /// </summary>
        private static long Bottom(WuColorCube cube, int direction, long[,,] moment)
        {
            switch (direction)
            {
                case Red:
                    return (-moment[cube.RedMinimum, cube.GreenMaximum, cube.BlueMaximum] +
                             moment[cube.RedMinimum, cube.GreenMaximum, cube.BlueMinimum] +
                             moment[cube.RedMinimum, cube.GreenMinimum, cube.BlueMaximum] -
                             moment[cube.RedMinimum, cube.GreenMinimum, cube.BlueMinimum]);

                case Green:
                    return (-moment[cube.RedMaximum, cube.GreenMinimum, cube.BlueMaximum] +
                             moment[cube.RedMaximum, cube.GreenMinimum, cube.BlueMinimum] +
                             moment[cube.RedMinimum, cube.GreenMinimum, cube.BlueMaximum] -
                             moment[cube.RedMinimum, cube.GreenMinimum, cube.BlueMinimum]);

                case Blue:
                    return (-moment[cube.RedMaximum, cube.GreenMaximum, cube.BlueMinimum] +
                             moment[cube.RedMaximum, cube.GreenMinimum, cube.BlueMinimum] +
                             moment[cube.RedMinimum, cube.GreenMaximum, cube.BlueMinimum] -
                             moment[cube.RedMinimum, cube.GreenMinimum, cube.BlueMinimum]);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Calculates statistical variance for a given cube.
        /// </summary>
        private float CalculateVariance(WuColorCube cube)
        {
            float volumeRed = Volume(cube, momentsRed);
            float volumeGreen = Volume(cube, momentsGreen);
            float volumeBlue = Volume(cube, momentsBlue);
            float volumeMoment = VolumeFloat(cube, moments);
            float volumeWeight = Volume(cube, weights);

            float distance = volumeRed*volumeRed + volumeGreen*volumeGreen + volumeBlue*volumeBlue;

            return volumeMoment - (distance/volumeWeight);
        }

        /// <summary>
        ///	Finds the optimal (maximal) position for the cut.
        /// </summary>
        private float Maximize(WuColorCube cube, int direction, int first, int last, IList<int> cut, long wholeRed, long wholeGreen, long wholeBlue, long wholeWeight)
        {
            long bottomRed = Bottom(cube, direction, momentsRed);
            long bottomGreen = Bottom(cube, direction, momentsGreen);
            long bottomBlue = Bottom(cube, direction, momentsBlue);
            long bottomWeight = Bottom(cube, direction, weights);

            float result = 0.0f;
            cut[0] = -1;

            for (int position = first; position < last; ++position)
            {
                // determines the cube cut at a certain position
                long halfRed = bottomRed + Top(cube, direction, position, momentsRed);
                long halfGreen = bottomGreen + Top(cube, direction, position, momentsGreen);
                long halfBlue = bottomBlue + Top(cube, direction, position, momentsBlue);
                long halfWeight = bottomWeight + Top(cube, direction, position, weights);

                // the cube cannot be cut at bottom (this would lead to empty cube)
                if (halfWeight != 0)
                {
                    float halfDistance = halfRed*halfRed + halfGreen*halfGreen + halfBlue*halfBlue;
                    float temp = halfDistance/halfWeight;

                    halfRed = wholeRed - halfRed;
                    halfGreen = wholeGreen - halfGreen;
                    halfBlue = wholeBlue - halfBlue;
                    halfWeight = wholeWeight - halfWeight;

                    if (halfWeight != 0)
                    {
                        halfDistance = halfRed * halfRed + halfGreen * halfGreen + halfBlue * halfBlue;
                        temp += halfDistance / halfWeight;

                        if (temp > result)
                        {
                            result = temp;
                            cut[0] = position;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Cuts a cube with another one.
        /// </summary>
        private bool Cut(WuColorCube first, WuColorCube second)
        {
            int direction;

            int[] cutRed = { 0 };
            int[] cutGreen = { 0 };
            int[] cutBlue = { 0 };

            long wholeRed = Volume(first, momentsRed);
            long wholeGreen = Volume(first, momentsGreen);
            long wholeBlue = Volume(first, momentsBlue);
            long wholeWeight = Volume(first, weights);

            float maxRed = Maximize(first, Red, first.RedMinimum + 1, first.RedMaximum, cutRed, wholeRed, wholeGreen, wholeBlue, wholeWeight);
            float maxGreen = Maximize(first, Green, first.GreenMinimum + 1, first.GreenMaximum, cutGreen, wholeRed, wholeGreen, wholeBlue, wholeWeight);
            float maxBlue = Maximize(first, Blue, first.BlueMinimum + 1, first.BlueMaximum, cutBlue, wholeRed, wholeGreen, wholeBlue, wholeWeight);

            if ((maxRed >= maxGreen) && (maxRed >= maxBlue))
            {
                direction = Red;

                // cannot split empty cube
                if (cutRed[0] < 0) return false; 
            }
            else
            {
                if ((maxGreen >= maxRed) && (maxGreen >= maxBlue))
                {
                    direction = Green;
                }
                else
                {
                    direction = Blue;
                }
            }

            second.RedMaximum = first.RedMaximum;
            second.GreenMaximum = first.GreenMaximum;
            second.BlueMaximum = first.BlueMaximum;

            // cuts in a certain direction
            switch (direction)
            {
                case Red:
                    second.RedMinimum = first.RedMaximum = cutRed[0];
                    second.GreenMinimum = first.GreenMinimum;
                    second.BlueMinimum = first.BlueMinimum;
                    break;

                case Green:
                    second.GreenMinimum = first.GreenMaximum = cutGreen[0];
                    second.RedMinimum = first.RedMinimum;
                    second.BlueMinimum = first.BlueMinimum;
                    break;

                case Blue:
                    second.BlueMinimum = first.BlueMaximum = cutBlue[0];
                    second.RedMinimum = first.RedMinimum;
                    second.GreenMinimum = first.GreenMinimum;
                    break;
            }

            // determines the volumes after cut
            first.Volume = (first.RedMaximum - first.RedMinimum)*(first.GreenMaximum - first.GreenMinimum)*(first.BlueMaximum - first.BlueMinimum);
            second.Volume = (second.RedMaximum - second.RedMinimum)*(second.GreenMaximum - second.GreenMinimum)*(second.BlueMaximum - second.BlueMinimum);

            // the cut was successfull
            return true;
        }

        /// <summary>
        /// Marks all the tags with a given label.
        /// </summary>
        private static void Mark(WuColorCube cube, int label, IList<int> tag)
        {
            for (int redIndex = cube.RedMinimum + 1; redIndex <= cube.RedMaximum; ++redIndex)
            {
                for (int greenIndex = cube.GreenMinimum + 1; greenIndex <= cube.GreenMaximum; ++greenIndex)
                {
                    for (int blueIndex = cube.BlueMinimum + 1; blueIndex <= cube.BlueMaximum; ++blueIndex)
                    {
                        tag[(redIndex << 10) + (redIndex << 6) + redIndex + (greenIndex << 5) + greenIndex + blueIndex] = label;
                    }
                }
            }
        }

        #endregion

        #region << BaseColorQuantizer >>

        /// <summary>
        /// See <see cref="BaseColorQuantizer.OnPrepare"/> for more details.
        /// </summary>
        protected override void OnPrepare(ImageBuffer image)
        {
            // creates all the cubes
            cubes = new WuColorCube[MaxColor];

            // initializes all the cubes
            for (int cubeIndex = 0; cubeIndex < MaxColor; cubeIndex++)
            {
                cubes[cubeIndex] = new WuColorCube();
            }

            // resets the reference minimums
            cubes[0].RedMinimum = 0;
            cubes[0].GreenMinimum = 0;
            cubes[0].BlueMinimum = 0;

            // resets the reference maximums
            cubes[0].RedMaximum = MaxSideIndex;
            cubes[0].GreenMaximum = MaxSideIndex;
            cubes[0].BlueMaximum = MaxSideIndex;

            weights = new long[SideSize, SideSize, SideSize];
            momentsRed = new long[SideSize, SideSize, SideSize];
            momentsGreen = new long[SideSize, SideSize, SideSize];
            momentsBlue = new long[SideSize, SideSize, SideSize];
            moments = new float[SideSize, SideSize, SideSize];

            table = new int[256];

            for (int tableIndex = 0; tableIndex < 256; ++tableIndex)
            {
                table[tableIndex] = tableIndex * tableIndex;
            }

            pixelIndex = 0;
            imageWidth = image.Width;
            imageSize = image.Width * image.Height;

            quantizedPixels = new int[imageSize];
            pixels = new int[imageSize];
        }

        /// <summary>
        /// See <see cref="BaseColorQuantizer.AddColor"/> for more details.
        /// </summary>
        protected override void OnAddColor(Color color, int key, int x, int y)
        {
            int indexRed = (color.R >> 3) + 1;
            int indexGreen = (color.G >> 3) + 1;
            int indexBlue = (color.B >> 3) + 1;

            weights[indexRed, indexGreen, indexBlue]++;
            momentsRed[indexRed, indexGreen, indexBlue] += color.R;
            momentsGreen[indexRed, indexGreen, indexBlue] += color.G;
            momentsBlue[indexRed, indexGreen, indexBlue] += color.B;
            moments[indexRed, indexGreen, indexBlue] += table[color.R] + table[color.G] + table[color.B];

            quantizedPixels[pixelIndex] = (indexRed << 10) + (indexRed << 6) + indexRed + (indexGreen << 5) + indexGreen + indexBlue;
            pixels[pixelIndex] = color.ToArgb();
            pixelIndex++;
        }

        /// <summary>
        /// See <see cref="BaseColorQuantizer.OnGetPalette"/> for more details.
        /// </summary>
        protected override List<Color> OnGetPalette(int colorCount)
        {
            // preprocess the colors
            CalculateMoments();

            int next = 0;
            float[] volumeVariance = new float[MaxColor];

            // processes the cubes
            for (int cubeIndex = 1; cubeIndex < colorCount; ++cubeIndex)
            {
                // if cut is possible; make it
                if (Cut(cubes[next], cubes[cubeIndex]))
                {
                    volumeVariance[next] = cubes[next].Volume > 1 ? CalculateVariance(cubes[next]) : 0.0f;
                    volumeVariance[cubeIndex] = cubes[cubeIndex].Volume > 1 ? CalculateVariance(cubes[cubeIndex]) : 0.0f;
                }
                else // the cut was not possible, revert the index
                {
                    volumeVariance[next] = 0.0f;
                    cubeIndex--;
                }

                next = 0;
                float temp = volumeVariance[0];

                for (int index = 1; index <= cubeIndex; ++index)
                {
                    if (volumeVariance[index] > temp)
                    {
                        temp = volumeVariance[index];
                        next = index;
                    }
                }

                if (temp <= 0.0)
                {
                    colorCount = cubeIndex + 1;
                    break;
                }
            }

            int[] lookupRed = new int[MaxColor];
            int[] lookupGreen = new int[MaxColor];
            int[] lookupBlue = new int[MaxColor];

            tag = new int[MaxVolume];

            // precalculates lookup tables
            for (int k = 0; k < colorCount; ++k)
            {
                Mark(cubes[k], k, tag);

                long weight = Volume(cubes[k], weights);

                if (weight > 0)
                {
                    lookupRed[k] = (int)(Volume(cubes[k], momentsRed) / weight);
                    lookupGreen[k] = (int)(Volume(cubes[k], momentsGreen) / weight);
                    lookupBlue[k] = (int)(Volume(cubes[k], momentsBlue) / weight);
                }
                else
                {
                    lookupRed[k] = 0;
                    lookupGreen[k] = 0;
                    lookupBlue[k] = 0;
                }
            }

            // copies the per pixel tags 
            for (int index = 0; index < imageSize; ++index)
            {
                quantizedPixels[index] = tag[quantizedPixels[index]];
            }

            reds = new int[colorCount + 1];
            greens = new int[colorCount + 1];
            blues = new int[colorCount + 1];
            sums = new int[colorCount + 1];
            indices = new int[imageSize];

            // scans and adds colors
            for (int index = 0; index < imageSize; index++)
            {
                Color color = Color.FromArgb(pixels[index]);

                int match = quantizedPixels[index];
                int bestMatch = match;
                int bestDistance = 100000000;

                for (int lookup = 0; lookup < colorCount; lookup++)
                {
                    int foundRed = lookupRed[lookup];
                    int foundGreen = lookupGreen[lookup];
                    int foundBlue = lookupBlue[lookup];
                    int deltaRed = color.R - foundRed;
                    int deltaGreen = color.G - foundGreen;
                    int deltaBlue = color.B - foundBlue;

                    int distance = deltaRed * deltaRed + deltaGreen * deltaGreen + deltaBlue * deltaBlue;

                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestMatch = lookup;
                    }
                }

                reds[bestMatch] += color.R;
                greens[bestMatch] += color.G;
                blues[bestMatch] += color.B;
                sums[bestMatch]++;

                indices[index] = bestMatch;
            }

            List<Color> result = new List<Color>();

            // generates palette
            for (int paletteIndex = 0; paletteIndex < colorCount; paletteIndex++)
            {
                if (sums[paletteIndex] > 0)
                {
                    reds[paletteIndex] /= sums[paletteIndex];
                    greens[paletteIndex] /= sums[paletteIndex];
                    blues[paletteIndex] /= sums[paletteIndex];
                }

                Color color = Color.FromArgb(255, reds[paletteIndex], greens[paletteIndex], blues[paletteIndex]);
                result.Add(color);
            }

            pixelIndex = 0;
            return result;
        }

        /// <summary>
        /// See <see cref="BaseColorQuantizer.OnGetPaletteIndex"/> for more details.
        /// </summary>
        protected override void OnGetPaletteIndex(Color color, int key, int x, int y, out int paletteIndex)
        {
            paletteIndex = indices[x + y*imageWidth];
        }

        /// <summary>
        /// See <see cref="BaseColorQuantizer.OnFinish"/> for more details.
        /// </summary>
        protected override void OnFinish()
        {
            base.OnFinish();

            cubes = null;
            weights = null;
            momentsRed = null;
            momentsGreen = null;
            momentsBlue = null;
            moments = null;
            quantizedPixels = null;
            pixels = null;
        }

        #endregion

        #region << IColorQuantizer >>
        /// <summary>
        /// See <see cref="IColorQuantizer.Label"/> for more details.
        /// </summary>
        public override string Label => "Wu's color quantizer";

        /// <summary>
        /// See <see cref="IColorQuantizer.AllowParallel"/> for more details.
        /// </summary>
        public override bool AllowParallel
        {
            get { return false; }
        }

        #endregion
    }
}
