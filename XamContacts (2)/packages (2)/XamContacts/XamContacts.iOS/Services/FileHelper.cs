using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;
using XamContacts.iOS.Services;
using XamContacts.Services;

[assembly:Dependency(typeof(FileHelper))]
namespace XamContacts.iOS.Services
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string fileName)
        {
            string docFolder =
                Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");
            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }
            return Path.Combine(libFolder, fileName);
        }
    }
}
