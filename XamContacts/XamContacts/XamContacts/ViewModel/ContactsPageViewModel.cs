using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Xamarin.Forms;
//using XamContacts.Data;
using XamContacts.Helpers;
using XamContacts.Model;
using XamContacts.View;

namespace XamContacts.ViewModel
{
    public class ContactsPageViewModel
    {
        public ObservableCollection<Grouping<string, Contact>> 
            ContactsList { get; set; }

        public Contact CurrentContact { get; set; }
        public Command AddContactCommand { get; set; }
        public Command ItemTappedCommand { get; }
        public INavigation Navigation { get; set; }

        public ContactsPageViewModel(INavigation navigation)
        {
            Navigation = navigation;

            var isConnected = CrossConnectivity.Current.IsConnected;

            //Task.Run(async () =>
            //    ContactsList = await ContactsManager.DefaultManager.GetItemsGroupedAsync(isConnected)).Wait();           

            Task.Run(async () =>
                ContactsList = await App.CloudService.GetTableAsync<Contact>()
                    .Result
                    .GetItemsGroupedAsync(isConnected)).Wait();

            AddContactCommand = new Command(async () =>await
                GoToContactDetailPage());
            ItemTappedCommand = new Command(async() =>await  
                GoToContactDetailPage(CurrentContact));
        }

        public async Task GoToContactDetailPage(Contact contact = null)
        {
            if (contact == null)
            {
                await Navigation.PushAsync(new ContactDetailPage());
            }
            else
            {
                await Navigation.PushAsync(new ContactDetailPage(CurrentContact));
            }
        }
    }
}
