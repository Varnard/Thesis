using Accord.MachineLearning.VectorMachines;
using Accord.Statistics.Kernels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;
using Accord.MachineLearning;

namespace Thesis
{
    public static class Refiner
    {
        public static MathModel removeRedundant(MathModel model)
        {
            var constraints = new List<double[]>(model.Constraints);

            var points = DataProvider.getRefinementPoints();

            var newConstraints = new List<double[]>();

            foreach (var constraint in constraints)
            {
                bool needed = false;

                var rest = new List<double[]>(constraints);
                rest.Remove(constraint);
                var reducedMathModel = new MathModel(rest);

                var removed = new List<double[]>();
                removed.Add(constraint);
                var removedModel = new MathModel(removed);
                foreach (var point in points.X)
                {
                    if (!removedModel.Decide(point) && reducedMathModel.Decide(point))
                    {
                        needed = true;
                    }
                }
                if (needed) newConstraints.Add(constraint);
            }

            return new MathModel(newConstraints);
        }

        public static MathModel removeSimiliar(MathModel model, double distance)
        {
            return removeSimiliar(model, distance, Globals.angle);
        }

        public static MathModel removeSimiliar(MathModel model)
        {
            return removeSimiliar(model, Globals.distance, Globals.angle);
        }

        public static MathModel removeSimiliar(MathModel model, double distance, double angle)
        {
            var constraints = new List<double[]>(model.Constraints);

            var newConstraints = new List<double[]>(model.Constraints);

            int i = 0;
            while (i < constraints.Count - 1)
            {
                for (int j = constraints.Count - 1; j > i; j--)
                {
                    var a = Math.Abs(ConstraintCalc.calculateAngle(constraints[i], constraints[j]));
                    if (a < angle || a > 180 - angle)
                    {
                        var d = ConstraintCalc.calculateDistance(constraints[i], constraints[j]);
                        if (d < distance)
                        {
                            constraints.RemoveAt(j);
                            newConstraints.RemoveAt(j);
                        }
                    }
                }

                i++;
            }


            return new MathModel(newConstraints);
        }

        public static MathModel mergeSimiliar(MathModel model, double distance)
        {
            return mergeSimiliar(model, distance, Globals.angle);
        }

        public static MathModel mergeSimiliar(MathModel model)
        {
            return mergeSimiliar(model, Globals.distance, Globals.angle);
        }

        public static MathModel mergeSimiliar(MathModel model, double distance, double angle)
        {
            
            var constraints = new List<double[]>(model.Constraints);

            for (int n = 0; n < 5; n++)
            {
                int i = 0;
                while (i < constraints.Count - 1)
                {
                    for (int j = constraints.Count - 1; j > i; j--)
                    {
                        var a = Math.Abs(ConstraintCalc.calculateAngle(constraints[i], constraints[j]));
                        if (a < angle || a > 180 - angle)
                        {
                            var d = ConstraintCalc.calculateDistance(constraints[i], constraints[j]);
                            if (d < distance)
                            {
                                var merged = ConstraintCalc.meanConstraint(constraints[i], constraints[j]);
                                constraints[j] = merged;
                                constraints[i] = merged;
                            }
                        }
                    }

                    i++;
                }

                constraints = new List<double[]>(removeSimiliar(new MathModel(constraints), 1, 0.01).Constraints);
            }

            return new MathModel(constraints);
        }

        public static MathModel cluster(MathModel model)
        {
            int k = removeRedundant(model).Constraints.Count;

            List<double[]> result = new List<double[]>(); 
            var kmeans = new KMeans(k);

            kmeans.Distance = new JacDistance();

            kmeans.Learn(model.Constraints.ToArray());

            result.AddRange(kmeans.Clusters.Centroids);

            return new MathModel(result);

        }

    }
}
