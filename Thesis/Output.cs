using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis
{
    public static class Output
    {

        public static void toFile(List<double[]> constraints)
        {
            List<String> output = new List<string>();
            int featureNumber = constraints.First().Length;

            foreach (var variables in constraints)
            {
                String constraint = "";
                for (int i = 0; i < featureNumber; i++)
                {
                    constraint += (Math.Round(variables[i], 2) + " x" + i + " + ");
                }
                constraint += (Math.Round(variables.Last(), 2) + " x" + featureNumber);

                output.Add(constraint);
            }
            System.IO.File.WriteAllLines(@"D:\Varn\Documents\Visual Studio 2015\Projects\Thesis\Thesis\model.txt", output);

        }

        public static void toConsole(List<double[]> constraints)
        {
            int featureNumber = constraints.First().Length;

            foreach (var variables in constraints)
            {
                String constraint = "";
                for (int i = 0; i < featureNumber; i++)
                {
                    Console.Out.Write(Math.Round(variables[i], 2) + " x" + i + " + ");
                }
                Console.Out.WriteLine(Math.Round(variables.Last(), 2) + " x" + featureNumber);

            }
        }
    }
}
