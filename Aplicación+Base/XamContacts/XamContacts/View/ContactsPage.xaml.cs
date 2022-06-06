using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamContacts.ViewModel;

namespace XamContacts.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactsPage : ContentPage
    {
        public ContactsPage()
        {
            InitializeComponent();  
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.BindingContext = new ContactsPageViewModel(Navigation);
        }
    }
}