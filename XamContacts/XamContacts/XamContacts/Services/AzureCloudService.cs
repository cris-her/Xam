using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using XamContacts.Abstractions;
using XamContacts.Model;

namespace XamContacts.Services
{
    public class AzureCloudService : ICloudService
    {
        private MobileServiceClient client;
        public AzureCloudService()
        {            
            client = 
                new MobileServiceClient("https://democsbackend.azurewebsites.net");
        }

        public async Task<ICloudTable<T>> GetTableAsync<T>() where T : TableData
        {
            await InitializeAsync();
            return new AzureCloudTable<T>(client);
        }

        async Task InitializeAsync()
        {
            if (client.SyncContext.IsInitialized)
            {
                return;
            }

            var store =
                new MobileServiceSQLiteStore("offlinecache.db");
            store.DefineTable<Contact>();

            await client.SyncContext.InitializeAsync(store);
        }

    }
}
