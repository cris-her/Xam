using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Azure;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace democsbackendService.Controllers
{
    [Authorize]
    [MobileAppController]
    public class GetStorageTokenController : ApiController
    {
        public string ConnectionString { get; }
        public CloudStorageAccount StorageAccount { get; }
        public CloudBlobClient BlobClient { get; }

        private const string connString =
            "AzureKey";

        public GetStorageTokenController()
        {
            ConnectionString =
                CloudConfigurationManager.GetSetting(connString);
            StorageAccount =
                CloudStorageAccount.Parse(ConnectionString);
            BlobClient = StorageAccount.CreateCloudBlobClient();
        }

        private const string containerName = "userdata";
        // GET api/GetStorageToken
        [HttpGet]
        public async Task<StorageTokenViewModel> GetAsync(string fileName)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var userId =
                claimsPrincipal
                    .FindFirst(ClaimTypes.NameIdentifier)
                    .Value.Substring(4);
            var container = BlobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();

            var directory = container.GetDirectoryReference(userId);
            var blobName = fileName;
            var blob = directory.GetBlockBlobReference(blobName);

            var blobPolilcy = new SharedAccessBlobPolicy
            {
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(60),
                Permissions = SharedAccessBlobPermissions.Read
                              | SharedAccessBlobPermissions.Write
                              | SharedAccessBlobPermissions.Create
            };
            return new StorageTokenViewModel
            {
                Name = blobName,
                Uri = blob.Uri,
                SasToken = blob.GetSharedAccessSignature(blobPolilcy)
            };

        }
    }

    public class StorageTokenViewModel
    {
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public string SasToken { get; set; }
    }
}
