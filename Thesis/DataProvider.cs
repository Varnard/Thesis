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

        public static Data getSingleLineData() {

            int n = 1000;

            double[][] X = new double[n][];          

            for (int i = 0; i < n; i++)
            {
                X[i] = new double[2] { Random.NextDouble() * 100, Random.NextDouble() * 100 };
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
                X[i] = new double[2] { Random.NextDouble() * 100, Random.NextDouble() * 100 };
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
                X[i] = new double[2] { Random.NextDouble() * 100, Random.NextDouble() * 100 };
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

        public static Data getSingleLine4DData()
        {

            int n = 1000;

            double[][] X = new double[n][];

            for (int i = 0; i < n; i++)
            {
                X[i] = new double[4] { Random.NextDouble() * 100, Random.NextDouble() * 100, Random.NextDouble() * 100, Random.NextDouble() * 100 };
            }

            int[] Y = new int[n];

            for (int i = 0; i < n; i++)
            {
                if (2 * X[i][0] - X[i][1] - 0.6 * X[i][2] + 1.4 * X[i][3] - 10 > 0) Y[i] = 1;
            }

            return new Data(X, Y);
        }

        public static Data getHyperCube()
        {
            int p = 1000;

            double[][] X = getPoints(p);

            int[] Y = new int[p];

            for (int i = 0; i < p; i++)
            {
                int satisfied=1;

                for (int j = 0; j < Globals.n; j++)
                {
                    if (X[i][j] > Globals.d || X[i][j] < -Globals.d) satisfied = 0;
                }
                Y[i] = satisfied;
            }

            return new Data(X, Y);
        }

        public static Data getSimplex()
        {
            int p = 1000;

            double[][] X = getPoints(p);

            int[] Y = new int[p];

            for (int i = 0; i < p; i++)
            {
                int satisfied = 1;
                double sum=0;

                for (int j = 0; j < Globals.n-1; j++)
                {
                    if ((X[i][j]*(1/Math.Tan(Math.PI/12)))- (X[i][j+1] * (Math.Tan(Math.PI / 12))) < 0) satisfied = 0;
                    if ((X[i][j+1]*(1/Math.Tan(Math.PI/12)))- (X[i][j] * (Math.Tan(Math.PI / 12))) < 0) satisfied = 0;
                    sum += X[i][j];
                }
                if ((X[i][Globals.n-1] * (1 / Math.Tan(Math.PI / 12))) - (X[i][0] * (Math.Tan(Math.PI / 12))) < 0) satisfied = 0;
                if ((X[i][0] * (1 / Math.Tan(Math.PI / 12))) - (X[i][Globals.n - 1] * (Math.Tan(Math.PI / 12))) < 0) satisfied = 0;
                sum += X[i][Globals.n-1];

                if (sum > Globals.d) satisfied = 0;

                Y[i] = satisfied;
            
            }

            return new Data(X, Y);
        }

        public static Data getHyperSphere()
        {
            int p = 1000;

            double[][] X = getPoints(p);

            int[] Y = new int[p];

            for (int i = 0; i < p; i++)
            {
                int satisfied = 1;
                double sum=0;

                for (int j = 0; j < Globals.n; j++)
                {
                    sum += Math.Pow(X[i][j], 2);
                }

                if (sum > Math.Pow(Globals.d, 2)) satisfied = 0;

                Y[i] = satisfied;
            }

            return new Data(X, Y);
        }

        public static Data getRefinementPoints()
        {
            int p = 10000;

            double[][] X = getPoints(p);


            int[] Y = new int[p];

            return new Data(X, Y);
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
