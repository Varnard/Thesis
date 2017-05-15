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

        public static Data getCircleData()
        {
            int n = 1000;

            double[][] X = new double[n][];

            for (int i = 0; i < n; i++)
            {
                X[i] = new double[2] { Random.NextDouble() * 100, Random.NextDouble() * 100 };
            }

            int[] Y = new int[n];

            for (int i = 0; i < n; i++)
            {
                if (Math.Pow(X[i][0]-40,2) + Math.Pow(X[i][1]-40, 2) < Math.Pow(30, 2)) Y[i] = 1;
            }

            return new Data(X, Y);
        }
                     

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

        public static Data getBenchmarkBalanced(MathModel benchmark)
        {
            int p = Globals.p;

            double[][] X = new double[p][];

            int[] Y = new int[p];

            int posCount = 0;
            int negCount = 0;

            int c = p / 2;
            int i = 0;
            while (posCount<c||negCount<c)
            {
                var point = new double[Globals.n];

                for (int j = 0; j < Globals.n; j++)
                {                    
                    point[j] = Random.NextDouble() * (Globals.maxVal - Globals.minVal) + Globals.minVal;
                }

                if (benchmark.Decide(point))
                {
                    if (posCount < c)
                    {
                        X[i] = point;
                        Y[i] = 1;
                        i++;
                        posCount++;
                    }
                }
                else
                {
                    if (negCount < c)
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
                    X[i][j] = Random.NextDouble() * (Globals.maxVal-Globals.minVal) + Globals.minVal;
                }
            }

            return X;
        }

    }
}
