using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fluxx
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectionPage : ContentPage
    {
        Parameter parameter;
        Socket socket;
        public ConnectionPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            socket = new ServerConnection().Socket();

            parameter = new Parameter();

        }
        static string Hash(string input)
        {
            var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
        }
        private void Connection_Clicked(object sender, EventArgs e)
        {
          
            Entry.IsEnabled = false;
            Entry.IsVisible = false;
            Loader.IsVisible = true;
            Loader.IsRunning = true;
            

            if (Pseudo.Text != null && Password.Text != null)
            {

                JObject jout = new JObject
                {
                    ["pseudo"] = Pseudo.Text,
                    ["password"] = Hash(Password.Text)
                };
                socket.Emit("loginPlayer", jout);
                socket.On("loginPlayerEcho", async data_result =>
                {
                    if (!data_result.ToString().StartsWith("Error: "))
                    {
                       
                        Player p = JsonConvert.DeserializeObject<Player>(data_result.ToString());
                        Application.Current.Properties["playerID"] = p.Id;
                        Application.Current.Properties["playerPseudo"] = p.Pseudo;
                        Application.Current.Properties["playerPassword"] = p.Password;
                        Application.Current.Properties["playerColor"] = p.Color;
                        Application.Current.Properties["playerWins"] = p.Wins;
                        Application.Current.Properties["playerLosses"] = p.Losses; 
                        await Application.Current.SavePropertiesAsync();
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            MainPage mainPage = new MainPage(p);
                            Application.Current.MainPage.Navigation.PopToRootAsync();
                            Application.Current.MainPage.Navigation.PushAsync(mainPage);
                        });
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            lbl_info.Text = data_result.ToString().Remove(0, 7); ;
                            Entry.IsEnabled = true;
                            Entry.IsVisible = true;
                            Loader.IsVisible = false;
                            Loader.IsRunning = false;
                        });
                    }
                });
            }
            else
            {
                lbl_info.Text = "Veuillez entrer du text dans les deux champs";
                Entry.IsEnabled = true;
                Entry.IsVisible = true;
                Loader.IsVisible = false;
                Loader.IsRunning = false;
            }
        }
    }
}