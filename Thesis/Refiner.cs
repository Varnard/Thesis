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
        public static List<SupportVectorMachine<Linear>> removeRedundant(List<SupportVectorMachine<Linear>> SVM, Data points)
        {
            var newSVM = new List<SupportVectorMachine<Linear>>();

            foreach (var svm1 in SVM)
            {

                bool needed = false;
                var asdf = new List<SupportVectorMachine<Linear>>(SVM);
                asdf.Remove(svm1);
                var model = new Model(asdf);
                foreach (var point in points.X)
                {
                    if (!svm1.Decide(point) && model.Decide(point))
                    {
                        needed = true;
                    }
                }
                if (needed) newSVM.Add(svm1);
            }

            return newSVM;
        }
    }
}
