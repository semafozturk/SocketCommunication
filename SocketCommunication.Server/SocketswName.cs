using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketCommunication.Server
{
    public class SocketswName
    {
        public Socket clientSocket { get; set; }
        public string UserName { get; set; }
        //public static void SocketAdd(Socket socket, string username)
        //{
        //    clientSocket = socket;
        //    UserName = username;
        //}
    }
}
