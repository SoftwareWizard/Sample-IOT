﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Newtonsoft.Json;
using WiredBrainCoffee.EventHub.Model;

namespace WiredBrainCoffee.EventHub.Processor
{
    public class WiredBrainCoffeeEventProcessor : IEventProcessor
    {
        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine($"Initialized processor for partition {context.PartitionId}");
            return Task.CompletedTask;
        }

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine($"Shutting down processor for partition {context.PartitionId} Reason: {reason}");
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            if (messages != null)
            {
                foreach (var eventData in messages)
                {
                    var dataAsJson = Encoding.UTF8.GetString(eventData.Body.Array);
                    var coffeeMachineData = JsonConvert.DeserializeObject<CoffeeMachineData>(dataAsJson);
                    Console.WriteLine($"{coffeeMachineData} | PartionId: {context.PartitionId} | Offset: { eventData.SystemProperties.Offset}");
                }
            }

            return context.CheckpointAsync();
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            Console.WriteLine($"Error for partition {context.PartitionId}: error {error.Message}");
            return Task.CompletedTask;
        }
    }
}
