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
using ExperimentDatabase;


//gurobi
namespace Thesis
{
    class Program
    {
        [MTAThread]
        static void Main(string[] args)
        {
            var database = new Database("test.db");

            Globals.k = 60;

            DataProvider.Random = MersenneTwister.Instance;


            var experiment = database.NewExperiment();

            experiment.Add("seed", Globals.seed);
            experiment.Add("K", Globals.k);
            experiment.Add("Tolerance", Globals.tolerance);
            experiment.Add("Complexity", Globals.complexity);
            experiment.Save();

            //Data data = DataProvider.getSingleLineData();
            //Data data = DataProvider.getDoubleLinesData();
            Data data = DataProvider.getMultipleLinesData();
            //Data data = DataProvider.getCircleData();

            var inputs = data.X;
            var outputs = data.Y;

            //ClusterWizard cluster = new SingleClusterWizard(data);
            ClusterWizard cluster = new KMeansClusterWizard(Globals.k, data);

            Model model = new Model(cluster, Globals.tolerance, Globals.complexity);

            var constraints = model.GetMathModel();

            //Output.ToConsole(constraints);
            //Output.ToFile(constraints);


            var refPoints = DataProvider.getRefinementPoints();
            var refinedModel = new Model(Refiner.removeRedundant(model.SVM, DataProvider.getRefinementPoints()));

            refinedModel.SaveModel(experiment);

            Console.Out.WriteLine("\nRefined: ");
            Output.ToConsole(refinedModel.GetMathModel());

            var stats = new Statistics(data, model.Decide(inputs));


            Console.Out.WriteLine("\nMeasures: ");
            Console.Out.WriteLine(stats.getMeasures());

            stats.saveMeasures(experiment);

            new Visualization()
                .addModelPlot(cluster, model)
                .addModelPlot(cluster, model, false)
                .Show();

            Console.ReadKey();


        }


    }
}
