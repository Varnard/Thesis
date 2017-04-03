using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis
{
    public class Statistics
    {
        int TP { get; }
        int FP { get; }
        int TN { get; }
        int FN { get; }


        public Statistics(Data original, bool[] decisions) 
        {
            TP = 0;
            FP = 0;
            TN = 0;
            FN = 0;

            for (int i = 0; i < decisions.Length; i++)
            {
                bool[] correct = original.getYAsBool();

                if (correct[i])
                {
                    if (decisions[i])
                    {
                        TP++;
                    }
                    else
                    {
                        FN++;
                    }
                }
                else
                {
                    if (decisions[i])
                    {
                        FP++;
                    }
                    else
                    {
                        TN++;
                    }
                }
            }
        }

        public double getAccuracy()
        {
            return (double)(TP + TN) / (TP + FP + TN + FN);
        }

        public double getPrecision()
        {
            return (double)TP / (TP + FP);
        }

        public double getRecall()
        {
            return (double)TP / (TP + FN);

        }

        public double getSpecificity()
        {
            return (double)TN / (FP + TN);
        }

        public string getMeasures()
        {
            return "Accuracy: " + Math.Round(getAccuracy(),3) +
            "\nPrecision: " + Math.Round(getPrecision(),3) +
            "\nRecall: " + Math.Round(getRecall(),3) +
            "\nSpecificity: " + Math.Round(getSpecificity(),3);
        }

    }
}
