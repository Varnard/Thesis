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
            DataProvider.Random = MersenneTwister.Instance;

            var database = new Database("test.db");
            var experiment = database.NewExperiment();

            experiment.Add("seed", Globals.seed);
            experiment.Add("K", Globals.k);
            experiment.Save();

            //Data data = DataProvider.getSingleLineData();
            //Data data = DataProvider.getDoubleLinesData();
            Data data = DataProvider.getMultipleLinesData();
            var inputs = data.X;
            var outputs = data.Y;

            //ClusterWizard cluster = new SingleClusterWizard(data);
            ClusterWizard cluster = new KMeansClusterWizard(Globals.k, data);

            Model model = new Model(cluster);

            var constraints = model.GetMathModel();
            
            Output.ToConsole(constraints);
            Output.ToFile(constraints);
            

            var refPoints = DataProvider.getRefinementPoints();
            var refinedModel = new Model(Refiner.removeRedundant(model.SVM, DataProvider.getRefinementPoints()));

            refinedModel.SaveModel(experiment);

            Console.Out.WriteLine("\nRefined: ");
            Output.ToConsole(refinedModel.GetMathModel());

            var stats = new Statistics(data, model.Decide(inputs));


            Console.Out.WriteLine("\nMeasures: ");
            Console.Out.WriteLine(stats.getMeasures());

            stats.saveMeasures(experiment);


            new Visualization(cluster, model)             
                .addModelPlot(cluster, model, false)
                .addModelPlot(cluster, refinedModel, false)
                .Show();


            ////new Visualization(new SingleClusterWizard(refPoints), refinedModel).showResults();
            //new Visualization(cluster, refinedModel).showResults();



            Console.ReadKey();
        }
    }
}
