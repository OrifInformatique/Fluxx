using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fluxx
{
    class ServerConnection
    {
        private static Uri url;
        private static Socket socket;

        public Socket Socket()
        {
            url = new Uri("http://192.168.3.74:8080");
            if(socket == null)
            {
                socket = IO.Socket(url);
            }
            return socket;
        }
    }
}
