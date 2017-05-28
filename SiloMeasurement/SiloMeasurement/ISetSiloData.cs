namespace SiloMeasurement
{
    /// <summary>
    /// Classes, that wants to get data from the GpioUltrasonicSensor class, have to implement this interface.
    /// </summary>
    public interface ISetSiloData
    {
        void setCurrentFillLevel(string value);
        void setSiloFillLevel(double value);
        void setWarning(string value);
    }
}
