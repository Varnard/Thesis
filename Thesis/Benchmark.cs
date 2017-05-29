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

            constraints.Add(new double[]{d*0.7,1,0.001});

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
            Globals.setMinMax(-Globals.d, 2 * Globals.d);

            var constraints = new List<double[]>();

            for (int i=1;i<=Globals.n;i++)
            {
                var const1 = new double[Globals.n+1];
                var const2 = new double[Globals.n+1];

                const1[i] = 1;

                const2[0] = Globals.d;    
                const2[i] = -1;

                constraints.Add(const1);
                constraints.Add(const2);
            }

            return new MathModel(constraints);
        }

        public static SphereModel getHyperSphere()
        {
            Globals.setMinMax(-2*Globals.d, 2 * Globals.d);

            var constraints = new List<double[]>();

            var const0 = new double[Globals.n + 1];
            const0[0] = Globals.d;

            for (int i = 0; i < Globals.n; i++)
            {
                const0[i + 1] = 1;
            }

            constraints.Add(const0);

            return new SphereModel(constraints);
        }

        public static MathModel getSimplex()
        {
            Globals.setMinMax(-1, 2 + Globals.d);

            var constraints = new List<double[]>();

            double tg = Math.Tan(Math.PI / 12);
            double ctg = 1/(Math.Tan(Math.PI / 12));

            for (int i = 1; i < Globals.n; i++)
            {
                var const1 = new double[Globals.n + 1];
                var const2 = new double[Globals.n + 1];

                const1[i] = ctg;
                const1[i + 1] = -tg;
                const2[i+1] = ctg;
                const2[i] = -tg;

                constraints.Add(const1);
                constraints.Add(const2);
            }

            

            if (Globals.n > 2)
            {
                var const3 = new double[Globals.n + 1];
                var const4 = new double[Globals.n + 1];

                const3[Globals.n] = ctg;
                const3[1] = -tg;
                const4[1] = ctg;
                const4[Globals.n] = -tg;

                constraints.Add(const3);
                constraints.Add(const4);
            }

            var const0 = new double[Globals.n + 1];
            const0[0] = Globals.d;

            for (int i = 0; i < Globals.n; i++)
            {
                const0[i+1] = -1;   
            }

            constraints.Add(const0);


            return new MathModel(constraints);
        }
    }
}
