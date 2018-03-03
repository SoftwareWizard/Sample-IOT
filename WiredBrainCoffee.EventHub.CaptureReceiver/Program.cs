using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Hadoop.Avro;
using Microsoft.Hadoop.Avro.Container;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using WiredBrainCoffee.EventHub.Model;

namespace WiredBrainCoffee.EventHub.CaptureReceiver
{
    public class Program
    {
        private const string containerName = "eventhubcapture";

        private const string connectionString = "DefaultEndpointsProtocol=https;" +
                                                "AccountName=wiredbraincoffeecapture1;" +
                                                "AccountKey=cJz5NiF53O3Rs7DXHF1dTSt/aMYrFQBWqZUpHKFxAgwL9pQ2XeAt0Swcp/CEkYoerUPY4i6Bgj3gYMck/RCXqw==;" +
                                                "EndpointSuffix=core.windows.net";

        public static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            await Task.Yield();
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var blobConainer = blobClient.GetContainerReference(containerName);
            var resultSegment = await blobConainer.ListBlobsSegmentedAsync(null, true, BlobListingDetails.All, null, null, null, null);

            foreach (var cloudBlockBlob in resultSegment.Results.OfType<CloudBlockBlob>())
            {
                await ProcessCloudBlockBlobAsync(cloudBlockBlob);
            }

            Console.ReadLine();
        }

        private static async Task ProcessCloudBlockBlobAsync(CloudBlockBlob cloudBlockBlob)
        {
            List<AvroRecord> avroRecords = await DownloadAvroRecordAsync(cloudBlockBlob);
            PrintCoffeeMachineDatas(avroRecords);
            await cloudBlockBlob.DeleteAsync();
        }

        private static async Task<List<AvroRecord>> DownloadAvroRecordAsync(CloudBlockBlob cloudBlockBlob)
        {
            var memoryStream = new MemoryStream();
            await cloudBlockBlob.DownloadToStreamAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            List<AvroRecord> avroRecords;
            using (var reader = AvroContainer.CreateGenericReader(memoryStream))
            {
                using (var sequentialReader = new SequentialReader<object>(reader))
                {
                    avroRecords = sequentialReader.Objects.OfType<AvroRecord>().ToList();
                }
            }

            return avroRecords;
        }

        private static CoffeeMachineData CreateCoffeeMachineData(AvroRecord avroRecord)
        {
            var body = avroRecord.GetField<byte[]>("Body");
            var dataAsJson = Encoding.UTF8.GetString(body);
            var coffeeMachineData = JsonConvert.DeserializeObject<CoffeeMachineData>(dataAsJson);
            return coffeeMachineData;
        }

        private static void PrintCoffeeMachineDatas(List<AvroRecord> avroRecords)
        {
            var coffeeMachineDatas = avroRecords.Select(CreateCoffeeMachineData);

            foreach (var coffeeMachineData in coffeeMachineDatas)
            {
                Console.WriteLine(coffeeMachineData);
            }
        }
    }
}
