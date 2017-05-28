using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace SiloMeasurement
{
    /// <summary>
    /// Class for handling ultrasonic sensors,
    /// designed for US-015 or HC-SR04.
    /// reads SiloData and provides it to ISetSiloData-classes.
    /// </summary>
    public class GpioUltrasonicSensor
    {
        private ISetSiloData view;
        private GpioLedAlert led;

        private GpioPin pinEcho;
        private GpioPin pinTrigger;
        private Stopwatch pulseLength;
        private double MaxLvl = 1;

        public GpioPin PinEcho
        {
            get { return pinEcho; }
            set { pinEcho = value; }
        }

        public GpioPin PinTrigger
        {
            get { return pinTrigger; }
            set { pinTrigger = value; }
        }

        public GpioUltrasonicSensor(ISetSiloData view, GpioLedAlert led)
        {
            this.view = view;
            this.led = led;
        }

        public async void InitGPIO(int echoPin, int triggerPin)
        {
            var gpio = GpioController.GetDefault();
            if (gpio == null)
            {
                PinEcho = null;
                PinTrigger = null;
                return;
            }

            PinEcho = gpio.OpenPin(echoPin);
            PinTrigger = gpio.OpenPin(triggerPin);

            PinTrigger.SetDriveMode(GpioPinDriveMode.Output);
            PinEcho.SetDriveMode(GpioPinDriveMode.Input);

            PinTrigger.Write(GpioPinValue.Low);

            await Task.Delay(100);
        }

        public void InitLed(int ledPin)
        {
            led.InitLed(ledPin);
        }

        public async void read(double reorderLevel)
        {
            pulseLength = new Stopwatch();

            // signal sensor to send impulse
            PinTrigger.Write(GpioPinValue.High);
            await Task.Delay(10);
            PinTrigger.Write(GpioPinValue.Low);

            // wait for impulse
            while (PinEcho.Read() == GpioPinValue.Low)
            {
            }

            // read imulse time
            pulseLength.Start();
            while (PinEcho.Read() == GpioPinValue.High)
            {
            }
            pulseLength.Stop();

            // calculate distance
            var elapsed = pulseLength.Elapsed.TotalSeconds;
            var distance = elapsed * 34300; // speed of sound
            distance /= 2;

            double distancePercentage = 100 - MaxLvl * distance;

            if (distancePercentage < 0)
                distancePercentage = 0;

            if (distancePercentage > 100)
                distancePercentage = 100;

            view.setCurrentFillLevel(Math.Round(distancePercentage, 2).ToString() + " %");            

            if (distancePercentage <= reorderLevel)
            {
                view.setWarning("Meldebestand erreicht!");
                led.On();
            }
            else
            {
                view.setWarning(string.Empty);
                led.Off();
            }

            view.setSiloFillLevel(distancePercentage);

            // save to Database
            DateTime now = DateTime.Now;
            SiloData data = new SiloData() { Value = distancePercentage, TimeString = now.ToString("yyyy-MM-dd HH:mm:ss") };
            data.Insert();
        }
    }
}
