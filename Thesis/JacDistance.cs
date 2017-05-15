using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math.Distances;

namespace Thesis
{
    class JacDistance : IDistance
    {
        public double Distance(double[] x, double[] y)
        {
            return ConstraintCalc.calculateDistance(x, y);
        }
    }
}
