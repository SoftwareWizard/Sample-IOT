using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

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
            var cloudBlockBlobs = resultSegment.Results.OfType<CloudBlockBlob>();
            var cloudBlockBlobGroups = cloudBlockBlobs.GroupBy(item => GetPartitionId(item.Name));

            foreach (var group in cloudBlockBlobGroups)
            {
                Console.WriteLine($"{group.Key}");
                foreach (var cloudBlockBlob in group)
                {
                    await ProcessCloudBlockBlobAsync(group.Key, cloudBlockBlob);
                }
            }

            Console.ReadLine();
        }


        private static async Task ProcessCloudBlockBlobAsync(string partitionId, CloudBlockBlob cloudBlockBlob)
        {
            const string rootPath = @"C:\Temp\blobStorage\";
            var path = Path.Combine(rootPath, partitionId);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filename = CreateFilename(cloudBlockBlob.Name);
            var filePath = $@"{path}\{filename}";

            Console.WriteLine(filePath);
            await cloudBlockBlob.DownloadToFileAsync(filePath, FileMode.Create);
        }

        private static string GetPartitionId(string name)
        {
            return name.Split('/')[2];
        }

        private static string CreateFilename(string name)
        {
            var tokens = name.Split('/');
            return $"{tokens[5]}-{tokens[4]}-{tokens[3]}_{tokens[6]}{tokens[7]}_{tokens[8]}";
        }
    }
}
