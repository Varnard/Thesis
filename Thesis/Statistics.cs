using ExperimentDatabase;
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
        double posJaccard { get; }
        double negJaccard { get; }


        public Statistics(Data original, bool[] decisions) 
        {
            TP = 0;
            FP = 0;
            TN = 0;
            FN = 0;

            int posJacDenominator=0;
            int posJacNumerator=0;
            int negJacDenominator=0;
            int negJacNumerator=0;

            bool[] correct = original.getYAsBool();

            for (int i = 0; i < decisions.Length; i++)
            {             
                if (correct[i])
                {
                    if (decisions[i])
                    {
                        TP++;
                        posJacNumerator++;
                    }
                    else
                    {
                        FN++;
                        negJacDenominator++;
                    }
                    posJacDenominator++;
                }
                else
                {
                    if (decisions[i])
                    {
                        FP++;
                        posJacDenominator++;
                    }
                    else
                    {
                        TN++;
                        negJacNumerator++;
                    }
                    negJacDenominator++;
                }
            }

            posJaccard = (double)posJacNumerator / posJacDenominator;
            negJaccard = (double)negJacNumerator / negJacDenominator;
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
            "\nSpecificity: " + Math.Round(getSpecificity(),3)+
            "\nPositive Jaccard index: "+Math.Round(posJaccard,3) +
            "\nNegative Jaccard index: " + Math.Round(negJaccard, 3);
        }

        public void saveMeasures(Experiment experiment)
        {
            experiment.Add("Positive_Jaccard_index", Math.Round(posJaccard, 3));
            experiment.Add("Negative_Jaccard_index", Math.Round(negJaccard, 3));
            experiment.Add("Accuracy", Math.Round(getAccuracy(), 3));
            experiment.Add("Precision", Math.Round(getPrecision(), 3));
            experiment.Add("Recall", Math.Round(getRecall(), 3));
            experiment.Add("Specificity", Math.Round(getSpecificity(), 3));
            experiment.Save();

        }
    }
}
