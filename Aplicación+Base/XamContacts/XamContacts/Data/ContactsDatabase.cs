using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using XamContacts.Helpers;
using XamContacts.Model;

namespace XamContacts.Data
{
    public class ContactsDatabase
    {
        private readonly SQLiteAsyncConnection database;

        public ContactsDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Contact>().Wait();
        }

        public async Task<List<Contact>> GetItemsAsync()
        {
            var data = await database.Table<Contact>()
                .ToListAsync();
            return data;
        }

        public async Task<ObservableCollection<Grouping<string, Contact>>>
            GetItemsGroupedAsync()
        {
            IList<Contact> contacts =
                await App.Database.GetItemsAsync();
            IEnumerable<Grouping<string,Contact>> sorted =
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

        public Task<Contact> GetItemAsync(int id)
        {
            return database.Table<Contact>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Contact item)
        {
            if (item.Id != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                var result = database.InsertAsync(item);
                return result;
            }
        }

        public Task<int> DeleteItemAsync(Contact item)
        {
            return database.DeleteAsync(item);
        }
    }
}
