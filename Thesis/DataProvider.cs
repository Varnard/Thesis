using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis
{
    public static class DataProvider
    {
        private static Random random = new Random(576);

    public static Data getSingleLineData() {

            int n = 1000;

            double[][] X = new double[n][];          

            for (int i = 0; i < n; i++)
            {
                X[i] = new double[2] { random.NextDouble() * 100, random.NextDouble() * 100 };
            }

            int[] Y = new int[n];

            for (int i = 0; i < n; i++)
            {
                if (2 * X[i][0] - X[i][1] -10 > 0) Y[i] = 1; // y=2x -10
            }

            return new Data(X, Y);
    }



        public static Data getDoubleLinesData()
        {

            int n = 1000;

            double[][] X = new double[n][];

            for (int i = 0; i < n; i++)
            {
                X[i] = new double[2] { random.NextDouble() * 100, random.NextDouble() * 100 };
            }

            int[] Y = new int[n];

            for (int i = 0; i < n; i++)
            {
                if (X[i][0] > X[i][1] - 30 && X[i][0] < X[i][1]+30) Y[i] = 1;
            }

            return new Data(X, Y);
        }

        public static Data getMultipleLinesData()
        {

            int n = 1000;

            double[][] X = new double[n][];

            for (int i = 0; i < n; i++)
            {
                X[i] = new double[2] { random.NextDouble() * 100, random.NextDouble() * 100 };
            }

            int[] Y = new int[n];

            for (int i = 0; i < n; i++)
            {
                if (
                X[i][0] > X[i][1] - 30 && 
                X[i][0] > 25 &&
                X[i][1] < 70 &&
                X[i][0] < X[i][1] + 30 &&
                X[i][0] > -X[i][1]+60
                ) Y[i] = 1;
            }

            return new Data(X, Y);
        }

        public static Data getRefinementPoints()
        {
            int n = 10000;

            double[][] X = new double[n][];

            for (int i = 0; i < n; i++)
            {
                X[i] = new double[2] { random.NextDouble() * 100, random.NextDouble() * 100 };
            }

            int[] Y = new int[n];

            return new Data(X, Y);
        }

    }
}
