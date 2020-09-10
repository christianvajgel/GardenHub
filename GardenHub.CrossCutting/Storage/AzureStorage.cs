using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GardenHub.CrossCutting.Storage
{
    public class AzureStorage
    {
        private AzureStorageOptions Options { get; set; }

        private CloudBlobClient CloudBlobClient { get; set; }
        private CloudBlobDirectory ImagesDirectory { get; set; }

        public AzureStorage(IOptions<AzureStorageOptions> options)
        {
            this.Options = options.Value;
            this.Setup();
        }

        private void Setup()
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(this.Options.ConnectionString);
            this.CloudBlobClient = account.CreateCloudBlobClient();
            CloudBlobContainer container = this.CloudBlobClient.GetContainerReference("gardenhub-images-post");
            this.ImagesDirectory = container.GetDirectoryReference(".");

        }

        public async Task<String> SaveToStorage(byte[] buffer, string fileName)
        {
            CloudBlockBlob cloudBlockBlob = null;
            cloudBlockBlob = this.ImagesDirectory.GetBlockBlobReference(fileName);
            using (Stream stream = new MemoryStream(buffer))
            {
                await cloudBlockBlob.UploadFromStreamAsync(stream);
            }
            return $"https://assetsforwork.blob.core.windows.net/gardenhub-images-post/{fileName}";
        }

        //public async void DeleteFromStorage(byte[] buffer, string fileName)
        //{
        //    CloudBlockBlob cloudBlockBlob = null;
        //    cloudBlockBlob = this.ImagesDirectory.GetBlockBlobReference(fileName);
        //    using (Stream stream = new MemoryStream(buffer))
        //    {
        //        await cloudBlockBlob.DeleteIfExistsAsync(default);
        //    }
        //   // return $"https://assetsforwork.blob.core.windows.net/gardenhub-images-post/{fileName}";
        //}

        public void DeleteBlob(string fileName)
        {
            CloudBlockBlob cloudBlockBlob = null;
            cloudBlockBlob = this.ImagesDirectory.GetBlockBlobReference(fileName);

            cloudBlockBlob.DeleteIfExists();
        }

        //public void DeleteBlob()
        //{
        //    var _containerName = "appcontainer";
        //    string _storageConnection = CloudConfigurationManager.GetSetting("StorageConnectionString");
        //    CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_storageConnection);
        //    CloudBlobClient _blobClient = cloudStorageAccount.CreateCloudBlobClient();

        //    CloudBlobContainer _cloudBlobContainer = _blobClient.GetContainerReference(_containerName);

        //    CloudBlockBlob _blockBlob = _cloudBlobContainer.GetBlockBlobReference("f115a610-a899-42c6-bd3f-74711eaef8d5-.jpg");
        //    //delete blob from container    
        //    _blockBlob.Delete();
        //}
    }
}
