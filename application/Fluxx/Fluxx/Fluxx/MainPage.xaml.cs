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

        public MainPage(Player p)
		{
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            if (p != null)
            {
                _player = p;
                ChargeConnected(true);
            }
            else
            {
                ChargeConnected(false);
            }
            socket = new ServerConnection().Socket();
        }
      
        public void ChargeConnected(bool connected)
        {
            if (connected)
            {
                btn_create_game.IsEnabled = true;
                btn_join_game.IsEnabled = true;
                btn_settings.IsEnabled = true;
                lbl_pseudo.Text = "Bienvenue, " + _player.Pseudo;
            }
            else
            {

                btn_create_game.IsEnabled = false;
                btn_join_game.IsEnabled = false;
                btn_settings.IsEnabled = false;
                lbl_pseudo.Text = "Vous n'êtes pas connecté !";
            }
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
            if (_player != null)
            {
                SettingsPage settingsPage = new SettingsPage();
                Application.Current.MainPage.Navigation.PushAsync(settingsPage);
            }
        }
        private void Connection_Clicked(object sender, EventArgs e)
        {
            ConnectionPage connectionPage = new ConnectionPage();
            Application.Current.MainPage.Navigation.PushAsync(connectionPage);
        }
        private void Test_Clicked(object sender, EventArgs e)
        {
            if (_player != null)
            {
                Room _room = null;
                GamePage setGamePage = new GamePage(_room, _player);
                Application.Current.MainPage.Navigation.PushAsync(setGamePage);
            }
        }      
    }
}
