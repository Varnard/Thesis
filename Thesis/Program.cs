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
using Modeling.Utils;


//gurobi
namespace Thesis
{
    class Program
    {
        [MTAThread]
        static void Main(string[] args)
        {
            DataProvider.Random = MersenneTwister.Instance;

            //Data data = DataProvider.getSingleLineData();
            //Data data = DataProvider.getDoubleLinesData();
            Data data = DataProvider.getMultipleLinesData();
            var inputs = data.X;
            var outputs = data.Y;

            //ClusterWizard cluster = new SingleClusterWizard(data);
            ClusterWizard cluster = new KMeansClusterWizard(30, data);

            Model model = new Model(cluster);

            var constraints = model.GetMathModel();
            
            Output.toConsole(constraints);
            Output.toFile(constraints);
            new Visualization(cluster, model).showResults();

            var refPoints = DataProvider.getRefinementPoints();
            var refinedModel = new Model(Refiner.removeRedundant(model.SVM, DataProvider.getRefinementPoints()));
            new Visualization(new SingleClusterWizard(refPoints), refinedModel).showResults();

            Console.Out.WriteLine("\nRefined: ");
            Output.toConsole(refinedModel.GetMathModel());

            
            Console.Out.WriteLine("\nMeasures: ");
            Console.Out.WriteLine(new Statistics(data, model.Decide(inputs)).getMeasures());

            new Visualization(cluster, refinedModel).showResults();


            Console.ReadKey();
        }
    }
}
