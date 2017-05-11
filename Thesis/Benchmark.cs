using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis
{
    class Benchmark
    {
        public static MathModel getMultipleLines2D()
        {
            Globals.n = 2;

            var constraints = new List<double[]>();

            double d = Globals.d;

            constraints.Add(new double[]{d*0.7,1,0});
            constraints.Add(new double[]{d/2,0.7,0.5});
            constraints.Add(new double[]{d,0,1});
            constraints.Add(new double[]{d,-0.5,0.3});
            constraints.Add(new double[]{2*d,0.5,-2.3});

            return new MathModel(constraints);
        }

        public static MathModel getHyperSurface()
        {
            var constraints = new List<double[]>();
            var const1 = new double[Globals.n + 1];

            const1[0] = Globals.d;

            for (int i = 1; i <= Globals.n; i++)
            {                
                const1[i] = 1;                
            }

            constraints.Add(const1);

            return new MathModel(constraints);
        }

        public static MathModel getHyperCube()
        {
            var constraints = new List<double[]>();

            for (int i=1;i<=Globals.n;i++)
            {
                var const1 = new double[Globals.n+1];
                var const2 = new double[Globals.n+1];

                const1[0] = Globals.d;
                const2[0] = Globals.d;
                const1[i] = 1;
                const2[i] = -1;

                constraints.Add(const1);
                constraints.Add(const2);
            }

            return new MathModel(constraints);
        }

        public static MathModel getHyperSphere()
        {
            var constraints = new List<double[]>();

            for (int i = 1; i <= Globals.n; i++)
            {
                var const1 = new double[Globals.n + 1];
                var const2 = new double[Globals.n + 1];

                const1[0] = Globals.d;
                const2[0] = Globals.d;
                const1[i] = 1;
                const2[i] = -1;

                constraints.Add(const1);
                constraints.Add(const2);
            }

            return new MathModel(constraints);
        }

        public static MathModel getSimplex()
        {
            var constraints = new List<double[]>();

            for (int i = 1; i <= Globals.n; i++)
            {
                var const1 = new double[Globals.n + 1];
                var const2 = new double[Globals.n + 1];

                const1[0] = Globals.d;
                const2[0] = Globals.d;
                const1[i] = 1;
                const2[i] = -1;

                constraints.Add(const1);
                constraints.Add(const2);
            }

            return new MathModel(constraints);
        }
    }
}
