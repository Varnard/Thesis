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
        List<PlotView> plotList;


        public Visualization() {

            plotList = new List<PlotView>();

            Application.EnableVisualStyles();

        }

        public void Show()
        {

            int c = plotList.Count;

            int h = 500 + ((c-1) / 3) * 400;
            int w;
            if (c > 2) w = 1300;
            else w = 100 + c*400;

            var form = new Form()
            {
                Text = "Thesis",
                Height = h,
                Width = w
            };

            int i = 0;

            foreach (var plot in plotList)
            {
                plot.Location = new System.Drawing.Point((i%3)*400, 20+(i/3)*400);
                form.Controls.Add(plot);
                i++;
            }

            Application.Run(form);
        }

        public Visualization addClusters(ClusterWizard input, String title=" ")
        {
            var plot = new PlotView();
            plot.Size = new System.Drawing.Size(400, 400);

            var model = new PlotModel { Title = title };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 100 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 100 });
            plot.Model = model;

            var zeroOneAxis = new RangeColorAxis { Key = "zeroOneColors" };
            zeroOneAxis.AddRange(0, 0.1, OxyColors.Red);
            zeroOneAxis.AddRange(1, 1.1, OxyColors.ForestGreen);

            var clustersAxis = new RangeColorAxis { Key = "clustersColors" };
            clustersAxis.AddRange(0, 0.1, OxyColors.Red);
            clustersAxis.AddRange(1, 1.1, OxyColors.Orange);
            clustersAxis.AddRange(2, 2.1, OxyColors.OrangeRed);
            clustersAxis.AddRange(3, 3.1, OxyColors.DarkOrange);
            clustersAxis.AddRange(4, 4.1, OxyColors.DarkRed);
            clustersAxis.AddRange(5, 5.1, OxyColors.IndianRed);

            plot.Model.Axes.Add(zeroOneAxis);
            plot.Model.Axes.Add(clustersAxis);

            var dataPositiveSeries = new ScatterSeries { MarkerType = MarkerType.Circle, ColorAxisKey = "zeroOneColors" };
            foreach (var point in input.getPositives())
            {
                dataPositiveSeries.Points.Add(new ScatterPoint(point[0], point[1], 3, 1));
            }

            plot.Model.Series.Add(dataPositiveSeries);

            var dataNegativeSeries = new ScatterSeries { MarkerType = MarkerType.Circle, ColorAxisKey = "clustersColors" };
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

            plot.Model.Series.Add(dataNegativeSeries);

            plotList.Add(plot);

            return this;
        }

        public Visualization addModelPlot(ClusterWizard input, Model model)
        {
            return addModelPlot(input, model.GetMathModel(), true, " ");
        }

        public Visualization addModelPlot(ClusterWizard input, Model model, bool modelClassLabels)
        {
            return addModelPlot(input, model.GetMathModel(), modelClassLabels, " ");
        }

        public Visualization addModelPlot(ClusterWizard input, Model model, string title)
        {
            return addModelPlot(input, model.GetMathModel(), true, title);
        }

        public Visualization addModelPlot(ClusterWizard input, Model model, bool modelClassLabels, String title)
        {
            return addModelPlot(input, model.GetMathModel(), modelClassLabels, title);
        }

        public Visualization addModelPlot(ClusterWizard input, MathModel model)
        {
            return addModelPlot(input, model, true, " ");
        }

        public Visualization addModelPlot(ClusterWizard input, MathModel model, bool modelClassLabels)
        {
            return addModelPlot(input, model, modelClassLabels, " ");
        }

        public Visualization addModelPlot(ClusterWizard input, MathModel model, string title)
        {
            return addModelPlot(input, model, true, title);
        }

        public Visualization addModelPlot(ClusterWizard input, MathModel model, bool modelClassLabels, String title)
        {
            
            var plot = new PlotView();
            plot.Location = new System.Drawing.Point(450, 20);
            plot.Size = new System.Drawing.Size(400, 400);

            var plotModel = new PlotModel { Title = title };
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 100 });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 100 });
            plot.Model = plotModel;

            var zeroOneAxis = new RangeColorAxis { Key = "zeroOneColors" };
            zeroOneAxis.AddRange(0, 0.1, OxyColors.Red);
            zeroOneAxis.AddRange(1, 1.1, OxyColors.ForestGreen);

            plot.Model.Axes.Add(zeroOneAxis);            

            var positiveSeries = new ScatterSeries { MarkerType = MarkerType.Circle, ColorAxisKey = "zeroOneColors" };
            var negativeSeries = new ScatterSeries { MarkerType = MarkerType.Circle, ColorAxisKey = "zeroOneColors" };

            if (modelClassLabels)
            {
                foreach (var point in input.getAll())
                {
                    if (model.Decide(point))
                    {
                        positiveSeries.Points.Add(new ScatterPoint(point[0], point[1], 3, 1));
                    }
                    else
                    {
                        negativeSeries.Points.Add(new ScatterPoint(point[0], point[1], 3, 0));
                    }
                }
            }
            else
            {               
                foreach (var point in input.getPositives())
                {
                    positiveSeries.Points.Add(new ScatterPoint(point[0], point[1], 3, 1));
                }
                
                foreach (var cluster in input.getNegatives())
                {
                    foreach (var point in cluster)
                    {
                        negativeSeries.Points.Add(new ScatterPoint(point[0], point[1], 3, 0));
                    }
                }
            }

            plot.Model.Series.Add(positiveSeries);
            plot.Model.Series.Add(negativeSeries);

            foreach (var constraint in model.Constraints)
            {
                plot.Model.Series.Add(new FunctionSeries(x => (x * constraint[1] + constraint[0]) / -constraint[2], 0, 100, 0.2));                
            }

            plotList.Add(plot);

            return this;
        }
    }
}
