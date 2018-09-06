using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Fluxx
{
    public partial class App : Application
    {
        Socket socket;
        public App()
        {
            InitializeComponent();
            Player p = null;
            if (Current.Properties.ContainsKey("playerID"))
            {
                socket = new ServerConnection().Socket();
                p = new Player
                {
                    Id = (int)Current.Properties["playerID"],
                    Pseudo = (string)Current.Properties["playerPseudo"],
                    Password = (string)Current.Properties["playerPassword"],
                    Color = (string)Current.Properties["playerColor"],
                    Wins = (int)Current.Properties["playerWins"],
                    Losses = (int)Current.Properties["playerLosses"]
                };
                socket.Emit("playerConnection", (JObject)JToken.FromObject(p));
                socket.On("playerConnection", result => {if (!(bool)result) p = null;});
            }
            MainPage = new NavigationPage(new MainPage(p));
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
