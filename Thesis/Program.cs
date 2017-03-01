using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Controls;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Math;
using Accord.Statistics.Kernels;

namespace Thesis
{
    class Program
    {
        [MTAThread]
        static void Main(string[] args)
        {
            //Data data = DataProvider.getSingleLineData();
            //Data data = DataProvider.getDoubleLinesData();
            Data data = DataProvider.getMultipleLinesData();
            var inputs = data.X;
            var outputs = data.Y;

            //ClusterWizard cluster = new SingleClusterWizard(data);
            ClusterWizard cluster = new KMeansClusterWizard(8, data);

            Model model = new Model(cluster);

            // Classify the samples using the model
            bool[] decisions = model.Decide(inputs);


            int[] answers = new int[decisions.Length]; 

            for (int i=0;i<decisions.Length;i++) {
                if (decisions[i]) answers[i] = 1;
                else answers[i] = -1;
            }

            // Plot the results
            ScatterplotBox.Show("Expected results", inputs, outputs);
            ScatterplotBox.Show("GaussianSVM results", inputs, answers);

            model.GetMathModel();

            Console.ReadKey();
        }
    }
}
