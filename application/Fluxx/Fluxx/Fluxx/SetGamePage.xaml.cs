using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fluxx
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SetGamePage : ContentPage
	{
        Player _player;
        Room _room;
        Socket socket;
        public SetGamePage (Player player)
		{
			InitializeComponent();
            socket = new ServerConnection().Socket();

            _player = player;
         
        }
        private void LunchCreateGame(object sender, EventArgs e)
        {

            if (RoomNbrPlayer.SelectedItem != null)
            {
                if (GameName.Text != string.Empty)
                {
                    GameName.IsEnabled = false;
                    RoomPassword.IsEnabled = false;
                    RoomNbrPlayer.IsEnabled = false;
                    btnCreateGame.IsEnabled = false;
                    CreateGame();
                }
            }
         
        }
        private void CreateGame()
        {
            int num_player = Int32.Parse(RoomNbrPlayer.SelectedItem.ToString());
            _room = new Room
            {
                Name = GameName.Text,
                Password = RoomPassword.Text,
                Players = new Player[num_player]
            };
            _room.Players[0] = _player;
            
            JObject jout = JObject.Parse(JsonConvert.SerializeObject(_room));

            socket.Emit("createRoom", jout);

            socket.On("createRoomEcho", data_result =>
            {
                if ((bool)data_result)
                {
                    socket.Off("createRoomEcho");
                    Device.BeginInvokeOnMainThread(() =>
                    {     
                        CreateGamePage createGamePage = new CreateGamePage(_room, _player);
                        Application.Current.MainPage.Navigation.PopToRootAsync();
                        Application.Current.MainPage.Navigation.PushAsync(createGamePage);
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        lbl_info.Text = "une room porte déja ce nom";
                        GameName.IsEnabled = true;
                        RoomPassword.IsEnabled = true;
                        RoomNbrPlayer.IsEnabled = true;
                        btnCreateGame.IsEnabled = true;
                    });
                }
            });
        }

        private void GameName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lbl_info.Text != String.Empty)
            {
                lbl_info.Text = String.Empty;
            }
        }
    }
}