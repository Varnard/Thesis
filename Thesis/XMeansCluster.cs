using Accord.MachineLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis
{
    class XMeansClusterWizard : ClusterWizard
    {
        public int k { get; private set; }

        public XMeansClusterWizard(Data data, int k) : base(data)
        {
            this.k = k;
        }

        public XMeansClusterWizard(Data data) : base(data)
        {
            this.k = Globals.k;
        }

        public override List<double[][]> clusterNegatives()
        {
            return new KMeansClusterWizard(base.negatives, base.positives, k).clusterNegatives();
        }

    }
}
