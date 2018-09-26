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
        public RoomView(Room room, Player player, Button lunchGameButton)
        {
			InitializeComponent();
            _room = room;
            _player = player;
            lunchGameButton.Clicked += JoinButton_Clicked;
            secondLine.Children.Add(lunchGameButton);
            socket = new ServerConnection().Socket();
            ChargeRoom();
           
		}
        private void ChargeRoom()
        {
            RoomName.Text = _room.Name;
            RoomNumPlayer.Text = GetNumberOfPlayer(_room) +"/"+_room.Players.Count().ToString();
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
            if(_room.Password == null||password.Text == _room.Password)
            {
                string jroom = JsonConvert.SerializeObject(_room);
                string jplayer = JsonConvert.SerializeObject(_player);
                JObject jout = JObject.Parse("{\"Room\":"+jroom+" , \"Player\": "+jplayer+"}");
                socket.Emit("joinRoom", jout);
            }
            else
            {
                lbl_info.Text = "Error: Ce mot de passe est faux !";
            }
        }
    }
}