﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis
{
    public static class Output
    {

        public static void ToFile(List<double[]> constraints)
        {
            List<String> output = new List<string>();
            int featureNumber = constraints.First().Length;

            output.Add("Variables");
            for (int i = 1; i < featureNumber; i++)
            {
                output.Add("x" + i + " in ["+Globals.minVal+","+Globals.maxVal+"];");
            }
            output.Add("Minimize");
            output.Add("x2");

            output.Add("Constraints");

            foreach (var variables in constraints)
            {
                string constraint = Math.Round(variables[0]).ToString();

                for (int i = 1; i < featureNumber; i++)
                {
                    constraint += (" + " + Math.Round(variables[i], 2) + "*x" + i);
                }
                constraint += ">=0;";
                output.Add(constraint.Replace(",", "."));
            }
            output.Add("end");
            System.IO.File.WriteAllLines("model.txt", output);

        }

        public static void ToConsole(List<double[]> constraints)
        {
            if (constraints.Count == 0) Console.Out.WriteLine("empty");
            else
            {
                int featureNumber = constraints.First().Length;

                foreach (var variables in constraints)
                {
                    for (int i = 0; i < featureNumber - 1; i++)
                    {
                        Console.Out.Write(Math.Round(variables[i], 2) + " x" + i + " + ");
                    }
                    Console.Out.WriteLine(Math.Round(variables.Last(), 2) + " x" + (featureNumber - 1));

                }
            }
        }

        public static string ToString(List<double[]> constraints)
        {
            if (constraints.Count == 0) return "empty";
            else
            {
                int featureNumber = constraints.First().Length;
                var sb = new StringBuilder();

                foreach (var variables in constraints)
                {
                    for (int i = 0; i < featureNumber - 1; i++)
                    {
                        sb.Append(Math.Round(variables[i], 2) + " x" + i + " + ");
                    }
                    sb.AppendLine(Math.Round(variables.Last(), 2) + " x" + (featureNumber - 1));

                }
                return sb.ToString();
            }
        }

        public static void PointsToFile(String filename, double[][] points)
        {

            System.IO.File.WriteAllLines(filename, points.Select(p => string.Join(" ", p.Select(c => c.ToString("r")).ToArray())).ToArray());
        }

    }
}
