using System;
using System.Collections.Generic;
using System.Linq;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace SiloMeasurement
{
    /// <summary>
    /// Wrapper class for trendlinecharts,
    /// gets SiloData and supplies it to the lineChart.
    /// </summary>
    public class LineChartWrapper
    {
        private class ChartValueItem
        {
            public string Date { get; set; }
            public double Value { get; set; }
        }

        private Chart trendLineChart;

        public LineChartWrapper(Chart lineChart)
        {
            trendLineChart = lineChart;
        }   

        public void setChartData(string period, double reorderLevel)
        {
            trendLineChart.Title = "Trend: " + period;

            List<SiloData> data;
            try
            {
                data = SiloData.ReadAll();
            }
            catch
            {
                data = new List<SiloData>();
            }

            // Get items for series[0]
            List<ChartValueItem> items1 = getItems(period, data);            

            // Supply items to the series[0]
            ((LineSeries)trendLineChart.Series[0]).ItemsSource = items1;
            // Change Y-Axis range from 0 to 100 with interval of 20 of Series[0]
            ((LineSeries)trendLineChart.Series[0]).DependentRangeAxis =
               new LinearAxis
               {
                   Minimum = 0,
                   Maximum = 100,
                   Orientation = AxisOrientation.Y,
                   Interval = 50,
                   ShowGridLines = true                   
               };
            // refresh series[0]
            ((LineSeries)trendLineChart.Series[0]).Refresh();

            // Supply items to the series[1]
            List<ChartValueItem> items2 = new List<ChartValueItem>();
            foreach (var item in items1)
            {
                items2.Add(new ChartValueItem() { Date = item.Date, Value = reorderLevel });
            }
            ((LineSeries)trendLineChart.Series[1]).ItemsSource = items2;
            // Change Y-Axis range from 0 to 100 with interval of 20 of Series[1]
            ((LineSeries)trendLineChart.Series[1]).DependentRangeAxis =
               new LinearAxis
               {
                   Minimum = 0,
                   Maximum = 100,
                   Orientation = AxisOrientation.Y,
                   Interval = 50,
                   ShowGridLines = true
               };
            // refresh series[1]
            ((LineSeries)trendLineChart.Series[1]).Refresh();
        }

        private List<ChartValueItem> getItems(string period, List<SiloData> data)
        {
            List<ChartValueItem> items1 = new List<ChartValueItem>();
            List<SiloData> filteredData = new List<SiloData>();

            if (period == "Heute")
            {
                DateTime hour = DateTime.Today;
                DateTime until = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour + 1, 0, 0);

                data = data.Where(x => x.Time >= hour && x.Time <= until).ToList();

                // last value from each hour
                while (hour < until)
                {
                    SiloData siloDataItem = new SiloData() { TimeString = hour.ToString(), Value = 0 };

                    try
                    {
                        List<SiloData> current = data.Where(x => x.Time >= hour && x.Time < hour.AddHours(1)).ToList();
                        if (current.Count > 0)
                            siloDataItem = current.OrderByDescending(x => x.Time).First();
                    }
                    catch
                    {
                    }

                    filteredData.Add(siloDataItem);
                    hour = hour.AddHours(1);
                }

                // Prepare items for Series[0] (first Trendline)
                foreach (var item in filteredData)
                {
                    items1.Add(new ChartValueItem { Date = item.Time.ToString("HH:00"), Value = Math.Round(item.Value, 2) });
                }
            }

            if (period == "Letzte 7 Tage")
            {
                DateTime day = DateTime.Today.AddDays(-6);
                DateTime until = DateTime.Today;

                data = data.Where(x => x.Time >= day && x.Time < until.AddDays(1)).ToList();

                // last value from each day
                while (day <= until)
                {
                    SiloData siloDataItem = new SiloData() { TimeString = day.ToString(), Value = 0 };

                    try
                    {
                        List<SiloData> current = data.Where(x => x.Time >= day && x.Time < day.AddDays(1)).ToList();
                        if (current.Count > 0)
                            siloDataItem = current.OrderByDescending(x => x.Time).First();
                    }
                    catch
                    {
                    }

                    filteredData.Add(siloDataItem);
                    day = day.AddDays(1);
                }

                // Prepare items for Series[0] (first Trendline)
                foreach (var item in filteredData)
                {
                    items1.Add(new ChartValueItem { Date = item.Time.ToString("dd.MM."), Value = Math.Round(item.Value, 2) });
                }
            }

            if (period == "Dieser Monat")
            {
                DateTime dayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                DateTime until = DateTime.Today;

                data = data.Where(x => x.Time >= dayOfMonth && x.Time < until.AddDays(1)).ToList();

                // last value from each day
                while (dayOfMonth <= until)
                {
                    SiloData siloDataItem = new SiloData() { TimeString = dayOfMonth.ToString(), Value = 0 };

                    try
                    {
                        List<SiloData> current = data.Where(x => x.Time >= dayOfMonth && x.Time < dayOfMonth.AddDays(1)).ToList();
                        if (current.Count > 0)
                            siloDataItem = current.OrderByDescending(x => x.Time).First();
                    }
                    catch
                    {
                    }

                    filteredData.Add(siloDataItem);
                    dayOfMonth = dayOfMonth.AddDays(1);
                }

                // Prepare items for Series[0] (first Trendline)
                foreach (var item in filteredData)
                {
                    items1.Add(new ChartValueItem { Date = item.Time.ToString("dd.MM."), Value = Math.Round(item.Value, 2) });
                }
            }

            return items1;
        }
    }
}
