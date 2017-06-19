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

            Globals.fromArgs(args);

            var database = new Database(Globals.dbName);
            var experiment = database.NewExperiment();

            Globals.dataset = "sphere";
            Globals.seed = 3;

            Globals.fromArgs(args);
            Globals.Save(experiment);

            Accord.Math.Random.Generator.Seed = Globals.seed;
            DataProvider.Random = MersenneTwister.Instance;

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

            //Data trainingData = DataProvider.getBenchmarkBalanced(originalModel);
            //Data ratioedTestData = DataProvider.getBenchmarkBalanced(originalModel);
            Data uniformTestData = DataProvider.getTestBenchmark(originalModel);



            ////Output.PointsToFile("Points/" + Globals.seed + Globals.dataset + Globals.n + "training.txt", trainingData.X);
            ////Output.PointsToFile("Points/" + Globals.seed + Globals.dataset + Globals.n + "test.txt", testData.X);

            Data trainingData = DataProvider.fromFile(originalModel, "Points/" + Globals.seed + Globals.dataset + Globals.n + "training.txt");
            Data ratioedTestData = DataProvider.fromFile(originalModel, "Points/" + Globals.seed + Globals.dataset + Globals.n + "test.txt");

            Process process = Process.GetCurrentProcess();

            process.Refresh();

            ClusterWizard cluster = new KMeansClusterWizard(trainingData);

            Model model = Model.fromCluster(cluster);
            var constraints = model.GetMathModel();

            var refinedConstraints = Refiner.removeRedundant(constraints);
            refinedConstraints = Refiner.mergeSimiliar(refinedConstraints);

            var refinedConstraints2 = Refiner.removeRedundant(constraints);


            var time = process.TotalProcessorTime.Milliseconds;

            experiment.Add("Time", time);
            experiment.Save();

            var clusterConstraints = Refiner.cluster(constraints);
            //clusterConstraints = Refiner.removeRedundant(clusterConstraints);
            var clusterConstraints2 = Refiner.removeRedundant(clusterConstraints);

            refinedConstraints.Save(experiment, "Refined_");

            //var stats = new Statistics(trainingData, model.Decide(trainingData.X));

            ////Console.Out.WriteLine("\nBase Measures: ");
            ////Console.Out.WriteLine(stats.getMeasures());
            ////Console.Out.WriteLine("\nContraints: " + model.CountConstraints());

            //stats.save(experiment, "Base_");


            var trainingStats = new Statistics(trainingData, refinedConstraints.Decide(trainingData.X));

            var uniformTestStats = new Statistics(uniformTestData, refinedConstraints.Decide(uniformTestData.X));
            var ratioedTestStats = new Statistics(ratioedTestData, refinedConstraints.Decide(ratioedTestData.X));

            //Console.Out.WriteLine("\nRefined Measures: ");
            //Console.Out.WriteLine(testStats.getMeasures());
            //Console.Out.WriteLine("\nContraints k: " + refinedConstraints.CountConstraints());

            trainingStats.save(experiment, "Training_");
            uniformTestStats.saveFull(experiment, "U_Test_");
            ratioedTestStats.saveFull(experiment, "R_Test_");

            double angle = -10000;

            if (Globals.dataset != "sphere")
            {
                try {
                    angle = Comparison.CalculateMeanAngle(originalModel, refinedConstraints);
                } catch (Exception e)
                {
                    angle = -500000;
                }
               
            }
            experiment.Add("Comparison_angle", angle);
            experiment.Save();

            //var testCluster = new SingleClusterWizard(testData);


            new Visualization()
                //.addClusters(cluster, "Clusters")
                .addModelPlot(cluster, model, false, "Base model")
                //.addModelPlot(cluster, refinedConstraints2, false, "Removed redundant")
                .addModelPlot(cluster, refinedConstraints, false, "Merged similiar")
                //.addModelPlot(testCluster, refinedConstraints, false, "Test comparison")
                //.addModelPlot(testCluster, refinedConstraints, true, "Test comparison")
                //.addModelPlot(cluster, originalModel, false, "original")
                //.addModelPlot(cluster, clusterConstraints, false, "Clustered redundant")
                .addModelPlot(cluster, clusterConstraints2, false, "Clustered final")
                //.addModelPlot(cluster, originalModel, false, "original")
                .Show();


            experiment.Dispose();
            database.Dispose();

            //Console.ReadKey();
        }
    }
}
