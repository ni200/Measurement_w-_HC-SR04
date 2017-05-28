using System;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;

namespace SiloMeasurement
{
    /// <summary>
    /// Class for handling GPIO-LEDs.
    /// </summary>
    public class GpioLedAlert
    {
        private GpioPin gpioLed;
        private TimeSpan timeAlert;
        private DispatcherTimer timerAlert;

        public GpioLedAlert()
        {
            timeAlert = TimeSpan.FromMilliseconds(400);
            timerAlert = new DispatcherTimer();
            timerAlert.Interval = timeAlert;
            timerAlert.Tick += Timer_TickAlert;
        }

        public void InitLed(int ledPin)
        {
            var gpioLED = GpioController.GetDefault();
            if (gpioLED == null)
                return;

            gpioLed = gpioLED.OpenPin(ledPin);
            gpioLed.SetDriveMode(GpioPinDriveMode.Output);
            gpioLed.Write(GpioPinValue.Low);
        }

        private async void Timer_TickAlert(object sender, object e)
        {
            gpioLed.Write(GpioPinValue.High);
            await Task.Delay(50);
            gpioLed.Write(GpioPinValue.Low);
            await Task.Delay(50);
            gpioLed.Write(GpioPinValue.High);
            await Task.Delay(50);
            gpioLed.Write(GpioPinValue.Low);
            await Task.Delay(50);
        }
        
        public void On()
        {
             if(!timerAlert.IsEnabled && gpioLed != null)
                timerAlert.Start();
        }
        public void Off()
        {
            timerAlert.Stop();
        }
    }
}
