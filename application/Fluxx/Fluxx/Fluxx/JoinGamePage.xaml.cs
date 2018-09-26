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

            socket.On("joinRoomEcho", data_result =>
            {
                if (!data_result.ToString().StartsWith("Error: "))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        socket.Off("joinRoomEcho");
                        socket.Off("newRoomEcho");
                        Room room = JsonConvert.DeserializeObject<Room>(data_result.ToString());
                        CreateGamePage createGamePage = new CreateGamePage(room, _player);
                        Application.Current.MainPage.Navigation.PushAsync(createGamePage);
                    });
                }
                else
                {
                    Chargement(false);
                    lbl_info.Text = (string)data_result;
                }
            });
        }

        private void ChargeAllRooms()
        {
            socket.Emit("getAllRoom");
            socket.On("getAllRoomEcho", data_result => {
                socket.Off("getAllRoomEcho");
                Device.BeginInvokeOnMainThread(() =>
                {
                    rooms = JsonConvert.DeserializeObject<Room[]>(data_result.ToString());
                    ChargeViews(rooms);
                });
            });
            socket.On("newRoomEcho", data_result =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ChargeViews(new Room[] {JsonConvert.DeserializeObject<Room>(data_result.ToString())});
                });
            });

        }
        private void Chargement(bool isCharge)
        {
            indicator.IsVisible = isCharge;
            indicator.IsRunning = isCharge;
            RoomsEmplacement.IsVisible = !isCharge;
        }
        private void ChargeRoom(object sender, EventArgs e)
        {
            Chargement(true);
        }
        private void ChargeViews(Room[] rooms)
        {
           
            if (rooms.Count() == 0)
                lbl_info.Text = "il n'y as aucune room pour le moment !";
            else
                foreach (Room room in rooms)
                {
                    Button lunchGameButton = new Button
                    {
                        Text = "Joindre",
                        BackgroundColor = Color.Transparent,
                        Margin = 0,
                        HorizontalOptions = LayoutOptions.End
                    };
                    lunchGameButton.Clicked += ChargeRoom;

                    RoomView newRoom = new RoomView(room, _player, lunchGameButton);
                    RoomsEmplacement.Children.Add(newRoom);
                }
        }
    }
}