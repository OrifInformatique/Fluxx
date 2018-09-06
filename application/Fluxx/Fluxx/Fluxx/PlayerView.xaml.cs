using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fluxx
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PlayerView : ContentView
	{
        Player player;
		public PlayerView ()
		{
			InitializeComponent ();
            ChargePlayer(player);
            
		}
        private void ChargePlayer(Player _player)
        {
            if(_player == null)
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
                PlayerName.Text = _player.Pseudo;
                PlayerId.Text += _player.Id.ToString();
                PlayerWins.Text += _player.Wins.ToString();
                PlayerLosses.Text += _player.Losses.ToString();
            }
             
        }
	}
}