using System;
using System.Collections.Generic;
using System.Text;

namespace Fluxx
{
    public class Room
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public int NumPlayer { get; set; }
        public Boolean Open { get; set; }
        public Player Host { get; set; }
        public Player[] Players  { get; set; }
    }
}
