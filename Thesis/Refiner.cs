using Accord.MachineLearning.VectorMachines;
using Accord.Statistics.Kernels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis
{
    public static class Refiner
    {
        public static Model removeRedundant(Model model)
        {
            var SVM = model.SVM;

            var points = DataProvider.getRefinementPoints();

            var newSVM = new List<SupportVectorMachine<Linear>>();

            foreach (var svm1 in SVM)
            {

                bool needed = false;
                var rest = new List<SupportVectorMachine<Linear>>(SVM);
                rest.Remove(svm1);
                var reducedModel = Model.fromSVM(rest);
                foreach (var point in points.X)
                {
                    if (!svm1.Decide(point) && reducedModel.Decide(point))
                    {
                        needed = true;
                    }
                }
                if (needed) newSVM.Add(svm1);
            }

            return Model.fromSVM(newSVM);
        }

        public static Model removeSimiliar(Model model, double distance)
        {
            return removeSimiliar(model, distance, Globals.angle);
        }

        public static Model removeSimiliar(Model model)
        {
            return removeSimiliar(model, Globals.distance, Globals.angle);
        }

        public static Model removeSimiliar(Model model, double distance, double angle)
        {
            var weights = model.GetMathModel();           

            var newWeights = new List<double[]>(weights);

            var newSVM = new List<SupportVectorMachine<Linear>>(model.SVM);

            int i = 0;
            while (i<newWeights.Count-1)
            { 
                for (int j = newWeights.Count-1; j > i; j--)
                {
                    var a = Math.Abs(Similiarity.calculateAngle(newWeights[i], newWeights[j]));
                    if (a<angle || a>180-angle)
                    {
                        var d = Similiarity.calculateDistance(newWeights[i], newWeights[j]);
                        if (d<distance)
                        {
                            newWeights.RemoveAt(j);
                            newSVM.RemoveAt(j);
                        }
                    }
                }

                i++;
            }


            return Model.fromSVM(newSVM);
        }
    }
}
