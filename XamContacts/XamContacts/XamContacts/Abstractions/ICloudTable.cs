using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamContacts.Helpers;
using XamContacts.Model;

namespace XamContacts.Abstractions
{
    public interface ICloudTable<T>
        where T : TableData
    {
        Task<T> SaveItemAsync(T item);
        Task<T> GetItemAsync(string id);
        Task DeleteItemAsync(T item);
        Task<ObservableCollection<T>> GetItemsAsync(bool syncItems = false);
        Task<ObservableCollection<Grouping<string, Contact>>> GetItemsGroupedAsync(bool syncItems = false);
    }
}
