using System;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System.Windows.Forms;

namespace DataGraphProducer
{
    public class DataVisualiser
    {
        // Uses dictionary from ExtractClaimsByGeography method to generate a bar chart
        public void GenerateClaimsBarChartByGeography(Dictionary<string, decimal> geographyData)
        {
            //initialise new bar chart
            var plotModel = new PlotModel { Title = "Claims By Geography" };

            //establish data points, labelled with geographical area
            var barSeries = new BarSeries { LabelPlacement = LabelPlacement.Inside, LabelFormatString = "{0}" };

            //foreach loop of dictionary contents entry = key, value pair
            foreach (var entry in geographyData)
            {
                //add bar for each value
                barSeries.Items.Add(new BarItem { Value = (double)entry.Value });
            }

            //adds bars to plotmodel
            plotModel.Series.Add(barSeries);

            //add labels to axis
            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            foreach (var entry in geographyData)
            {
                categoryAxis.Labels.Add(entry.Key);
            }

            //add labels to plotmodel
            plotModel.Axes.Add(categoryAxis);
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 2000 }); // Max value on axis

            //creates view of bar chart
            using (var plotView = new PlotView { Model = plotModel })
            {
                var form = new Form { Text = "Bar Chart", ClientSize = new System.Drawing.Size(800, 600) };
                plotView.Dock = DockStyle.Fill;
                form.Controls.Add(plotView);
                form.ShowDialog();
            }
        }
        //add lossratio visualiser by geography
        //add claim visualiser by policy
        //add loss ratio visualiser by policy
        //add claim visualiser by month
        //add loss ratio visualiser by month
    }
}

