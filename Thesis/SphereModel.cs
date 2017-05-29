using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Thesis
{
    class SphereModel : MathModel
    {
        public SphereModel()
        {
        }

        public SphereModel(double[] constraints) : base(constraints)
        {
        }

        public SphereModel(List<double[]> constraints) : base(constraints)
        {
        }

        public override bool[] Decide(double[][] inputs)
        {
            bool[] output = new bool[inputs.Length];

            for (int i = 0; i < inputs.Length; i++)
            {
                List<bool> results = new List<bool>(); ;
                foreach (var constraint in Constraints)
                {
                    double value = Pow(constraint[0],2);
                    for (int j = 0; j < inputs[i].Length; j++)
                    {
                        value -= Pow(inputs[i][j] * constraint[j + 1],2);
                    }
                    results.Add(value > 0);
                }
                bool result = true;

                foreach (bool partialResult in results)
                {
                    if (partialResult == false) result = false;
                }
                output[i] = result;
            }

            return output;
        }

    }
}
