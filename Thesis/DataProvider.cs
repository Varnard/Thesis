using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis
{
    public static class DataProvider
    {

    public static Data getSingleLineData() {

            double[][] X = new double[1000][];
            Random r = new Random();

            for (int i = 0; i < 1000; i++)
            {
                X[i] = new double[2] { r.NextDouble()*100, r.NextDouble() * 100};
            }

            int[] Y = new int[1000];

            for (int i = 0; i < 1000; i++)
            {
                if (2 * X[i][0] - X[i][1] -10 > 0) Y[i] = 1; // y=2x -10
            }

            return new Data(X, Y);
    }

        public static Data getDoubleLinesData()
        {

            double[][] X = new double[1000][];
            Random r = new Random();

            for (int i = 0; i < 1000; i++)
            {
                X[i] = new double[2] { r.NextDouble() * 100, r.NextDouble() * 100 };
            }

            int[] Y = new int[1000];

            for (int i = 0; i < 1000; i++)
            {
                if (X[i][0] > X[i][1] - 30 && X[i][0] < X[i][1]+30) Y[i] = 1;
            }

            return new Data(X, Y);
        }

        public static Data getMultipleLinesData()
        {

            double[][] X = new double[1000][];
            Random r = new Random();

            for (int i = 0; i < 1000; i++)
            {
                X[i] = new double[2] { r.NextDouble() * 100, r.NextDouble() * 100 };
               // X[i] = new double[3] { r.NextDouble() * 100, r.NextDouble() * 100, r.NextDouble() * 100, };
            }

            int[] Y = new int[1000];

            for (int i = 0; i < 1000; i++)
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

    }
}
