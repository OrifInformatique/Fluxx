using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;

namespace Fluxx
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{

		public SettingsPage()
		{
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            int value = (int) sliderColor.Value;
            string color;
            switch (value)
            {
                case 0: color = "#000000"; break;
                case 1: color = "#2A2A2A"; break;
                case 2: color = "#444444"; break;
                case 3: color = "#555555"; break;
                case 4: color = "#880000"; break;
                case 5: color = "#882A00"; break;
                case 6: color = "#885500"; break;
                case 7: color = "#888800"; break;
                case 8: color = "#558800"; break;
                case 9: color = "#2A8800"; break;
                case 10: color = "#008800"; break;
                case 11: color = "#00882A"; break;
                case 12: color = "#008855"; break;
                case 13: color = "#008888"; break;
                case 14: color = "#005588"; break;
                case 15: color = "#002A88"; break;
                case 16: color = "#000088"; break;
                case 17: color = "#2A0088"; break;
                case 18: color = "#550088"; break;
                case 19: color = "#880088"; break;
                case 20: color = "#880055"; break;
                case 21: color = "#88002A"; break;
                default: color = "#888888"; break;
            }
            labelColor.BackgroundColor = Color.FromHex(color);
            sliderColor.BackgroundColor = Color.FromHex(color);
        }
        private void OnReturn(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }
        Uri url = new Uri("http://192.168.4.136:8080/");
        private void OnConnect()
        {

        


            #region old
            /*
            var client = new HttpClient();
            var postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("room_name", "la room de Galax"),
                new KeyValuePair<string, string>("room_pass", "test"),
                new KeyValuePair<string, string>("player_id", "3"),
                new KeyValuePair<string, string>("room_num_player", "3")
            };

            HttpContent content = new FormUrlEncodedContent(postData);

            var response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                lbl_info.Text = await response.Content.ReadAsStringAsync();

            }

            
            mPost = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("room_name", "la room de Galax"),
                new KeyValuePair<string,string>("room_pass", "test"),
                new KeyValuePair<string,string>("player_id", "3"),
                new KeyValuePair<string,string>("room_num_player", "3")
            });

            mClient.DownloadDataAsync(mUrl);
            mClient.DownloadDataCompleted += mClient_DownloadDataCompleted;*/
            #endregion
        }

        private void BtnConnect_Clicked(object sender, EventArgs e)
        {
            OnConnect();
        }
    }
}