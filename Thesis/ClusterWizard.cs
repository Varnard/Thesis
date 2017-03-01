using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis
{
    public abstract class ClusterWizard
    {
    
        protected double[][] negatives;
        protected double[][] positives;

        public ClusterWizard(Data data)
        {
            List<double[]> neg = new List<double[]>();
            List<double[]> pos = new List<double[]>();

            for (int i = 0; i < data.Y.Length; i++)
            {
                if (data.Y[i] == 1) pos.Add(data.X[i]);
                else neg.Add(data.X[i]);
            }
            negatives = neg.ToArray();
            positives = pos.ToArray();
        }

        public List<double[][]> getNegatives()
        {
            return clusterNegatives();
        }

        public double[][] getPositives()
        {
            return positives;
        }

        public abstract List<double[][]> clusterNegatives();

    }
}
