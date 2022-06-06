using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using XamContacts.Abstractions;
using XamContacts.Helpers;
using XamContacts.Model;

namespace XamContacts.Services
{
    public class AzureCloudTable<T> : ICloudTable<T> where T : TableData
    {
        private MobileServiceClient client;
        private IMobileServiceSyncTable<T> table;

        public AzureCloudTable(MobileServiceClient client)
        {
            this.client = client;
            table = client.GetSyncTable<T>();
        }

        public async Task<T> SaveItemAsync(T item)
        {
            try
            {
                if (item.Id != null)
                {
                    await table.UpdateAsync(item);
                }
                else
                {
                    try
                    {
                        await table.InsertAsync(item);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
            return item;
        }

        public async Task<T> GetItemAsync(string id)
        {
            var items = await table
               .Where(i => i.Id == id)
               .ToListAsync();
            return items.FirstOrDefault();
        }

        public async Task DeleteItemAsync(T item)
        {
            await table.DeleteAsync(item);
        }

        public async Task<ObservableCollection<T>> GetItemsAsync(bool syncItems = false)
        {
            try
            {
                if (syncItems)
                {
                    await SyncAsync();
                }
                IEnumerable<T> items
                    = await table.ToEnumerableAsync();
                return new ObservableCollection<T>(items);
            }
            catch (MobileServiceInvalidOperationException mobileException)
            {
                Debug.WriteLine($"Excepción: {mobileException.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción: {ex.Message}");
            }
            return null;
        }

        public async Task<ObservableCollection<Grouping<string, Contact>>> GetItemsGroupedAsync(bool syncItems = false)
        {
            try
            {
                if (syncItems)
                {
                    await SyncAsync();
                }
                IEnumerable<Contact> contacts =
                    (IEnumerable<Contact>) await GetItemsAsync();
                IEnumerable<Grouping<string, Contact>> sorted =
                    new Grouping<string, Contact>[0];
                if (contacts != null)
                {
                    sorted =
                        from c in contacts
                        orderby c.Name
                        group c by c.Name[0].ToString()
                        into theGroup
                        select new Grouping<string, Contact>
                            (theGroup.Key, theGroup);
                }
                return new ObservableCollection<Grouping<string, Contact>>(sorted);
            }
            catch (MobileServiceInvalidOperationException mobileException)
            {
                Debug.WriteLine($"Excepción: {mobileException.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción: {ex.Message}");
            }
            return null;
        }

        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError>
                syncErrors = null;
            try
            {
                await this.client.SyncContext.PushAsync();
                string queryName = $"incsync_{typeof(T).Name}";
                await this.table.PullAsync(queryName,
                    this.table.CreateQuery());
            }
            catch (MobileServicePushFailedException ex)
            {
                if (ex.PushResult != null)
                {
                    syncErrors = ex.PushResult.Errors;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message}");
            }
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update)
                    {
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        await error.CancelAndDiscardItemAsync();
                    }
                }
            }
        }
    }
}
