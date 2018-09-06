using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fluxx;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Fluxx
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RoomView : ContentView
	{
        Room _room;
        Player _player;
        Socket socket;
        public RoomView(Room room, Player player)
        {
			InitializeComponent();
            _room = room;
            _player = player;
            socket = new ServerConnection().Socket();
            ChargeRoom();
           
		}
        private void ChargeRoom()
        {
            RoomName.Text = _room.Name;
            RoomNumPlayer.Text = GetNumberOfPlayer(_room) +"/"+_room.NumPlayer.ToString();
            if (String.IsNullOrEmpty(_room.Password)) password.IsVisible = false;
            
        }
        private int GetNumberOfPlayer(Room room)
        {
            int i = 1;
            foreach (object p in room.Players)
                if (p != null)
                    i++;
            return i;
        }
        private void JoinButton_Clicked(object sender, EventArgs e)
        {

            if (_room.Password == null) _room.Password = "";
            if(password.Text == _room.Password)
            {

                JObject players = JObject.Parse(JsonConvert.SerializeObject(_room.Players));
                    
                
                string jout = "{ RoomName: '" + _room.Name + ", RoomOpen: " + _room.Name + ", RoomPLayers: [" + players + "], PlayerId: " + _player.Id + ", PlayerPseudo: " + _player.Pseudo + ", PlayerPassword: " + _player.Password + ", PlayerWins: " + _player.Wins + ", PlayerLosses: " + _player.Losses + ", PlayerColor: " + _player.Color + "}";
                socket.Emit("joinRoom", jout);

                socket.On("joinRoomEcho", data_result =>
                {
                    if ( (string)data_result == "true")
                    {
                        CreateGamePage createGamePage = new CreateGamePage(_room, _player);
                        Application.Current.MainPage.Navigation.PushAsync(createGamePage);
                    }
                    else
                    {
                        lbl_info.Text = (string)data_result;
                    }
                });
            }
            else
            {
                lbl_info.Text = "Error: Ce mot de passe est faux !";
            }
        }
    }
}