using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamContacts.Model;

namespace XamContacts.Abstractions
{
    public interface ICloudService
    {
        Task<ICloudTable<T>> GetTableAsync<T>()
            where T : TableData;

        Task LoginAsync();
        bool IsUserLogged();
        Task<StorageTokenViewModel> GetSasTokenAsync(string fileName);
    }
}
