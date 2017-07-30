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
            //Globals.seed = 3;

            Globals.k = 4;
            Globals.ratio = 0.5;
            Globals.p = 1000;

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
            //refinedConstraints = Refiner.mergeSimiliar(refinedConstraints);

            //var refinedConstraints2 = Refiner.removeRedundant(constraints);


            var time = process.TotalProcessorTime.Milliseconds;

            experiment.Add("Time", time);
            experiment.Save();

            //var clusterConstraints = Refiner.cluster(constraints);
            //clusterConstraints = Refiner.removeRedundant(clusterConstraints);
            //var clusterConstraints2 = Refiner.removeRedundant(clusterConstraints);
            //var refinedConstraints = Refiner.removeRedundant(clusterConstraints);

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
            var p = cluster.getPositives();
            int pc = p.Length;
            var n = cluster.getNegatives();
            var n1 = n[0];
            int n1c = n1.Length;
            var n2 = n[1];
            int n2c = n2.Length;
            var n3 = n[2];
            int n3c = n3.Length;
            var n4 = n[3];
            int n4c = n4.Length;

            double[][] pr1x = new double[pc + n1c][];
            int[] pr1y = new int[pc + n1c];
            double[][] pr2x = new double[pc + n2c][];
            int[] pr2y = new int[pc + n2c];
            double[][] pr3x = new double[pc + n3c][];
            int[] pr3y = new int[pc + n3c];
            double[][] pr4x = new double[pc + n4c][];
            int[] pr4y = new int[pc + n4c];

            for (int i = 0; i < pc; i++)
            {
                pr1x[i] = p[i];
                pr1y[i] = 1;
                pr2x[i] = p[i];
                pr2y[i] = 1;
                pr3x[i] = p[i];
                pr3y[i] = 1;
                pr4x[i] = p[i];
                pr4y[i] = 1;
            }
            for (int i = 0; i < n1c; i++)
            {
                pr1x[pc+i] = n1[i];
                pr1y[pc+i] = -1;
            }
            for (int i = 0; i < n2c; i++)
            {
                pr2x[pc + i] = n2[i];
                pr2y[pc + i] = -1;
            }
            for (int i = 0; i < n3c; i++)
            {
                pr3x[pc + i] = n3[i];
                pr3y[pc + i] = -1;
            }
            for (int i = 0; i < n4c; i++)
            {
                pr4x[pc + i] = n4[i];
                pr4y[pc + i] = -1;
            }

            var pr1 = new SingleClusterWizard(new Data(pr1x, pr1y));
            var pr1m = Model.fromCluster(pr1);

            var pr2 = new SingleClusterWizard(new Data(pr2x, pr2y));
            var pr2m = Model.fromCluster(pr2);

            var pr3 = new SingleClusterWizard(new Data(pr3x, pr3y));
            var pr3m = Model.fromCluster(pr3);

            var pr4 = new SingleClusterWizard(new Data(pr4x, pr4y));
            var pr4m = Model.fromCluster(pr4);

            new Visualization()
              
            //    //.addModelPlot(cluster, originalModel, false, "original")
            //    //.addClusters(cluster, "Clusters")
                //.addModelPlot(pr1, pr1m, false, "1st constraint")
                //.addModelPlot(pr2, pr2m, false, "2nd constraint")
                //  .addClusters(new SingleClusterWizard(trainingData), "Unclustered Data")
                //.addModelPlot(pr3, pr3m, false, "3rd constraint")
                //.addModelPlot(pr4, pr4m, false, "4th constraint")
                .addModelPlot(cluster, model, false, "Base model")
            //    //.addModelPlot(cluster, refinedConstraints2, false, "Removed redundant")
            //    //.addModelPlot(cluster, refinedConstraints, false, "Merged similiar")
            //    //.addModelPlot(testCluster, refinedConstraints, false, "Test comparison")
            //    //.addModelPlot(testCluster, refinedConstraints, true, "Test comparison")
            //    //.addModelPlot(cluster, originalModel, false, "original")
            //    .addModelPlot(cluster, clusterConstraints, false, "Clustered redundant")
            //    .addModelPlot(cluster, clusterConstraints2, false, "Clustered final")
            //    //.addModelPlot(cluster, originalModel, false, "original")
                .Show();
            Output.ToFile(model.GetMathModel().Constraints);

            experiment.Dispose();
            database.Dispose();

            //Console.ReadKey();
        }
    }
}
