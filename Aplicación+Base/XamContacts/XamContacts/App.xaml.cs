using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using XamContacts.Data;
using XamContacts.Services;
using XamContacts.View;

namespace XamContacts
{
    public partial class App : Application
    {
        private static ContactsDatabase database;

        public static ContactsDatabase Database
        {
            get
            {
                if (database == null)
                {
                    try
                    {
                        database =
                            new ContactsDatabase(DependencyService
                                .Get<IFileHelper>()
                                .GetLocalFilePath("contactsdb.db3"));
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new ContactsPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
