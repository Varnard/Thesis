using ExperimentDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis
{
    public class MathModel
    {
        public List<double[]> Constraints { get; set; }


        public MathModel(List<double[]> constraints)
        {
            this.Constraints=constraints;
        }

        public MathModel()
        {
            this.Constraints = new List<double[]>();
        }
       

        public bool[] Decide(double[][] inputs)
        {
            bool[] output = new bool[inputs.Length];

            for (int i = 0; i < inputs.Length; i++)
            {
                List<bool> results = new List<bool>(); ;
                foreach (var constraint in Constraints)
                {
                    double value=constraint[0];
                    for (int j = 0; j < inputs[i].Length; j++)
                    {
                        value += inputs[i][j] * constraint[j + 1];
                    }                    
                    results.Add(value>0);
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

        public bool Decide(double[] input)
        {
            double[][] array = { input };

            return Decide(array)[0];
        }

        public int CountConstraints()
        {
            return Constraints.Count;
        }

        public void Save(Experiment experiment)
        {
            experiment.Add("constraint_count", CountConstraints());
            experiment.Add("constraints", Output.ToString(Constraints));
            experiment.Save();
        }
    }
}
