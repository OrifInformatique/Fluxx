using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Fluxx
{
    public class Card
    {
        public enum TypeOfCard { Action , Keeper, NewRule, Goal, Error}
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Type { private get; set; }
        public string Image { private get; set; }
        public string Description { get; set; }
        public string Keepers1 { get; set; }
        public string Keepers2 { get; set; }

        public new TypeOfCard GetType() => (TypeOfCard)Enum.Parse(typeof(TypeOfCard), Type, true);//return enum via string 
        public ImageSource GetImage() => ImageSource.FromFile(Image + ".png");
    }

}
