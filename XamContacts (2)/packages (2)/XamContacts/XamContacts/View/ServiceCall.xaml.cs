using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamContacts.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceCall : ContentPage
    {
        public ServiceCall()
        {
            InitializeComponent();
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            var servicio = await CallService();
            lblResult.Text = servicio.Result.ToString();

        }

        private async Task<ResultViewModel> CallService()
        {
            Dictionary<string, string> parameters =
                new Dictionary<string, string>();

            parameters.Add("first", "10");
            parameters.Add("second", "20");

            return await App.CurrentClient
                .InvokeApiAsync<ResultViewModel>("addition", 
                HttpMethod.Get, parameters);
        }
    }

    public class ResultViewModel
    {
        public int First { get; set; }
        public int Second { get; set; }
        public int Result { get; set; }
    }
}