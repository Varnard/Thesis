using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Controls;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;

namespace Thesis
{
    class Model
    {
        private List<SupportVectorMachine<Linear>> SVM;


        public Model(ClusterWizard cluster)
        {
            SVM = new List<SupportVectorMachine<Linear>>();

            var posList = cluster.getPositives();
            var negList = cluster.getNegatives();

            foreach (double[][] negSamples in negList)
            {

                var teacher = new SequentialMinimalOptimization<Linear>()
                {
                    UseComplexityHeuristic = false,
                    Complexity = 1

                };

                List<double[]> inputs = new List<double[]>();
                List<bool> outputs = new List<bool>();

                foreach (double[] data in posList)
                {
                    inputs.Add(data);
                    outputs.Add(true);
                }

                foreach (double[] data in negSamples)
                {
                    inputs.Add(data);
                    outputs.Add(false);
                }

                var decisions = outputs.ToArray();

                int[] answers = new int[decisions.Length];

                for (int i = 0; i < decisions.Length; i++)
                {
                    if (decisions[i]) answers[i] = 1;
                    else answers[i] = -1;
                }

                ScatterplotBox.Show("SVM learns", inputs.ToArray(), answers);

                try
                {
                    SVM.Add(teacher.Learn(inputs.ToArray(), outputs.ToArray()));
                } catch (Exception e)
                {
                    Console.Out.WriteLine(e.Message);

                }
            }
        }

        public bool[] Decide(double[][] inputs)
        {
            bool[] output = new bool[inputs.Length];

            for (int i = 0; i < inputs.Length; i++)
            {
                List<bool> results = new List<bool>(); ;
                foreach (var classifer in SVM)
                {
                    results.Add(classifer.Decide(inputs[i]));
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

        public List<double[]> GetMathModel() 
        {
            List<double[]> models = new List<double[]>();            

            foreach (var svm in SVM)
            {
                var supportVectors = svm.SupportVectors;

                var weights = svm.Weights;

                int featureNumber = supportVectors.First().Length;

                double[] variables = new double[featureNumber + 1];
                variables[0] = svm.Threshold;
                
                for (int i = 0; i < featureNumber; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < supportVectors.Length; j++)
                        sum += weights[j] * supportVectors[j][i];
                    variables[i + 1] = sum;
                }
                
                models.Add(variables);
            }
           
            return models;
        }
    }
}
