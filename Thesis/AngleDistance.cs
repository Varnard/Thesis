using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.MachineLearning;
using Accord.Math.Distances;

namespace Thesis
{
    class AngleDistance : IDistance
    {
        public double Distance(double[] x, double[] y)
        {
            return ConstraintCalc.calculateAngle(x,y);
        }
    }
}
