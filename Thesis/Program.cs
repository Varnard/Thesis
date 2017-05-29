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
using System.Diagnostics;

namespace Thesis
{
    class Program
    {
        [MTAThread]
        static void Main(string[] args)
        {

            //var database = new Database("test4v1.db");

            //var experiment = database.NewExperiment();


            Globals.fromArgs(args);
            //Globals.Save(experiment);

            Accord.Math.Random.Generator.Seed = Globals.seed;
            DataProvider.Random = MersenneTwister.Instance;



           // Globals.dataset = "sphere";

            MathModel originalModel = null;

            switch (Globals.dataset)
            {
                case "cube":
                    {
                        originalModel = Benchmark.getHyperCube();
                        break;
                    }

                case "sphere":
                    {
                        originalModel = Benchmark.getHyperSphere();
                        break;
                    }

                case "simplex":
                    {
                        originalModel = Benchmark.getSimplex();
                        break;
                    }

            }

            //var originalModel = Benchmark.getMultipleLines2D();


            Data trainingData = DataProvider.getBenchmarkBalanced(originalModel);
            Data testData = DataProvider.getBenchmarkBalanced(originalModel);

            Output.PointsToFile("Points/" + Globals.seed + Globals.dataset + Globals.n + "training.txt", trainingData.X);
            Output.PointsToFile("Points/" + Globals.seed + Globals.dataset + Globals.n + "test.txt", testData.X);

            //Data trainingData2 = DataProvider.fromFile(originalModel, "Points/" + Globals.seed + Globals.dataset + Globals.n + "training.txt");
            //Data testData2 = DataProvider.fromFile(originalModel, "Points/" + Globals.seed + Globals.dataset + Globals.n + "test.txt");


            //ClusterWizard cluster = new KMeansClusterWizard(trainingData);

            //Model model = Model.fromCluster(cluster);
            //var constraints = model.GetMathModel();

            //var refinedConstraints = Refiner.removeRedundant(constraints);
            //refinedConstraints = Refiner.mergeSimiliar(refinedConstraints);

            ////var clusterConstraints = Refiner.cluster(constraints);
            ////clusterConstraints = Refiner.removeRedundant(clusterConstraints);

            ////var refinedConstraints = Refiner.mergeSimiliar(constraints);
            ////var refined2Constraints = Refiner.removeRedundant(refinedConstraints);

            //refinedConstraints.Save(experiment, "Refined_");

            ////Console.Out.WriteLine("\nBase: ");
            ////Output.ToConsole(constraints.Constraints);

            ////Console.Out.WriteLine("\nRefined: ");
            ////Output.ToConsole(refinedConstraints.Constraints);


            ////var stats = new Statistics(trainingData, model.Decide(trainingData.X));

            //////Console.Out.WriteLine("\nBase Measures: ");
            //////Console.Out.WriteLine(stats.getMeasures());
            //////Console.Out.WriteLine("\nContraints: " + model.CountConstraints());

            ////stats.save(experiment, "Base_");


            //var trainingStats = new Statistics(trainingData, refinedConstraints.Decide(trainingData.X));

            //var testStats = new Statistics(testData, refinedConstraints.Decide(testData.X));

            ////Console.Out.WriteLine("\nRefined Measures: ");
            ////Console.Out.WriteLine(stats2.getMeasures());
            ////Console.Out.WriteLine("\nContraints k: " + refinedConstraints.CountConstraints());

            ////var stats3 = new Statistics(data, clusterConstraints.Decide(data.X));

            ////Console.Out.WriteLine("\nClustered Measures: ");
            ////Console.Out.WriteLine(stats3.getMeasures());
            ////Console.Out.WriteLine("\nContraints: " + clusterConstraints.CountConstraints());

            //trainingStats.save(experiment, "Training_");
            //testStats.save(experiment, "Test_");

            ////Console.WriteLine("\nRefiner Classic\n");
            ////experiment.Add("Comparison_angle",Comparison.CalculateMeanAngle(originalModel, refinedConstraints));
            ////experiment.Save();

            ////Console.WriteLine("\nRefiner Cluster\n");
            ////Comparison.CalculateMeanAngle(originalModel, clusterConstraints);

            ////new Visualization()
            ////    .addModelPlot(cluster, model)
            ////    .addModelPlot(cluster, refinedConstraints, false)
            ////    //.addModelPlot(cluster, clusterConstraints, false)
            ////    .addModelPlot(cluster, originalModel, false)
            ////    .Show();


            //experiment.Dispose();
            //database.Dispose();

            //Console.ReadKey();
        }
    }
}
