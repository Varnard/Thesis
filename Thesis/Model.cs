using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Controls;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;
using ExperimentDatabase;

namespace Thesis
{
    public class Model
    {
        public List<SupportVectorMachine<Linear>> SVM { get; }

        public static Model fromSVM(List<SupportVectorMachine<Linear>> svm)
        {
            return new Model(svm);
        }

        public static Model fromCluster(ClusterWizard cluster, double tolerance, double complexity)
        {
            return new Model(cluster, tolerance, complexity);
        }

        public static Model fromCluster(ClusterWizard cluster)
        {
            return new Model(cluster, Globals.tolerance, Globals.complexity);
        }


        private Model(List<SupportVectorMachine<Linear>> svm)
        {
            this.SVM = svm;
        }

        private Model(ClusterWizard cluster, double tolerance, double complexity)
        {
            SVM = new List<SupportVectorMachine<Linear>>();

            var posList = cluster.getPositives();
            var negList = cluster.getNegatives();

            foreach (double[][] negSamples in negList)
            {
                var teacher = new SequentialMinimalOptimization<Linear>()
                {
                    UseComplexityHeuristic = false,
                    Strategy = SelectionStrategy.WorstPair
                                          
                };

                teacher.Complexity = complexity;
                teacher.Tolerance = tolerance;

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

        public bool Decide(double[] input)
        {
            double[][] array = { input };

            return Decide(array)[0];
        }

        public MathModel GetMathModel() 
        {
            List<double[]> constraints = new List<double[]>();            

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
                
                constraints.Add(variables);
            }
           
            return new MathModel(constraints);
        }

        public int CountConstraints()
        {
            return SVM.Count;
        }

        public void Save(Experiment experiment)
        {
            experiment.Add("constraint_count", CountConstraints());
            experiment.Add("constraints", Output.ToString(GetMathModel().Constraints));
            experiment.Save();
        }
    }
}
