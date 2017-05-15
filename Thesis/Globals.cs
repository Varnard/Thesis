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
        static public int k { get; set; } = 60;

        static public int p { get; set; } = 1000;

        static public int n { get; set; } = 2;
        static public double d { get; set; } = 2.7;
        static public double maxVal { get; set; }
        static public double minVal { get; set; }

        static public double tolerance { get; set; } = 0.01;
        static public double complexity { get; set; } = 0.1;

        static public double distance { get; set; } = 0.1;
        static public double angle { get; set; } = 25.0;

        static public string dataset { get; set; } = "cube";

        public static void setMinMax(double min, double max) 
        {
            minVal = min;
            maxVal = max;        
        }

        //Thesis.exe seed dataset k p n d tolerance complexity distance angle
        public static void fromArgs(string[] args)
        {
            var count = args.Length;

            if (count > 0) seed = int.Parse(args[0]);
            if (count > 1) dataset = args[1];
            if (count > 2) k = int.Parse(args[2]);
            if (count > 3) p = int.Parse(args[3]);
            if (count > 4) n = int.Parse(args[4]);
            if (count > 5) d = double.Parse(args[5]);
            if (count > 6) tolerance = double.Parse(args[6]);
            if (count > 7) complexity = double.Parse(args[7]);
            if (count > 8) distance = double.Parse(args[8]);
            if (count > 9) angle = double.Parse(args[9]);
        }


        public static void Save(Experiment experiment)
        {
            experiment.Add("seed", Globals.seed);
            experiment.Add("dataset", Globals.dataset);
            experiment.Add("K", Globals.k);
            experiment.Add("P", Globals.p);
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
