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

            //establish data points, labelled with geographical area {0} = first parameter
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
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 5000 }); // Max value on axis

            //creates view of bar chart
            using (var plotView = new PlotView { Model = plotModel })
            {
                var form = new Form { Text = "Bar Chart", ClientSize = new System.Drawing.Size(800, 600) };
                plotView.Dock = DockStyle.Fill;
                form.Controls.Add(plotView);
                form.ShowDialog();
            }
        }

        //loss ratio visualiser by geography
        public void GenerateLossRatioBarChartByGeography(Dictionary<string, decimal> lossRatioGeographyData)
        {
            //initialise new bar chart
            var plotModel = new PlotModel { Title = "Loss Ratio By Geography" };

            //establish data points, labelled with geographical area
            var barSeries = new BarSeries { LabelPlacement = LabelPlacement.Inside, LabelFormatString = "{0}" };

            //foreach loop of dictionary contents entry = key, value pair
            foreach (var entry in lossRatioGeographyData)
            {
                //add bar for each value
                barSeries.Items.Add(new BarItem { Value = (double)entry.Value });
            }
            //adds bars to plotmodel
            plotModel.Series.Add(barSeries);

            //add labels to axis
            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            foreach (var entry in lossRatioGeographyData)
            {
                categoryAxis.Labels.Add(entry.Key);
            }

            //add labels to plotmodel
            plotModel.Axes.Add(categoryAxis);
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 100 }); // Max value on axis max % = 100

            //creates view of bar chart
            using (var plotView = new PlotView { Model = plotModel })
            {
                var form = new Form { Text = "Bar Chart", ClientSize = new System.Drawing.Size(800, 600) };
                plotView.Dock = DockStyle.Fill;
                form.Controls.Add(plotView);
                form.ShowDialog();
            }
        }

        public void GenerateMonthlyClaimsLineGraph(Dictionary<string, decimal> monthlyClaimsData)
        {
            //initialise new line graph
            var plotModel = new PlotModel { Title = "Claims Per Month" };

            //edit appearance of data types
            var lineSeries = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White
            };

            int index = 0; //x-axis value
            foreach (var entry in monthlyClaimsData)
            {
                lineSeries.Points.Add(new DataPoint(index, (double)entry.Value));
                index++;
            }

            plotModel.Series.Add(lineSeries);

            //establish x-axis 
            var xAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Month",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            };

            // Add month labels to X-axis
            foreach (var entry in monthlyClaimsData)
            {
                xAxis.Labels.Add(entry.Key);
            }

            //establish y-axis
            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 5000,
                Title = "Amount Claimed",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            };

            //add axes points to graph
            plotModel.Axes.Add(xAxis);
            plotModel.Axes.Add(yAxis);

            var form = new Form
            {
                Text = "Monthly Claims Line Graph",
                ClientSize = new System.Drawing.Size(800, 600)
            };

            //appearance
            var plotView = new PlotView { Model = plotModel, Dock = DockStyle.Fill };
            form.Controls.Add(plotView);
            form.ShowDialog();
        }
    }
}

