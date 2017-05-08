using ExperimentDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis
{
    public static class Globals
    {
        static public int seed { get; set; } = 1;
        static public int k { get; set; } = 20;

        static public int n { get; set; } = 2;
        static public double d { get; set; } = 50;
        static public double maxVal { get; set; } = 100;
        static public double minVal { get; set; } = -100;

        static public double tolerance { get; set; } = 0.01;
        static public double complexity { get; set; } = 0.01;

        static public double distance { get; set; } = 0.2;
        static public double angle { get; set; } = 10.0;

        public static void Save(Experiment experiment)
        {
            experiment.Add("seed", Globals.seed);
            experiment.Add("K", Globals.k);
            experiment.Add("N", Globals.n);
            experiment.Add("D", Globals.d);
            experiment.Add("Tolerance", Globals.tolerance);
            experiment.Add("Complexity", Globals.complexity);
            experiment.Add("Distance", Globals.distance);
            experiment.Add("Angle", Globals.angle);
            experiment.Save();
        }
    }
}
