using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis
{
    public static class DataProvider
    {
        public static Random Random { get; set; }

        private static Data refPoints;

        public static Data getBenchmark(MathModel benchmark)
        {
            int p = Globals.p;

            double[][] X = getPoints(p);

            int[] Y = new int[p];

            for (int i = 0; i < p; i++)
            {
                if (benchmark.Decide(X[i])) Y[i] = 1;
            }

            return new Data(X, Y);
        }


        public static Data getTestBenchmark(MathModel benchmark)
        {
            int p = 10000;

            double[][] X = getPoints(p);

            int[] Y = new int[p];

            for (int i = 0; i < p; i++)
            {
                if (benchmark.Decide(X[i])) Y[i] = 1;
            }

            return new Data(X, Y);
        }

        public static Data getBenchmarkBalanced(MathModel benchmark, double ratio = 1)
        {
            int p = Globals.p;

            double[][] X = new double[p][];

            int[] Y = new int[p];

            int posCount = 0;
            int negCount = 0;

            int nc = (int)(p / (double)(1 + ratio));

            int pc = (int)((p / (double)(1 + ratio)) * (double)ratio);
            int i = 0;

            if (pc + nc < p) pc += 1;

            while (posCount < pc || negCount < nc)
            {
                var point = new double[Globals.n];

                for (int j = 0; j < Globals.n; j++)
                {
                    point[j] = Math.Round(Random.NextDouble() * (Globals.maxVal - Globals.minVal) + Globals.minVal, 5);
                }

                if (benchmark.Decide(point))
                {
                    if (posCount < pc)
                    {
                        X[i] = point;
                        Y[i] = 1;
                        i++;
                        posCount++;
                    }
                }
                else
                {
                    if (negCount < nc)
                    {
                        X[i] = point;
                        Y[i] = 0;
                        i++;
                        negCount++;
                    }
                }
            }

            return new Data(X, Y);
        }

        public static Data fromFile(MathModel benchmark, string filename)
        {
            int p = Globals.p;

            double[][] points = Input.readPoints(filename);

            double[][] X = new double[p][];

            int[] Y = new int[p];

            for (int i = 0; i < p; i++)
            {
                X[i] = points[i];
                if (benchmark.Decide(points[i])) Y[i] = 1;
            }

            return new Data(X, Y);
        }


        public static Data getRefinementPoints()
        {
            if (refPoints == null)
            {
                int p = 1000;

                double[][] X = getPoints(p);


                int[] Y = new int[p];
                refPoints = new Data(X, Y);
            }

            return refPoints;
        }

        private static double[][] getPoints(int n)
        {
            double[][] X = new double[n][];

            for (int i = 0; i < n; i++)
            {
                X[i] = new double[Globals.n];

                for (int j = 0; j < Globals.n; j++)
                {
                    X[i][j] = Math.Round(Random.NextDouble() * (Globals.maxVal - Globals.minVal) + Globals.minVal, 5);
                }
            }

            return X;
        }

    }
}
