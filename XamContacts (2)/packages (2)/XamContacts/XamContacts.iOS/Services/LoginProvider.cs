using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using Microsoft.WindowsAzure.MobileServices;
using UIKit;
using Xamarin.Forms;
using XamContacts.iOS.Services;
using XamContacts.Services;

[assembly:Dependency(typeof(LoginProvider))]
namespace XamContacts.iOS.Services
{
    public class LoginProvider : ILoginProvider
    {
        public UIViewController RootView =>
            UIApplication.SharedApplication.KeyWindow
                .RootViewController;
        public async Task LoginAsync(MobileServiceClient client)
        {
            var user =
                await client.LoginAsync(RootView
                    , MobileServiceAuthenticationProvider.Facebook,
                    "democsbackend");
        }
    }
}