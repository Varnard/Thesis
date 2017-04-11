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
        static public double tolerance { get; set; } = 0.01;
        static public double complexity { get; set; } = 0.005;

        public static void Save(Experiment experiment)
        {
            experiment.Add("seed", Globals.seed);
            experiment.Add("K", Globals.k);
            experiment.Add("Tolerance", Globals.tolerance);
            experiment.Add("Complexity", Globals.complexity);
            experiment.Save();
        }
    }
}
