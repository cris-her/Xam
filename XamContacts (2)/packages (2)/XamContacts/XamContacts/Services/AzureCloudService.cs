using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Xamarin.Forms;
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
            App.CurrentClient = client;
        }

        public async Task<ICloudTable<T>> GetTableAsync<T>() where T : TableData
        {
            await InitializeAsync();
            return new AzureCloudTable<T>(client);
        }

        public Task LoginAsync()
        {
            var loginProvider =
                DependencyService.Get<ILoginProvider>();
            return loginProvider.LoginAsync(client);
        }

        public bool IsUserLogged()
        {
            bool isUserLogged =
                client.CurrentUser != null;
            return isUserLogged;
        }

        public async Task<StorageTokenViewModel> GetSasTokenAsync(string fileName)
        {
            var parameters = new Dictionary<string,string>();
            parameters.Add("fileName", fileName);

            var storageToken =
                await client.InvokeApiAsync<StorageTokenViewModel>
                    ("GetStorageToken", HttpMethod.Get, parameters);
            return storageToken;
        }

        async Task InitializeAsync()
        {
            if (client.SyncContext.IsInitialized)
            {
                return;
            }

            var store =
                new MobileServiceSQLiteStore("offlinecache2.db");
            store.DefineTable<Contact>();

            await client.SyncContext.InitializeAsync(store);
        }

    }
}
