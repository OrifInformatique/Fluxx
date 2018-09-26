using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Fluxx
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChatBox : ContentView
	{
        Socket socket;
        Player _player;
        Room _room; 
        public ChatBox (Player player, Room room)
		{
			InitializeComponent();
        
            socket = new ServerConnection().Socket(); 
            _player = player;
            _room = room;
            socket.On("ReceiveMessage", result =>
            {
                Message msg = JsonConvert.DeserializeObject<Message>(result.ToString());
                Device.BeginInvokeOnMainThread(() =>
                {
                    NewMessage(msg.GetMessage());
                    ScrollView.ScrollToAsync(0, MessageEmplacement.Height, true);
                });
            });
        }

        private void NewMessage(Label newMessage)
        {
            LastMessage = newMessage;
            MessageEmplacement.Children.Add(newMessage);
           /* while (MessageEmplacement.Children.Count() > 20)
            {
                MessageEmplacement.Children.RemoveAt(0);
            }*/
        }

        private void BtnMinimize_Clicked(object sender, EventArgs e)
        {
            MaximizeChat.IsVisible = false;
            MinimizeChat.IsVisible = true;
           // Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Load(typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("Audio." + "send_msg.wav"));
        }

        private void BtnMaximize_Clicked(object sender, EventArgs e)
        {
            MinimizeChat.IsVisible = false;
            MaximizeChat.IsVisible = true;
           // Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Load(typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("Audio." + "send_msg.wav"));
        }

        private void BtnSendMessage_Clicked(object sender, EventArgs e)
        {
            if (EntryMessage.Text != string.Empty)
            {
                Message msg = new Message
                {
                    PseudoPlayer = _player.Pseudo,
                    ColorPlayer = _player.Color,
                    MessageText = EntryMessage.Text,
                    RoomName = _room.Name
                    
                };
                socket.Emit("SendMessage", JObject.Parse(JsonConvert.SerializeObject(msg)));
            }
            EntryMessage.Text = String.Empty;
        }
    }
}