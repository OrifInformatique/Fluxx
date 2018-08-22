using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Fluxx
{
    public partial class MainPage : ContentPage
	{
        Player _player;
        Socket socket;

        public MainPage()
		{
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            socket = new ServerConnection().Socket();
        }
      
        protected override void OnAppearing()
        {
            //base.OnAppearing();
            CheckConnection();
        }
        public void CheckConnection()
        {
            btn_create_game.IsEnabled = false;
            btn_join_game.IsEnabled = false;
            btn_settings.IsEnabled = false;
            if(Application.Current.Properties.ContainsKey("playerID"))
            {
                _player = new Player
                {
                    Id = (int)Application.Current.Properties["playerID"],
                    Pseudo = (string)Application.Current.Properties["playerPseudo"],
                    Password = (string)Application.Current.Properties["playerPassword"],
                    Color = (string)Application.Current.Properties["playerColor"],
                    Wins = (int)Application.Current.Properties["playerWins"],
                    Losses = (int)Application.Current.Properties["playerLosses"]
                };
                
                JObject jout = (JObject)JToken.FromObject(_player);
                socket.Emit("connection player", jout);
                lbl_pseudo.Text = "Bienvenue, "+_player.Pseudo;

                btn_create_game.IsEnabled = true;
                btn_join_game.IsEnabled = true;
                btn_settings.IsEnabled = true;
            }
            else lbl_pseudo.Text = "Vous n'êtes pas connecté !";
            
        }

        private void OnCreateGame(object sender, EventArgs e)
        {
            if (_player != null)
            {
                SetGamePage setGamePage = new SetGamePage(_player);
                Application.Current.MainPage.Navigation.PushAsync(setGamePage);
            }
        }
        private void OnJoinGame(object sender, EventArgs e)
        {
            if (_player != null)
            {
                JoinGamePage joinGamePage = new JoinGamePage(_player);
                Application.Current.MainPage.Navigation.PushAsync(joinGamePage);
            }
        }
        private void OnOpenSettings(object sender, EventArgs e)
        {
            SettingsPage settingsPage = new SettingsPage();
            Application.Current.MainPage.Navigation.PushAsync(settingsPage);
    
        }
        private void Connection_Clicked(object sender, EventArgs e)
        {
            ConnectionPage connectionPage = new ConnectionPage();
            Application.Current.MainPage.Navigation.PushAsync(connectionPage);
            
        }
        private void Test_Clicked(object sender, EventArgs e)
        {
            GamePage gamePage = new GamePage();
            Application.Current.MainPage.Navigation.PushAsync(gamePage);
        }

       
    }
}
