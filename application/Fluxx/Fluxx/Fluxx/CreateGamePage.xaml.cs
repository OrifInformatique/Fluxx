using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fluxx
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateGamePage : ContentPage
	{
        Room _room;
        Player _player;
        PlayerView[] _playersView;
        Socket socket;

        public CreateGamePage(Room room, Player player)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            socket = new ServerConnection().Socket();
            _room = room;
            _player = player;
            _playersView = new PlayerView[room.Players.Count()];
            InitialiseRoom();
            ChatBox chat = new ChatBox(_player, _room);
            Chat.Children.Add(chat);

            if (_room.Players[0] == _player)//if my == host
            {
                socket.On("hostCheck", newRoom =>
                {
                    _room = JsonConvert.DeserializeObject<Room>(newRoom.ToString());
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UpdateRoom();
                        socket.Emit("updateRoom", newRoom);
                    });
                });
                socket.On("playerLeaveRoom", player_id =>
                {
                    for (int i = 0; i < _playersView.Length; i++)
                    {
                        if (_playersView[i].SameID((int)player_id))
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                _room.Players[i] = null;
                                UpdateRoom();
                                socket.Emit("updateRoom", JObject.Parse(JsonConvert.SerializeObject(_room)));
                            });
                        }
                    }
                });
            }
            else //my == invite
            {
                LunchGame.IsVisible = false;
                socket.On("lunchGame", final_room =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Room finalroom = JsonConvert.DeserializeObject<Room>(final_room.ToString());
                        GamePage gamePage = new GamePage(finalroom, _player);
                        Application.Current.MainPage.Navigation.PopToRootAsync();
                        Application.Current.MainPage.Navigation.PushAsync(gamePage);
                    });
                });
                socket.On("updateRoom", newRoom =>
                {
                    _room = JsonConvert.DeserializeObject<Room>(newRoom.ToString());
                    UpdateRoom();
                });
            }
		}

        protected override bool OnBackButtonPressed()
        {
            int i = Array.IndexOf(_room.Players, _player);
            if (i != -1) _room.Players[i] = null;
            socket.Emit("leaveRoom", JObject.Parse(JsonConvert.SerializeObject(_room)));
            return base.OnBackButtonPressed();
        }

        private void LuncheGame(object sender, EventArgs e)
        {
            if (GetNumPlayerPresent() == _room.Players.Count())
            {
                socket.Off("hostCheck");
                socket.Off("updateRoom");
                socket.Off("playerLeaveRoom");
                socket.Emit("lunchGame", JObject.Parse(JsonConvert.SerializeObject(_room)));
                GamePage gamePage = new GamePage(_room, _player);
                Application.Current.MainPage.Navigation.PopToRootAsync();
                Application.Current.MainPage.Navigation.PushAsync(gamePage);
            }
            else
            {
                DisplayMessage("Il reste encore des places libre");
            }
        }
        

        private int GetNumPlayerPresent()
        {
            int i = 0;
            foreach (var player in _room.Players) if (player != null) i++;
            return i;
        }
        private void InitialiseRoom()
        {
            RoomName.Text = _room.Name;
            RoomNumPlayer.Text = 1 + "/" + _room.Players.Count();

            
            for (int i = 0; i < _room.Players.Length; i++)
            {
                _playersView[i] = new PlayerView(_room.Players[i]);
                PlayersEmplacement.Children.Add(_playersView[i]);
            }
        }
        private void UpdateRoom()
        {
            for (int p = 0; p < _playersView.Length; p++)
            {
                DisplayMessage(_playersView[p].UpdatePlayer(_room.Players[p]));
            }
            RoomNumPlayer.Text = GetNumPlayerPresent() + "/" + _room.Players.Count();
        }
        async void DisplayMessage(string message)
        {
            if (message != string.Empty)
            {
                lbl_info.Text = message;
                await Task.Delay(3000);
                lbl_info.Text = string.Empty;
            }
        }
    }
}