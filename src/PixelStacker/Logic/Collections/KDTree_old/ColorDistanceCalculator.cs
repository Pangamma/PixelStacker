using Accord.Math.Distances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Collections
{
    public class ColorDistanceCalculator : IMetric<double[]>
    {
        public double Distance(double[] x, double[] y)
        {
            int dR = (int) (x[0] - y[0]);
            int dG = (int) (x[1] - y[1]);
            int dB = (int) (x[2] - y[2]);
            int dHue = (int) GetDegreeDistance(x[3], y[3]);
            int diff = (
                (dR * dR)
                + (dG * dG)
                + (dB * dB)
                + (int) (Math.Sqrt(dHue * dHue * dHue))
                ); 

            return diff;
        }

        public static double GetDegreeDistance(double alpha, double beta)
        {
            var phi = Math.Abs(beta - alpha) % 360;       // This is either the distance or 360 - distance
            var distance = phi > 180 ? 360 - phi : phi;
            return distance;
        }

    }
}
