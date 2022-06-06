using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamContacts.Services;

namespace XamContacts.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StorageDemoView : ContentPage
    {
        public StorageDemoView()
        {
            InitializeComponent();
        }

        private async void CrearContenedor(object sender, EventArgs e)
        {
            StorageHelper helper = new StorageHelper();
            await helper.CreateContainer("demostorage");
        }

        private async void CreateFile(object sender, EventArgs e)
        {
            StorageHelper helper = new  StorageHelper();
            await helper.CreateTextBlob("demostorage", "demo01.txt", "Esta es una demo de Storage");
        }

        private async void ReadFile(object sender, EventArgs e)
        {
            StorageHelper helper = new StorageHelper();
            var resultado = await 
                helper.DownloadTextBlob("demostorage", "demo01.txt");
            Debug.WriteLine(resultado);
        }

        private async void ListFiles(object sender, EventArgs e)
        {
            StorageHelper helper = new StorageHelper();
            await helper.ListBlobs("demostorage");
        }


        private async void ChangePermissions(object sender, EventArgs e)
        {
            StorageHelper helper = new StorageHelper();
            await helper.ChangeContainerPermission("demostorage",
                BlobContainerPublicAccessType.Container);
        }

        private async void DeleteContainer(object sender, EventArgs e)
        {
            StorageHelper helper = new StorageHelper();
            await helper.DeleteContainer("demostorage");
        }
    }
}