using log4net;
using Newtonsoft.Json;
using SocketCommunication.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SocketCommunication
{
    public partial class ClientSide : Form
    {
        public static Socket clientSocket;
        private byte[] buffer;
        private static string username;
        private static string tc = null;
        private static string nameSurname = null;
        private static string jsonText;
        private GetInfo getForm = null;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ClientSide()
        {
            log4net.Config.XmlConfigurator.Configure();
            InitializeComponent();
        }
        public ClientSide(Form callingForm)
        {
            getForm = callingForm as GetInfo;
            InitializeComponent();
        }
        public string SendJsonText()
        {
            return jsonText;
        }
        public string sendUsername()
        {
            return txtUsername.Text;
        }
        #region Server socketinden Client Socketinin bilgilerine ulaşabilmek için kullanılan method
        #endregion
        public Socket sendSocket()
        {
            return clientSocket;
        }
        #region Server socketinden Client Socketi ile bağlanan userın usernameine ulaşabilmek için kullanılan method
        #endregion
        public string GetUserName()
        {
            ClientSide clientF = this;
            return clientF.txtUsername.Text;
        }
        #region Socketten gelen mesajı textboxa yazdırma methodu
        #endregion
        private void AppendToTextBox(string txt)
        {
            Invoke((Action)delegate {
                txtContent.Text += Environment.NewLine + txt;
            });
        }
        #region Soket aracılığıyla Server'den gelen mesajı karşılayan method
        #endregion
        private void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                int received = clientSocket.EndReceive(AR);
                //byte[] buff2=new byte[1024];
                byte[] buff2 = new byte[received];
                string text2 = Encoding.ASCII.GetString(buff2);
                //Array.Resize(ref buff2, clientSocket.ReceiveBufferSize);
                Array.Copy(buffer, buff2, received);
                string text3 = Encoding.ASCII.GetString(buffer);
                //Array.Resize(ref buffer, received);
                string text = Encoding.ASCII.GetString(buff2);
                Array.Resize(ref buffer, clientSocket.ReceiveBufferSize);
                //serverdan json değeri döndüğünde o değeri gridee gömmek için
                if (text.Contains(VariableConfig.bracketSign) && !text.Contains(VariableConfig.greaterThanSign))
                {
                    jsonText = text;
                    string cleanedJson = text.Replace("\0", " ").Trim();
                    User user = JsonConvert.DeserializeObject<User>(cleanedJson);
                    if (user.UserId != null) tc = user.UserId;
                    if (user.UserName != null && user.UserSurname != null) nameSurname = user.UserName + " " + user.UserSurname;
                    //btnSend_Click(null, null);
                    tc = null;
                    nameSurname = null;
                    button1_Click_1(null, null);
                }
                else { if (!text.Contains(VariableConfig.bracketSign) && text.Contains(VariableConfig.greaterThanSign)) AppendToTextBox(text); }
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), clientSocket);
            }

            catch (SocketException ex)
            {
                log.Error(VariableConfig.clienttoSocketConnectError);
                //txtMessage.Text += VariableConfig.socketexError;
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += VariableConfig.objectDisposedExError;
            }
        }
        #region Server'ın Socket bağlantısını başlatan method
        #endregion
        private void ConnectCallback(IAsyncResult AR)
        {
            try
            {

                clientSocket.EndConnect(AR);
                //username2=txtUsername.Text;
                //buffer= Encoding.ASCII.GetBytes(username2);
                //clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);
                buffer = new byte[clientSocket.ReceiveBufferSize];
                //Array.Resize(ref buffer, clientSocket.ReceiveBufferSize);
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), clientSocket);
                //Thread.Sleep(200);
            }
            catch (SocketException ex)
            {
                log.Error(VariableConfig.connectCallbackError1 + username + VariableConfig.connectCallbackError2);
                //txtMessage.Text += VariableConfig.socketexError;
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += VariableConfig.objectDisposedExError;
            }
        }

        #region Server'dan Socket'e mesaj gönderiminin sağlandığı method
        #endregion
        private void SendCallback(IAsyncResult AR)
        {
            try
            {
                //Thread.Sleep(1000);
                var asd = clientSocket.EndSend(AR);
                //txtMessage.Text = String.Empty;
                log.Info(username + VariableConfig.sendCallbackInfo);
            }
            catch (SocketException ex)
            {
                //txtMessage.Text += VariableConfig.socketexError;
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += VariableConfig.objectDisposedExError;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var endPoint = new IPEndPoint(IPAddress.Parse(txtHost.Text), 9000);
                if (!clientSocket.Connected)
                {
                    btnSend_Click(null, null);
                }
                //clientSocket.BeginConnect(endPoint, new AsyncCallback(ConnectCallback), null);
                log.Info($"{username}{VariableConfig.requestConnectServerError1}{txtHost.Text}:{txtPort.Text}{VariableConfig.requestConnectServerError2}");
            }
            catch (SocketException ex)
            {
                //txtMessage.Text += VariableConfig.socketexError;
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += VariableConfig.objectDisposedExError;
            }
        }

        public void btnSend_Click(object sender, EventArgs e)
        {
            try
            {

                username = txtUsername.Text;
                string text = String.Empty;
                if (!clientSocket.Connected)
                {
                    text = username;
                    var endPoint = new IPEndPoint(IPAddress.Parse(txtHost.Text), 9000);
                    clientSocket.BeginConnect(endPoint, new AsyncCallback(ConnectCallback), null);
                }
                else if (tc != null)
                {
                    User user = new User();
                    user.UserId = tc;
                    user.UserName = username;
                    text = JsonConvert.SerializeObject(user);
                }
                else if (nameSurname != null)
                {
                    User user = new User();
                    string[] nameSname = nameSurname.Split(' ');
                    user.UserName = nameSname[0];
                    user.UserSurname = nameSname[1];
                    user.UserInfo = username;
                    text = JsonConvert.SerializeObject(user);
                }

                else if (!txtMessage.Text.Contains('{')) text = username + VariableConfig.greaterThanSign + txtMessage.Text;
                else { text = txtMessage.Text; }
                byte[] buffer = Encoding.ASCII.GetBytes(text);
                log.Info(username + VariableConfig.requestSendMessageError1 + txtMessage.Text + VariableConfig.requestSendMessageError2);
                log.Debug(VariableConfig.bufferArrivedDebug + text);
                clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), clientSocket);
                txtMessage.Text = string.Empty;
            }
            catch (SocketException ex)
            {
                //txtMessage.Text += VariableConfig.socketexError;
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += VariableConfig.objectDisposedExError;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        #region Server Form'undan erişim sağlayarak Mesaj TextBox'ını doldurmaya yarayan property
        #endregion
        public string FillMessagewithJson
        {
            get { return txtMessage.Text; }
            set { txtMessage.Text = value; }
        }
        public string getTC
        {
            set { tc = value; }
        }
        public string getNameSurname
        {
            set { nameSurname = value; }
        }

        #region Tc değerinin jsona dönüştürülmek üzere Server'a gönderileceği GetInfo Formunun çağrıldığı Click Eventi
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            GetInfo gi = new GetInfo(this);
            gi.ShowDialog();


        }
        private void txtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text == VariableConfig.usernameString) txtUsername.Text = String.Empty;
        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (txtUsername.Text == String.Empty) txtUsername.Text = VariableConfig.usernameString;
        }
        #region DataGridView'i doldurulacak şekilde GetInfo Formunun çağrıldığı Click Eventi
        #endregion
        private void button1_Click_1(object sender, EventArgs e)
        {
            GetInfo gi = new GetInfo(this);
            gi.GetJsonData(jsonText);
            User result = JsonConvert.DeserializeObject<User>(jsonText);
            DataTable dt = gi.FillGrid(result);
            gi.FillGridWithJson = dt;

            gi.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
    }
}
