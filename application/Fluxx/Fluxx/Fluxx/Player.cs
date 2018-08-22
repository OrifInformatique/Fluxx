using System;
using System.Collections.Generic;
using System.Text;

namespace Fluxx
{
    public class Player
    {
        public int Id { get; set; }
        public string Pseudo { get; set; }
        public string Password { get; set; }
        public string Color { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
    }
}
