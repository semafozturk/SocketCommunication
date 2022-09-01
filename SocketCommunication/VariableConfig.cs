using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketCommunication.Client
{
    public class VariableConfig
    {
        public static string BaseUrl= "https://localhost:7168/";
        public static string endpointUrl = "api/Users/GetById?tc=";
        public static string tc = "TC NO";
        public static string userName = "User Name";
        public static string userSurname = "User Surname";
        public static string userInfo = "User Info";
        public static string serverString = "Server: ";
        public static string usernameString = "username";
        public static string clienttoSocketConnectError = "Socket bağlantısı kesildi.";
        public static string socketexError = "Socket Exception HATASI";
        public static string objectDisposedExError = "ObjectDisposedException HATASI";
        public static string connectCallbackError1 = "Server bağlantısı başlatılmadığı için, ";
        public static string connectCallbackError2 = " Client'ı bağlanamadı.";
        public static string sendCallbackInfo = " Client'ı Server'a bağlandı.";
        public static string requestSendMessageError1 = " usernameli Client, Servera ";
        public static string requestSendMessageError2 = " mesajı gönderme isteğinde bulundu.";
        public static string requestConnectServerError1 = " Client'ı ";
        public static string requestConnectServerError2 = " Portu üzerinden Server'a bağlanma isteği gönderdi.";

    }
}
