using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketCommunication.Client
{
    public class VariableConfig
    {
        public static string BaseUrl = "https://localhost:44355/";
        public static string endpointUrl = "api/Users/GetById?tc=";
        public static string tc = "TC NO";
        public static string userName = "User Name";
        public static string userSurname = "User Surname";
        public static string userInfo = "User Info";
        public static string usernameString = "username";
        public static string clienttoSocketConnectError = "Socket bağlantısı kesildi.";
        public static string socketexError = "Socket kaynaklı bağlantı hatası.";
        public static string objectDisposedExError = "Socket bağlantısı kaynaklı ObjectDisposedException hatası.";
        public static string connectCallbackError1 = "Server bağlantısı başlatılmadığı için, ";
        public static string connectCallbackError2 = " Client'ı bağlanamadı.";
        public static string sendCallbackInfo = " Client'ı Server'a bağlandı.";
        public static string requestSendMessageError1 = " usernameli Client, Servera ";
        public static string requestSendMessageError2 = " mesajı gönderme isteğinde bulundu.";
        public static string requestConnectServerError1 = " Client'ı ";
        public static string requestConnectServerError2 = " Portu üzerinden Server'a bağlanma isteği gönderdi.";
        public static string socketExError = " Server'a bilgi aktarılırken Socket bağlantısı kaynaklı hata oluştu.";
        public static string disposeError = " Server'a bilgi aktarılırken Socket bağlantısının reddedilmesinden dolayı hata oluştu.";

        //Server Log Variables
        public static string connectStarted = "Server bağlantısı başlatıldı.";
        public static string okMessageSend = "Server'dan Client'a mesaj gönderimi gerçekleşti.";
        public static string okMessageArrived = " Client'ı tarafından gönderilen mesaj Server'a ulaştı.";
        public static string serverString = "Server> ";
        public static string apiConnectError = "Server Api'ye bağlanamadı.";
        public static string whileApiConnectError = "Server Api'ye bağlanırken bir hata oluştu.";
        public static string acceptClientConRequest = "Client'ın bağlantı isteği Server tarafından kabul edildi.";
        public static string infoRequestwTc = " Tc'li kullanıcı için bilgi talebi Api'den getirilmek üzere Server'a iletildi.";
        public static string messageArrivedtoServer1 = "Client tarafından gönderilen ";
        public static string messageArrivedtoServer2 = " mesajı Server'a ulaştı.";
        public static string connectedtoApi = "Server Api'ye bağlantı sağladı.";
        public static string bufferArrivedDebug = "Client'tan Server'a ulaşan veri : ";
        public static string bufferArrivedDebug2 = "Client'tan Server'a gönderilecek veri : ";
        public static string greaterThanSign = ">";
        public static string starSign = "*";
        public static char starSignChar = '*';
        public static string bracketSign = "{";
    }
}
