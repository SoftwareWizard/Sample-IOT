using System;

namespace WiredBrainCoffee.MachineSimulator.UI.Model
{
    public class CoffeeMachineData
    {
        public string City { get; set; }
        public string SerialNumber { get; set; }
        public string Sensortype { get; set; }
        public int SensorValue { get; set; }
        public DateTime RecordingTime { get; set; }
    }
}