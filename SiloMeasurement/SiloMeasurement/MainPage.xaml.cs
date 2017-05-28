using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SiloMeasurement
{
    /// <summary>
    /// units for the time of the timer.Interval
    /// </summary>
    public enum Interval
    {
        Sekunden,
        Minuten,
        Stunden
    }

    /// <summary>
    /// Main page/view for showing data and handling user input
    /// </summary>
    public sealed partial class MainPage : Page, ISetSiloData
    {
        private DispatcherTimer timer;
        private TimeSpan time;       
        private LineChartWrapper lineChartWrapper;
        
        private GpioUltrasonicSensor ultrasonicSensor;

        public double ReorderLevel
        {
            get
            {
                double reorderLevel;
                try
                {
                    reorderLevel = Convert.ToDouble(tbReorderLevel.Text);
                }
                catch
                {
                    reorderLevel = 0;
                }

                return reorderLevel;
            }
        }

        public MainPage()
        {
            this.InitializeComponent();

            ultrasonicSensor = new GpioUltrasonicSensor(this, new GpioLedAlert());
            ultrasonicSensor.InitLed(2);
            ultrasonicSensor.InitGPIO(23, 18);

            time = TimeSpan.FromMilliseconds(400);
            timer = new DispatcherTimer();
            timer.Interval = time;
            timer.Tick += Timer_Tick;

            lineChartWrapper = new LineChartWrapper(this.LineChartTrend);
            lineChartWrapper.setChartData("Heute", ReorderLevel);
        }

        private void Timer_Tick(object sender, object e)
        {
            ultrasonicSensor.read(ReorderLevel);
        }

        // implemented from ISetSiloData
        public void setCurrentFillLevel(string value)
        {
            tbCurrentFillLevel.Text = value;
        }

        // implemented from ISetSiloData
        public void setSiloFillLevel(double value)
        {
            prgbSiloFillLevel.Value = value;
        }

        // implemented from ISetSiloData
        public void setWarning(string value)
        {
            tbWarning.Text = value;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();

            string text = "";
            try { text = ((ComboBoxItem)cbxTimeUnit.SelectedItem).Content.ToString(); } catch { text = ""; }

            if (text == Interval.Sekunden.ToString())
            {
                try { time = TimeSpan.FromSeconds(Convert.ToDouble(tbTimer.Text)); }
                catch { ErrorMsg.Text = "Der Intervall konnte nicht übernommen werden."; return; }
            }
            if (text == Interval.Minuten.ToString())
            {
                try { time = TimeSpan.FromMinutes(Convert.ToDouble(tbTimer.Text)); }
                catch { ErrorMsg.Text = "Der Intervall konnte nicht übernommen werden."; return; }
            }
            if (text == Interval.Stunden.ToString())
            {
                try { time = TimeSpan.FromHours(Convert.ToDouble(tbTimer.Text)); }
                catch { ErrorMsg.Text = "Der Intervall konnte nicht übernommen werden."; return; }
            }

            timer.Interval = time;

            if (ultrasonicSensor.PinEcho != null && ultrasonicSensor.PinTrigger != null)
            {
                timer.Start();
            }
            else
            {
                ErrorMsg.Text = "Es konnte kein Sonsor" + Environment.NewLine + "an diesem Gerät gefunden werden.";
                return;
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void cbxTrendPeriod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = string.Empty;
            if (e.AddedItems != null && e.AddedItems.Count != 0)
            {
                try { selected = ((ComboBoxItem)e.AddedItems.First()).Content.ToString(); }
                catch { selected = string.Empty; }
            }

            string deSelected = string.Empty;
            if (e.RemovedItems != null && e.RemovedItems.Count != 0)
            {
                try { deSelected = ((ComboBoxItem)e.RemovedItems.First()).Content.ToString(); }
                catch { deSelected = string.Empty; }
            }

            if (selected == deSelected || selected == string.Empty)
                return;

            if(lineChartWrapper != null)
                lineChartWrapper.setChartData(selected, ReorderLevel);
        }

        private void btnRefreshChart_Click(object sender, RoutedEventArgs e)
        {
            string selected = string.Empty;

            if (cbxTrendPeriod.SelectedItem != null)
            {
                try { selected = ((ComboBoxItem)cbxTrendPeriod.SelectedItem).Content.ToString(); }
                catch { selected = string.Empty; }
            }

            if (selected == string.Empty || lineChartWrapper == null)
                return;

            lineChartWrapper.setChartData(selected, ReorderLevel);
        }
    }
}
