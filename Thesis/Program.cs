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


//gurobi
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
            ClusterWizard cluster = new KMeansClusterWizard(20, data);

            Model model = new Model(cluster);

            var constraints = model.GetMathModel();
            Output.toConsole(constraints);
            Output.toFile(constraints);

            new Visualization(cluster, model).showResults();

            Console.ReadKey();
        }
    }
}
