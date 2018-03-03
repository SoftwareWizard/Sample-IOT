using System;

namespace WiredBrainCoffee.EventHub.Model
{
    public class CoffeeMachineData
    {
        public string City { get; set; }
        public string SerialNumber { get; set; }
        public string SensorType { get; set; }
        public int SensorValue { get; set; }
        public DateTime RecordingTime { get; set; }

        public override string ToString()
        {
            return $"{nameof(City)}: {City}| {nameof(SerialNumber)}: {SerialNumber}| {nameof(SensorType)}: {SensorType}| {nameof(SensorValue)}: {SensorValue}| {nameof(RecordingTime)}: {RecordingTime}";
        }
    }
}