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
	public partial class 
        Page : ContentPage
	{
        Room _room;
        Player _player;
        Socket socket;
		public void CreateGamePage (Room room, Player player)
		{
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            socket = new ServerConnection().Socket();
            _room = room;
            _player = player;
            ChargeRoom();
            ChargePlayer();
		}


        protected override bool OnBackButtonPressed()
        {
            if(_room.Host == _player)
            {
                _room.Host = null;
            }
            else
            {
                int i = Array.IndexOf(_room.Players, _player);
                if (i != -1) _room.Players[i] = null;
            }

            JObject jout = JObject.Parse(JsonConvert.SerializeObject(_room));
            socket.Emit("leaveRoom", jout);
            return base.OnBackButtonPressed();

        }

        private void LuncheGame(object sender, EventArgs e)
        {

            
        }
        
       
        private void ChargeRoom()
        {
            RoomName.Text = _room.Name;
            RoomNumPlayer.Text = GetNumPlayerPresent() + "/" + _room.NumPlayer;
        }

        private int GetNumPlayerPresent()
        {
            int i = 1; //1 = host
            foreach (var player in _room.Players) if (player != null) i++;
            return i;
        }

        private void ChargePlayer()
        {
            /*var client = new HttpClient();
            var postData = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
             
            });
            var response = await client.PostAsync(url, postData);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                _room.Players =  JsonConvert.DeserializeObject<Player[]>(result);
            }
            foreach (Player player in _room.Players)
            {
                PlayerView newRoom = new PlayerView(player);

                PlayersEmplacement.Children.Add(newRoom);

            }*/
        }
    }
}