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
    }
}
