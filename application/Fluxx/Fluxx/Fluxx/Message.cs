using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Fluxx
{
    class Message
    {
        public string PseudoPlayer { get; set; }
        public string ColorPlayer { get; set; }
        public string MessageText { get; set; }
        public string RoomName { get; set; }

        public Label GetMessage()
        {
            FormattedString fs = new FormattedString();
            fs.Spans.Add(new Span
            {
                Text = PseudoPlayer,
                TextColor = Color.FromHex("#" + ColorPlayer)
            });
            fs.Spans.Add(new Span
            {
                Text = ": " + MessageText
            });
            return new Label { FormattedText = fs };
        }
    }
}
