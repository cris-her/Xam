using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using XamContacts.Droid.Services;
using XamContacts.Services;

[assembly:Dependency(typeof(LoginProvider))]
namespace XamContacts.Droid.Services
{
    public class LoginProvider : ILoginProvider
    {
        private Context context;

        public void Init(Context context)
        {
            this.context = context;
        }
        public async Task LoginAsync(MobileServiceClient client)
        {
            var user =
                await client.LoginAsync(context,
                    MobileServiceAuthenticationProvider.Facebook,
                    "democsbackend");
        }
    }
}