using System;

namespace WiredBrainCoffee.EventHubSender.Model
{
    public class CoffeeMachineData
    {
        public string City { get; set; }
        public string SerialNumber { get; set; }
        public string SensorType { get; set; }
        public int SensorValue { get; set; }
        public DateTime RecordingTime { get; set; }
    }
}