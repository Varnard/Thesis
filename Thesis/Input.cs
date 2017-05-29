using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis
{
    class Input
    {
        public static double[][] readPoints(string filename)
        {
            string[] lines = System.IO.File.ReadAllLines(filename);

            double[][] array = lines.Where(line => !String.IsNullOrWhiteSpace(line)) // Use this to filter blank lines.
                .Select(line => line.Split((string[])null, StringSplitOptions.RemoveEmptyEntries))
                .Select(line => line.Select(element => double.Parse(element)))
                .Select(line => line.ToArray())
                .ToArray();

            return array;
        }

    }
}
