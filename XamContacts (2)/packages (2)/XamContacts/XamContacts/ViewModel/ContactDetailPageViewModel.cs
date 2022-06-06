using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Xamarin.Forms;
using XamContacts.Annotations;
//using XamContacts.Data;
using XamContacts.Model;
using XamContacts.Services;

namespace XamContacts.ViewModel
{
    public class ContactDetailPageViewModel : INotifyPropertyChanged
    {
        private ImageSource _profileImage;
        public Command SaveContactCommand { get; set; }
        public Command UploadFileCommand { get; set; }
        public Contact CurrentContact { get; set; }
        public INavigation Navigation { get; set; }

        public ImageSource ProfileImage
        {
            get { return _profileImage; }
            set
            {
                _profileImage = value; 
                OnPropertyChanged();
            }
        }

        public ContactDetailPageViewModel(INavigation navigation
            , Contact contact = null)
        {
            Navigation = navigation;
            if (contact == null)
            {
                CurrentContact = new Contact();
            }
            else
            {
                CurrentContact = contact;
                Task.Run(async () =>
                    await GetFile(CurrentContact.FileName)).Wait();

            }
            SaveContactCommand = new Command(async() => await SaveContact());
            UploadFileCommand = new Command(async() => 
            await AddNewFileAsync());
        }

        private async Task AddNewFileAsync()
        {
            try
            {
                var media = new MediaService();
                var mediaStream = await media.GetUploadFileAsync();
                if (mediaStream == null)
                {
                    return;
                }

                var storageToken =
                    await App.CloudService
                        .GetSasTokenAsync(mediaStream.FileName);
                var storageUri =
                    new Uri($"{storageToken.Uri}{storageToken.SasToken}");
                var blobStorage =
                    new CloudBlockBlob(storageUri);
                await blobStorage
                    .UploadFromStreamAsync(mediaStream.FileStram);
                CurrentContact.FileName =
                    mediaStream.FileName;
                SetImage(storageUri);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error subiendo el archivo",
                    ex.Message,
                    "OK");
            }
        }

        public async Task SaveContact()
        {
            //await App.Database.SaveItemAsync(CurrentContact);
            //await ContactsManager.DefaultManager.SaveItemAsync(CurrentContact);
            await App.CloudService
                .GetTableAsync<Contact>()
                .Result.SaveItemAsync(CurrentContact);
            await Navigation.PopToRootAsync();
        }

        private async Task GetFile(string fileName)
        {
            var storageToken =
                await App.CloudService
                    .GetSasTokenAsync(fileName);
            var storageUri =
                new Uri($"{storageToken.Uri}{storageToken.SasToken}");
            SetImage(storageUri);
        }

        private void SetImage(Uri url)
        {
            ProfileImage = ImageSource.FromUri(url);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
