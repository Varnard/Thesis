using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.MachineLearning;

namespace Thesis
{
    class KMeansClusterWizard : ClusterWizard
    {
        public int k { get; }

        public KMeansClusterWizard(int k, Data data) : base(data)
        {
            this.k = k;
        }

        public override List<double[][]> clusterNegatives()
        {
            KMeans kmeans = new KMeans(k);

            List<double[]>[] clusterArray = new List<double[]>[k];

            for (int i = 0; i < k; i++)
            {
                clusterArray[i] = new List<double[]>();
            }

            KMeansClusterCollection clusters = kmeans.Learn(negatives);

            int[] labels = clusters.Decide(negatives);

            for (int i = 0; i < negatives.Length; i++)
            {
                clusterArray[labels[i]].Add(negatives[i]);
            }

            List<double[][]> clusterList = new List<double[][]>();
            for (int i = 0; i < k; i++)
            {
                clusterList.Add(clusterArray[i].ToArray());
            }
            return clusterList;
        }
    }
}
