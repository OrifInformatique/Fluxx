using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fluxx
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerView : ContentView
    {
        Player _player;
        public PlayerView(Player player)
        {
            InitializeComponent();
            
            ChargePlayer(player);
        }
        public void ChargePlayer(Player player)
        {
            _player = player;
            if (_player == null)
            {
                PlayerName.IsVisible = false;
                PlayerId.IsVisible = false;
                PlayerWins.IsVisible = false;
                PlayerLosses.IsVisible = false;
                content.BackgroundColor = Color.Black;
            }
            else
            {
                PlayerName.IsVisible = true;
                PlayerId.IsVisible = true;
                PlayerWins.IsVisible = true;
                PlayerLosses.IsVisible = true;
                content.BackgroundColor = Color.FromHex("#22000000");
                PlayerName.Text = _player.Pseudo;
                PlayerName.TextColor = Color.FromHex("#"+_player.Color);
                PlayerId.Text += _player.Id.ToString();
                PlayerWins.Text += _player.Wins.ToString();
                PlayerLosses.Text += _player.Losses.ToString();
            }

        }
        public string UpdatePlayer(Player new_player)
        {
            string msg = string.Empty;
            //if (player == null && _player == null)pas de joueurs
            if (new_player == null && _player != null)
                msg =  _player.Pseudo + " est partit.";
            else if (new_player != null && _player == null)
                msg = new_player.Pseudo + " vient d'arriver.";
            else if (new_player != null && _player != null)
                if (new_player.Pseudo != _player.Pseudo || new_player.Password != _player.Password || new_player.Id != _player.Id)
                    msg = "Error: le joueur ne correspond pas";
            if (msg != string.Empty)
                ChargePlayer(new_player);
            return msg;
        }
        public bool SameID(int id)
        {
            return _player.Id == id;
        }
	}
}