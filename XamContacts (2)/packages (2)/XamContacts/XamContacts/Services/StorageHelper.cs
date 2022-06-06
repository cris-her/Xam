using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using XamContacts.Extensions;

namespace XamContacts.Services
{
    public class StorageHelper
    {
        private CloudBlobClient CurrentClient;

        public StorageHelper()
        {
            CurrentClient = CreateBlobClient();
        }

        public CloudStorageAccount GetAccount(bool useHttps = false)
        {
            //StorageCredentials credentials =
            //    new StorageCredentials("xamcontacts",
            //    "tPKziQIrpoV2wf8upLSaOpJ/8s6nd2DEzHt5zq87aOrMTL3MDPMxfwwkrGwFNdc6L/LztM8flhvaifW4p5O0NQ==");
            //CloudStorageAccount account =
            //    new CloudStorageAccount(credentials, useHttps);

            //CloudStorageAccount account =
            //    CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=xamcontacts;AccountKey=tPKziQIrpoV2wf8upLSaOpJ/8s6nd2DEzHt5zq87aOrMTL3MDPMxfwwkrGwFNdc6L/LztM8flhvaifW4p5O0NQ==;EndpointSuffix=core.windows.net");

            CloudStorageAccount account =
                CloudStorageAccount.DevelopmentStorageAccount;

            return account;
        }

        private CloudBlobClient CreateBlobClient()
        {
            var account = GetAccount();
            CloudBlobClient client =
                account.CreateCloudBlobClient();
            return client;
        }

        public async Task<CloudBlobContainer> CreateContainer(string containerName)
        {            
            CloudBlobContainer container =
                CurrentClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();
            return container;
        }

        public async Task<CloudBlobContainer> GetContainer(string containerName)
        {
            var container =
                await CreateContainer(containerName);
            return container;
        }

        private async Task<CloudBlockBlob> GetBlob(
            string containerName, string blobName)
        {
            var container = await GetContainer(containerName);
            CloudBlockBlob blob =
                container.GetBlockBlobReference(blobName);
            return blob;
        }

        public async Task CreateTextBlob(string containerName,
            string blobName, string data)
        {
            var blob = await GetBlob(containerName, blobName);
            using (var stream = new MemoryStream(data.Encode()))
            {
                await blob.UploadFromStreamAsync(stream);
            }
        }

        public async Task<string> DownloadTextBlob(string containerName,
            string blobName)
        {
            var blob = await GetBlob(containerName, blobName);
            string data = string.Empty;
            using (var stream = new MemoryStream())
            {
                await blob.DownloadToStreamAsync(stream);
                stream.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(stream);
                data = await reader.ReadToEndAsync();
            }
            return data;
        }

        public async Task ListBlobs(string containerName)
        {
            var container = await GetContainer(containerName);
            var list = await container.ListBlobsSegmentedAsync(null);
            foreach(var item in list.Results)
            {
                Debug.WriteLine(item.Uri);
            }
        }

        public async Task ChangeContainerPermission(string containerName,
            BlobContainerPublicAccessType accessType)
        {
            var container = await GetContainer(containerName);
            BlobContainerPermissions permissions =
                new BlobContainerPermissions
                {
                    PublicAccess = accessType
                };
            await container.SetPermissionsAsync(permissions);
        }

        public async Task DeleteBlob(string containerName, string blobName)
        {
            var blob = await GetBlob(containerName, blobName);
            await blob.DeleteAsync();
        }

        public async Task DeleteContainer(string containerName)
        {
            var container = await GetContainer(containerName);
            await container.DeleteAsync();
        }
    }
}
