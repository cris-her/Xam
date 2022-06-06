using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CognitiveXamDemo.CognitiveServices;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Vision;
using Plugin.Media;
using Xamarin.Forms;
using XamCognitiveServices.CognitiveServices;
using XamCognitiveServices.CognitiveServices.SpellCheck;
using XamCognitiveServices.CognitiveServices.Translation;

namespace XamCognitiveServices
{
    public partial class MainPage : ContentPage
    {
        private Stream fileStream;
        private VisionServiceClient visionClient;
        private FaceServiceClient faceClient;

        public MainPage()
        {
            InitializeComponent();
            visionClient =
                new VisionServiceClient
                (Constants.VisionApiKey, 
                Constants.VisionEndpoint);

            faceClient =
                new FaceServiceClient(
                    Constants.FaceApiKey,
                    Constants.FaceEndpoint);
        }

        private async void OpenImage(object sender, EventArgs e)
        {
            var mediaPlugin = CrossMedia.Current;
            await mediaPlugin.Initialize();
            if (mediaPlugin.IsPickPhotoSupported)
            {
                var mediaFile =
                    await mediaPlugin.PickPhotoAsync();
                if (mediaFile != null)
                {
                    fileStream = mediaFile.GetStream();
                }
            }
        }

        private async void AnalyzeImage(object sender, EventArgs e)
        {
            try
            {
                var result =
                    await visionClient.AnalyzeImageAsync(fileStream,
                        new List<VisualFeature>
                        {
                            VisualFeature.Adult,
                            VisualFeature.Categories,
                            VisualFeature.Description
                        });
            }
            catch (ClientException ex)
            {
                Debug.WriteLine(ex.Error.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void AnalyzeOCR(object sender, EventArgs e)
        {
            try
            {
                string text = string.Empty;
                var result = await visionClient
                    .RecognizeTextAsync(fileStream);
                foreach (var region in result.Regions)
                {
                    foreach (var line in region.Lines)
                    {
                        foreach (var word in line.Words)
                        {
                            text += word.Text + " ";
                        }
                    }
                }
                await DisplayAlert("Resultado", text, "OK");
            }
            catch (ClientException ex)
            {
                Debug.WriteLine(ex.Error.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void AnalyzeFace(object sender, EventArgs e)
        {
            StringBuilder builder =
                new StringBuilder();
            builder.Append("Características de la persona");
            try
            {
                var faces =
                    await faceClient.DetectAsync(
                        fileStream, true, false,
                        new List<FaceAttributeType>
                        {
                            FaceAttributeType.Age,
                            FaceAttributeType.Gender,
                            FaceAttributeType.Hair,
                            FaceAttributeType.FacialHair
                        });
                foreach (var face in faces)
                {
                    builder.Append(face.FaceAttributes.Age);
                    builder.AppendLine();
                    builder.Append(face.FaceAttributes.Gender);
                    builder.AppendLine();
                    foreach (var hairAttribute in face.FaceAttributes.Hair.HairColor)
                    {
                        if (hairAttribute.Confidence > .5)
                        {
                            builder.Append(hairAttribute.Color);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
              Debug.WriteLine(ex.Message);   
            }
        }

        private async void CheckTextErrors(object sender, EventArgs e)
        {
            var spellChecker = 
                new BingSpellCheckService();
            var resultado = await spellChecker.SpellCheckTextAsync(
                "esta es una demotración de coreccion de erores");
        }

        private async void TranslateText(object sender, EventArgs e)
        {
            var translator =
                new TextTranslationService(new AuthenticationService(Constants.TranslatorApiKey));
            var resultado =
                await translator.TranslateTextAsync("Esta es una demostración de los servicios de traduccción",
                    "es",
                    "en");
        }
    }
}
