using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using PixelStacker.Logic.Engine.Quantizer.Helpers;

namespace PixelStacker.Logic.Engine.Quantizer.Quantizers.NeuQuant
{
    /// <summary>
    /// The NeuQuant Neural-Net image quantization algorithm (© Anthony Dekker 1994) 
    /// is a replacement for the common Median Cut algorithm. It is described in the 
    /// article Kohonen neural networks for optimal colour quantization  in Volume 5, 
    /// pp 351-367 of the journal Network: Computation in Neural Systems, Institute of 
    /// Physics Publishing, 1994 (PDF version available).
    /// </summary>
    public class NeuralColorQuantizer : BaseColorQuantizer
    {
        #region | Constants |

        private const byte DefaultQuality = 10; // 10

        private const int AlphaBiasShift = 10;
        private const int AlphaRadiusBias = 1 << AlphaRadiusBetaShift;
        private const int AlphaRadiusBetaShift = AlphaBiasShift + RadiusBiasShift;
        private const int Beta = InitialBias >> BetaShift;
        private const int BetaShift = 10;
        private const int BetaGamma = InitialBias << GammaShift - BetaShift;
        private const int DefaultRadius = NetworkSize >> 3;
        private const int DefaultRadiusBiasShift = 6;
        private const int DefaultRadiusBias = 1 << DefaultRadiusBiasShift;
        private const int GammaShift = 10;
        private const int InitialAlpha = 1 << AlphaBiasShift;
        private const int InitialBias = 1 << InitialBiasShift;
        private const int InitialBiasShift = 16;
        private const int InitialRadius = DefaultRadius * DefaultRadiusBias;
        private const int MaximalNetworkPosition = NetworkSize - 1;
        private const int NetworkSize = 256;
        private const int NetworkBiasShift = 4;
        private const int RadiusBiasShift = 8;
        private const int RadiusDecrease = 30;
        private const int RadiusBias = 1 << RadiusBiasShift;

        #endregion

        #region | Fields |

        private readonly FastRandom random;
        private readonly ConcurrentDictionary<int, bool> uniqueColors;

        private int[] bias;
        private int[] frequency;
        private int[] networkIndexLookup;
        private int[] radiusPower;

        private byte quality;
        private int delta;
        private int radius;
        private int alpha;
        private int initialRadius;
        private int alphaDecrease;
        private int[][] network;

        #endregion

        #region | Properties |

        /// <summary>
        /// Gets or sets the quality.
        /// </summary>
        /// <value>The quality.</value>
        public byte Quality
        {
            get { return quality; }
            set
            {
                quality = value;
                alphaDecrease = 30 + (quality - 1);
            }
        }

        #endregion

        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="NeuralColorQuantizer"/> class.
        /// </summary>
        public NeuralColorQuantizer()
        {
            Quality = DefaultQuality;

            random = new FastRandom(0);
            uniqueColors = new ConcurrentDictionary<int, bool>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NeuralColorQuantizer"/> class.
        /// </summary>
        /// <param name="quality">The quality.</param>
        public NeuralColorQuantizer(byte quality) : this()
        {
            Quality = quality;
        }

        #endregion

        #region | Helper methods |

        private int FindClosestNeuron(int red, int green, int blue)
        {
            // initializes the search variables
            int bestIndex = -1;
            int bestDistance = ~(1 << 31);
            int bestBiasIndex = bestIndex;
            int bestBiasDistance = bestDistance;

            for (int index = 0; index < NetworkSize; index++)
            {
                int[] neuron = network[index];

                // computes differences between neuron (color), and provided color
                int deltaRed = neuron[2] - red;
                int deltaGreen = neuron[1] - green;
                int deltaBlue = neuron[0] - blue;

                // makes values absolute
                if (deltaRed < 0) deltaRed = -deltaRed;
                if (deltaGreen < 0) deltaGreen = -deltaGreen;
                if (deltaBlue < 0) deltaBlue = -deltaBlue;

                // sums the distance
                int distance = deltaRed + deltaGreen + deltaBlue;

                // if best so far, store it
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestIndex = index;
                }

                // calculates biase distance
                int biasDistance = distance - (bias[index] >> InitialBiasShift - NetworkBiasShift);

                // if best so far, store it
                if (biasDistance < bestBiasDistance)
                {
                    bestBiasDistance = biasDistance;
                    bestBiasIndex = index;
                }

                int betaFrequency = frequency[index] >> BetaShift;
                frequency[index] -= betaFrequency;
                bias[index] += betaFrequency << GammaShift;
            }

            frequency[bestIndex] += Beta;
            bias[bestIndex] -= BetaGamma;
            return bestBiasIndex;
        }

        /// <summary>
        /// Forces the strength of bias of the neuron towards certain color.
        /// </summary>
        private void LearnNeuron(int alpha, int red, int green, int blue, int networkIndex)
        {
            /* alter hit neuron */
            int[] neuron = network[networkIndex];
            neuron[2] -= alpha * (neuron[2] - red) / InitialAlpha;
            neuron[1] -= alpha * (neuron[1] - green) / InitialAlpha;
            neuron[0] -= alpha * (neuron[0] - blue) / InitialAlpha;
        }

        /// <summary>
        /// Spread the bias to neuron neighbors.
        /// </summary>
        protected void LearnNeuronNeighbors(int red, int green, int blue, int networkIndex, int radius)
        {
            // detects lower border
            int lowBound = networkIndex - radius;
            if (lowBound < -1) lowBound = -1;

            // detects high border
            int highBound = networkIndex + radius;
            if (highBound > NetworkSize) highBound = NetworkSize;

            // initializes the variables
            int increaseIndex = networkIndex + 1;
            int decreaseIndex = networkIndex - 1;
            int radiusStep = 1;

            // learns neurons in a given radius
            while (increaseIndex < highBound || decreaseIndex > lowBound)
            {
                int[] neuron;
                int alphaMultiplicator = radiusPower[radiusStep++];

                if (increaseIndex < highBound)
                {
                    neuron = network[increaseIndex++];
                    neuron[0] -= alphaMultiplicator * (neuron[0] - blue) / AlphaRadiusBias;
                    neuron[1] -= alphaMultiplicator * (neuron[1] - green) / AlphaRadiusBias;
                    neuron[2] -= alphaMultiplicator * (neuron[2] - red) / AlphaRadiusBias;
                }

                if (decreaseIndex > lowBound)
                {
                    neuron = network[decreaseIndex--];
                    neuron[0] -= alphaMultiplicator * (neuron[0] - blue) / AlphaRadiusBias;
                    neuron[1] -= alphaMultiplicator * (neuron[1] - green) / AlphaRadiusBias;
                    neuron[2] -= alphaMultiplicator * (neuron[2] - red) / AlphaRadiusBias;
                }
            }
        }

        #endregion

        #region | Network methods |

        private void UnbiasNetwork()
        {
            for (int index = 0; index < NetworkSize; index++)
            {
                network[index][0] >>= NetworkBiasShift;
                network[index][1] >>= NetworkBiasShift;
                network[index][2] >>= NetworkBiasShift;
                network[index][3] = index;
            }
        }

        private void SortNetwork()
        {
            int startIndex = 0;
            int previousValue = 0;

            for (int index = 0; index < NetworkSize; index++)
            {
                int[] neuron = network[index];

                int bestIndex = index;
                int bestValue = neuron[1];

                for (int subIndex = index + 1; subIndex < NetworkSize; subIndex++)
                {
                    int[] subNeuron = network[subIndex];

                    if (subNeuron[1] < bestValue)
                    {
                        bestIndex = subIndex;
                        bestValue = subNeuron[1];
                    }
                }

                // swaps the neuron components
                if (index != bestIndex)
                {
                    int[] neuronB = network[bestIndex];

                    for (int subIndex = 0; subIndex < 4; subIndex++)
                    {
                        int swap = neuronB[subIndex];
                        neuronB[subIndex] = neuron[subIndex];
                        neuron[subIndex] = swap;
                    }
                }

                // if the value is still not optimal
                if (bestValue != previousValue)
                {
                    networkIndexLookup[previousValue] = startIndex + index >> 1;

                    for (int subIndex = previousValue + 1; subIndex < bestValue; subIndex++)
                    {
                        networkIndexLookup[subIndex] = index;
                    }

                    previousValue = bestValue;
                    startIndex = index;
                }
            }

            networkIndexLookup[previousValue] = startIndex + MaximalNetworkPosition >> 1;

            // resets certain portion of the index lookup
            for (int index = previousValue + 1; index < 256; index++)
            {
                networkIndexLookup[index] = MaximalNetworkPosition;
            }
        }

        private void LearnSampleColor(Color color)
        {
            int red = color.R << NetworkBiasShift;
            int green = color.G << NetworkBiasShift;
            int blue = color.B << NetworkBiasShift;

            int neuronIndex = FindClosestNeuron(red, green, blue);

            LearnNeuron(alpha, red, green, blue, neuronIndex);

            if (radius != 0)
            {
                LearnNeuronNeighbors(red, green, blue, neuronIndex, radius);
            }

            if (delta == 0) delta = 1;

            alpha -= alpha / alphaDecrease;
            initialRadius -= initialRadius / RadiusDecrease;
            radius = initialRadius >> DefaultRadiusBiasShift;

            if (radius <= 1) radius = 0;
            int radiusSquared = radius * radius;

            for (int index = 0; index < radius; index++)
            {
                int indexSquared = index * index;
                radiusPower[index] = alpha * ((radiusSquared - indexSquared) * RadiusBias / radiusSquared);
            }
        }

        #endregion

        #region << BaseColorQuantizer >>

        /// <summary>
        /// See <see cref="BaseColorQuantizer.OnPrepare"/> for more details.
        /// </summary>
        protected override void OnPrepare(ImageBuffer image)
        {
            base.OnPrepare(image);

            OnFinish();

            network = new int[NetworkSize][];
            uniqueColors.Clear();

            // initializes all the neurons in the network
            for (int neuronIndex = 0; neuronIndex < NetworkSize; neuronIndex++)
            {
                int[] neuron = new int[4];

                // calculates the base value for all the components
                int baseValue = (neuronIndex << NetworkBiasShift + 8) / NetworkSize;
                neuron[0] = baseValue;
                neuron[1] = baseValue;
                neuron[2] = baseValue;

                // determines other per neuron values
                bias[neuronIndex] = 0;
                network[neuronIndex] = neuron;
                frequency[neuronIndex] = InitialBias / NetworkSize;
            }

            // initializes the some variables
            alpha = InitialAlpha;
            initialRadius = InitialRadius;

            // determines the radius
            int potentialRadius = InitialRadius >> DefaultRadiusBiasShift;
            radius = potentialRadius <= 1 ? 0 : potentialRadius;
            int radiusSquared = radius * radius;

            // precalculates the powers for all the radiuses
            for (int index = 0; index < radius; index++)
            {
                int indexSquared = index * index;
                radiusPower[index] = alpha * ((radiusSquared - indexSquared) * RadiusBias / radiusSquared);
            }
        }

        /// <summary>
        /// See <see cref="BaseColorQuantizer.OnAddColor"/> for more details.
        /// </summary>
        protected override void OnAddColor(Color color, int key, int x, int y)
        {
            base.OnAddColor(color, key, x, y);

            if (random.Next(DefaultQuality) == 0)
            {
                LearnSampleColor(color);
            }
        }

        /// <summary>
        /// See <see cref="BaseColorQuantizer.OnGetPalette"/> for more details.
        /// </summary>
        protected override List<Color> OnGetPalette(int colorCount)
        {
            // post-process the neural network
            UnbiasNetwork();
            SortNetwork();

            // initialize the index cache
            int[] indices = new int[NetworkSize];

            // re-index the network cache
            for (int index = 0; index < NetworkSize; index++)
            {
                indices[network[index][3]] = index;
            }

            // initializes the empty palette
            List<Color> result = new List<Color>();

            // grabs the best palette, from the neurons
            for (int index = 0; index < NetworkSize; index++)
            {
                int neuronIndex = indices[index];

                int red = network[neuronIndex][2];
                int green = network[neuronIndex][1];
                int blue = network[neuronIndex][0];

                Color color = Color.FromArgb(255, red, green, blue);
                result.Add(color);
            }

            return result;
        }

        /// <summary>
        /// See <see cref="IColorQuantizer.GetPaletteIndex"/> for more details.
        /// </summary>
        protected override void OnGetPaletteIndex(Color color, int key, int x, int y, out int paletteIndex)
        {
            int bestDistance = 1000;
            int increaseIndex = networkIndexLookup[color.G];
            int decreaseIndex = increaseIndex - 1;
            paletteIndex = -1;

            while (increaseIndex < NetworkSize || decreaseIndex >= 0)
            {
                if (increaseIndex < NetworkSize)
                {
                    int[] neuron = network[increaseIndex];

                    // add green delta
                    int deltaG = neuron[1] - color.G;
                    if (deltaG < 0) deltaG = -deltaG;
                    int distance = deltaG;

                    if (distance >= bestDistance)
                    {
                        increaseIndex = NetworkSize;
                    }
                    else
                    {
                        increaseIndex++;

                        // add blue delta
                        int deltaB = neuron[0] - color.B;
                        if (deltaB < 0) deltaB = -deltaB;
                        distance += deltaB;

                        if (distance < bestDistance)
                        {
                            // add red delta
                            int deltaR = neuron[2] - color.R;
                            if (deltaR < 0) deltaR = -deltaR;
                            distance += deltaR;

                            if (distance < bestDistance)
                            {
                                bestDistance = distance;
                                paletteIndex = neuron[3];
                            }
                        }
                    }
                }

                if (decreaseIndex >= 0)
                {
                    int[] neuron = network[decreaseIndex];

                    // add green delta
                    int deltaG = color.G - neuron[1];
                    if (deltaG < 0) deltaG = -deltaG;
                    int distance = deltaG;

                    if (distance >= bestDistance)
                    {
                        decreaseIndex = -1;
                    }
                    else
                    {
                        decreaseIndex--;

                        // add blue delta
                        int deltaBlue = neuron[0] - color.B;
                        if (deltaBlue < 0) deltaBlue = -deltaBlue;
                        distance += deltaBlue;

                        if (distance < bestDistance)
                        {
                            // add red delta
                            int deltaRed = neuron[2] - color.R;
                            if (deltaRed < 0) deltaRed = -deltaRed;
                            distance += deltaRed;

                            if (distance < bestDistance)
                            {
                                bestDistance = distance;
                                paletteIndex = neuron[3];
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// See <see cref="BaseColorQuantizer.OnFinish"/> for more details.
        /// </summary>
        protected override void OnFinish()
        {
            base.OnFinish();

            bias = new int[NetworkSize];
            frequency = new int[NetworkSize];
            networkIndexLookup = new int[256];
            radiusPower = new int[DefaultRadius];
            network = null;
        }

        #endregion

        #region << IColorQuantizer >>
        /// <summary>
        /// See <see cref="IColorQuantizer.Label"/> for more details.
        /// </summary>
        public override string Label => "Neural quantizer";

        /// <summary>
        /// See <see cref="IColorQuantizer.AllowParallel"/> for more details.
        /// </summary>
        public override bool AllowParallel
        {
            get { return true; }
        }

        #endregion
    }
}
