using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis
{
    class SingleClusterWizard : ClusterWizard
    {
        public SingleClusterWizard(Data data) : base(data)
        {
        }

        public override List<double[][]> clusterNegatives()
        {
            List<double[][]> temp = new List<double[][]>();
            temp.Add(negatives);
            return temp;
        }
    }
}
