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
		public PlayerView (Player player)
		{
			InitializeComponent ();
            ChargePlayer(player);
		}
        private void ChargePlayer(Player player)
        {
            PlayerName.Text = player.Pseudo;
            PlayerId.Text += player.Id.ToString();
            PlayerWins.Text += player.Wins.ToString();
            PlayerLosses.Text += player.Losses.ToString();
        }
	}
}