using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketCommunication.Client
{
    public class SocketswName
    {
        private Socket clientSocket { get; set; }
        private string UserName { get; set; }
        public void SocketAdd(Socket socket,string username)
        {
            clientSocket= socket;
            UserName= username;
        }
    }
}
