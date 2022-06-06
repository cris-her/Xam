using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;
using XamContacts.Services;
using XamContacts.UWP.Services;

[assembly:Dependency(typeof(FileHelper))]
namespace XamContacts.UWP.Services
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string fileName)
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path,
                fileName);
        }
    }
}
