using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Edm.Validation;
using Plugin.Media;
using XamContacts.Model;

namespace XamContacts.Services
{
    public class MediaService
    {
        public async Task<FileStreamModel> GetUploadFileAsync()
        {
            var mediaPlugin = CrossMedia.Current;
            var mainPage = Xamarin.Forms.Application.Current.MainPage;

            await mediaPlugin.Initialize();
            if (mediaPlugin.IsPickPhotoSupported)
            {
                var mediaFile = await mediaPlugin.PickPhotoAsync();
                var file = new FileStreamModel
                {
                    FileStram = mediaFile.GetStream(),
                    FileName = System.IO.Path.GetFileName(mediaFile.Path)
                };
                return file;
            }
            else
            {
                await mainPage.DisplayAlert("El servicio de medios no está disponible",
                    "No se puede obtener una foto",
                    "OK");
                return null;
            }
        }
    }
}
