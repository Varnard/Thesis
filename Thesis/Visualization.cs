using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.MachineLearning.VectorMachines;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

namespace Thesis
{
    public class Visualization
    {
        ClusterWizard input;
        Model model;
        Form form;
        PlotView beforePlot;
        PlotView afterPlot;
        PlotView comparisonPlot;


        public Visualization(ClusterWizard input, Model model) {
            this.input = input;
            this.model = model;
            form = new Form()
            {
                Text = "Thesis",
                Height = 900,
                Width = 900
            };

            beforePlot = new PlotView();
            beforePlot.Location = new System.Drawing.Point(0, 20);
            beforePlot.Size = new System.Drawing.Size(400, 400);

            var beforeModel = new PlotModel { Title = "Data"};
            beforeModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 100 });
            beforeModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 100 });
            beforePlot.Model = beforeModel;

            afterPlot = new PlotView();
            afterPlot.Location = new System.Drawing.Point(450, 20);
            afterPlot.Size = new System.Drawing.Size(400, 400);

            var afterModel = new PlotModel { Title = "SVM" };
            afterModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 100 });
            afterModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 100 });
            afterPlot.Model = afterModel;

            comparisonPlot = new PlotView();
            comparisonPlot.Location = new System.Drawing.Point(0, 420);
            comparisonPlot.Size = new System.Drawing.Size(400, 400);

            var comparisonModel = new PlotModel { Title = "Comparison" };
            comparisonModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 100 });
            comparisonModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 100 });
            comparisonPlot.Model = comparisonModel;

            form.Controls.Add(beforePlot);
            form.Controls.Add(afterPlot);
            form.Controls.Add(comparisonPlot);


            Application.EnableVisualStyles();

        }

        public void showResults()
        {
            var zeroOneAxis = new RangeColorAxis { Key = "zeroOneColors" };
            zeroOneAxis.AddRange(0, 0.1, OxyColors.Red);
            zeroOneAxis.AddRange(1, 1.1, OxyColors.ForestGreen);

            var afterZeroOneAxis = new RangeColorAxis { Key = "zeroOneColors" };
            afterZeroOneAxis.AddRange(0, 0.1, OxyColors.Red);
            afterZeroOneAxis.AddRange(1, 1.1, OxyColors.ForestGreen);

            var comparisonZeroOneAxis = new RangeColorAxis { Key = "zeroOneColors" };
            comparisonZeroOneAxis.AddRange(0, 0.1, OxyColors.Red);
            comparisonZeroOneAxis.AddRange(1, 1.1, OxyColors.ForestGreen);

            var clustersAxis = new RangeColorAxis { Key = "clustersColors" };
            clustersAxis.AddRange(0, 0.1, OxyColors.Red);
            clustersAxis.AddRange(1, 1.1, OxyColors.Orange);
            clustersAxis.AddRange(2, 2.1, OxyColors.OrangeRed);
            clustersAxis.AddRange(3, 3.1, OxyColors.DarkOrange);
            clustersAxis.AddRange(4, 4.1, OxyColors.DarkRed);
            clustersAxis.AddRange(5, 5.1, OxyColors.IndianRed);
        
            beforePlot.Model.Axes.Add(zeroOneAxis);
            beforePlot.Model.Axes.Add(clustersAxis);

            afterPlot.Model.Axes.Add(afterZeroOneAxis);

            comparisonPlot.Model.Axes.Add(comparisonZeroOneAxis);


            var dataPositiveSeries = new ScatterSeries { MarkerType = MarkerType.Circle, ColorAxisKey = "zeroOneColors"};
            foreach (var point in input.getPositives())
            {
                dataPositiveSeries.Points.Add(new ScatterPoint(point[0], point[1], 3, 1));
            }

            beforePlot.Model.Series.Add(dataPositiveSeries);

            var dataNegativeSeries = new ScatterSeries { MarkerType = MarkerType.Circle , ColorAxisKey = "clustersColors" };
            int i = 0;
            foreach (var cluster in input.getNegatives())
            {
                int color = i % 6;
                i++;
                foreach (var point in cluster)
                {
                    dataNegativeSeries.Points.Add(new ScatterPoint(point[0], point[1], 3, color));
                }
            }    

            beforePlot.Model.Series.Add(dataNegativeSeries);




            var svmPositiveSeries = new ScatterSeries { MarkerType = MarkerType.Circle, ColorAxisKey = "zeroOneColors" };
            var svmNegativeSeries = new ScatterSeries { MarkerType = MarkerType.Circle, ColorAxisKey = "zeroOneColors" };

            foreach (var point in input.getAll())
            {            
                if (model.Decide(point)) 
                {
                    svmPositiveSeries.Points.Add(new ScatterPoint(point[0], point[1], 3, 1));
                } 
                else 
                {
                    svmNegativeSeries.Points.Add(new ScatterPoint(point[0], point[1], 3, 0));
                }

            }

            afterPlot.Model.Series.Add(svmPositiveSeries);
            afterPlot.Model.Series.Add(svmNegativeSeries);



            var comparisonPositiveSeries = new ScatterSeries { MarkerType = MarkerType.Circle, ColorAxisKey = "zeroOneColors" };
            foreach (var point in input.getPositives())
            {
                comparisonPositiveSeries.Points.Add(new ScatterPoint(point[0], point[1], 3, 1));
            }

            var comparisonNegativeSeries = new ScatterSeries { MarkerType = MarkerType.Circle, ColorAxisKey = "zeroOneColors" };
            foreach (var cluster in input.getNegatives())
            {
                foreach (var point in cluster)
                {
                    comparisonNegativeSeries.Points.Add(new ScatterPoint(point[0], point[1], 3, 0));
                }
            }

            comparisonPlot.Model.Series.Add(comparisonNegativeSeries);
            comparisonPlot.Model.Series.Add(comparisonPositiveSeries);

            foreach (var constraint in model.GetMathModel())
            {
                afterPlot.Model.Series.Add(new FunctionSeries(x => (x * constraint[1] + constraint[0]) / -constraint[2], 0, 100, 0.2));
                comparisonPlot.Model.Series.Add(new FunctionSeries(x => (x * constraint[1] + constraint[0]) / -constraint[2], 0, 100, 0.2));
            }

            Application.Run(form);
        }
    }
}
