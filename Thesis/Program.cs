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

            var experiment = database.NewExperiment();


            Globals.k = 70;

            Globals.Save(experiment);


            DataProvider.Random = MersenneTwister.Instance;

            //Data data = DataProvider.getSingleLineData();
            //Data data = DataProvider.getDoubleLinesData();
            Data data = DataProvider.getMultipleLinesData();
            //Data data = DataProvider.getCircleData();


            ClusterWizard cluster = new KMeansClusterWizard(data);


            Model model = Model.fromCluster(cluster);
            var constraints = model.GetMathModel();

            constraints.Decide(data.X);

            var refinedConstraints = Refiner.removeRedundant(constraints);
            var refined2Constraints = Refiner.removeSimiliar(refinedConstraints);

            refined2Constraints.Save(experiment);

            Console.Out.WriteLine("\nRefined: ");
            Output.ToConsole(refined2Constraints.Constraints);


            var stats = new Statistics(data, model.Decide(data.X));

            Console.Out.WriteLine("\nBase Measures: ");
            Console.Out.WriteLine(stats.getMeasures());
            Console.Out.WriteLine("\nContraints: " + model.CountConstraints());

            var stats2 = new Statistics(data, refined2Constraints.Decide(data.X));

            Console.Out.WriteLine("\nRefined Measures: ");
            Console.Out.WriteLine(stats2.getMeasures());
            Console.Out.WriteLine("\nContraints k: " + refined2Constraints.CountConstraints());

            stats2.save(experiment);


            new Visualization()
                .addModelPlot(cluster, model, false)
                .addModelPlot(cluster, refinedConstraints, false)
                .addModelPlot(cluster, refined2Constraints, false)
                .Show();



            Console.ReadKey();
        }
    }
}
