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

            Data uniformTestData = DataProvider.getTestBenchmark(originalModel);

            //Data trainingData = DataProvider.fromFile(originalModel, "Points/" + Globals.seed + Globals.dataset + Globals.n + "training.txt");
            Data trainingData = DataProvider.getBenchmark(originalModel);
            //Data ratioedTestData = DataProvider.fromFile(originalModel, "Points/" + Globals.seed + Globals.dataset + Globals.n + "test.txt");

            Process process = Process.GetCurrentProcess();

            process.Refresh();

            ClusterWizard cluster = new KMeansClusterWizard(trainingData);

            Model model = Model.fromCluster(cluster);
            var constraints = model.GetMathModel();

            //no refining
            //var refinedConstraints = constraints;

            //refine method I
            var refinedConstraints = Refiner.removeRedundant(constraints);
            refinedConstraints = Refiner.mergeSimiliar(refinedConstraints);

            //refine method II
            //var clusterConstraints = Refiner.cluster(constraints);
            //var refinedConstraints = Refiner.removeRedundant(clusterConstraints);

            var time = process.TotalProcessorTime.Milliseconds;

            experiment.Add("Time", time);
            experiment.Save();


            refinedConstraints.Save(experiment, "Refined_");

            var trainingStats = new Statistics(trainingData, refinedConstraints.Decide(trainingData.X));

            var uniformTestStats = new Statistics(uniformTestData, refinedConstraints.Decide(uniformTestData.X));
            //var ratioedTestStats = new Statistics(ratioedTestData, refinedConstraints.Decide(ratioedTestData.X));

            trainingStats.save(experiment, "Training_");
            uniformTestStats.saveFull(experiment, "U_Test_");
            //ratioedTestStats.saveFull(experiment, "R_Test_");

            experiment.Save();

            new Visualization()
                .addClusters(cluster,"Clusters")
                .addModelPlot(cluster, refinedConstraints, "Model")
                .Show();
 
            experiment.Dispose();
            database.Dispose();

        }
    }
}
