using Newtonsoft.Json;
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
	public partial class JoinGamePage : ContentPage
	{
        Player _player;
        Socket socket;
        Room[] rooms;

        public JoinGamePage (Player player)
		{
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            socket = new ServerConnection().Socket();

            _player = player;
            ChargeAllRooms();
        }

        private void ChargeAllRooms()
        {

            socket.Emit("getAllRoom");
            socket.On("getAllRoomEcho", data_result => {
                rooms = JsonConvert.DeserializeObject<Room[]>(data_result.ToString());
                ChargeViews(rooms);
            });
            socket.On("newRoomEcho", data_result =>
            {
                Room[] room = { JsonConvert.DeserializeObject<Room>(data_result.ToString()) };
                ChargeViews(room);
            });

        }
        private  void ChargeViews(Room[] rooms)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (rooms.Count() == 0)
                    lbl_info.Text = "il n'y as aucune room pour le moment !";

                else
                    foreach (Room room in rooms)
                    {
                        RoomView newRoom = new RoomView(room, _player);
                        RoomsEmplacement.Children.Add(newRoom);
                    }
            });
        }
    }
}