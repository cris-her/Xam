using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using XamContacts.Services;
using XamContacts.UWP.Services;

[assembly:Dependency(typeof(LoginProvider))]
namespace XamContacts.UWP.Services
{
    public class LoginProvider : ILoginProvider
    {
        public async Task LoginAsync(MobileServiceClient client)
        {
            var user =
                await 
                client.LoginAsync(
                    MobileServiceAuthenticationProvider.Facebook, 
                    "democsbackend");
        }
    }
}
