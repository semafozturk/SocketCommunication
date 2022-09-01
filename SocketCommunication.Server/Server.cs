using Newtonsoft.Json;
using SocketCommunication.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketCommunication.Server
{
    public partial class ServerSide : Form
    {
        //TcpListener? server = null;
        private Socket serverSocket;
        private Socket clientSocket; 
        private byte[] buffer;
        IPAddress ip;
        int port;
        string _username;
        private static string tc;
        private static string jsonText;
        public string SendTc()
        {
            return tc;
        }
        public string SendJsonText()
        {
            return jsonText;
        }
        
        
        public ServerSide()
        {
            log4net.Config.XmlConfigurator.Configure();
            
            InitializeComponent();
        }
        //protected Socket AcceptMethod(Socket listeningSocket)
        //{
        //    Socket mySocket;
        //    using (mySocket = listeningSocket.Accept())
        //        return mySocket;
        //}

        private void AcceptCallback(IAsyncResult AR)
        {
            try
            {
                clientSocket = serverSocket.EndAccept(AR);
                buffer = new byte[clientSocket.ReceiveBufferSize];
                
                var sendData = Encoding.ASCII.GetBytes(txtMessage.Text);
                clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, null);
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
                txtMessage.Text = String.Empty;
                serverSocket.BeginAccept(AcceptCallback, null);
                log.Info("AcceptCallback");
            }
            catch (SocketException ex)
            {
                txtMessage.Text += "AcceptCallBack SocketException Hatası";
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += "AcceptCallBack ObjectDisposedException Hatası";
            }
        }
        private void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                int received = clientSocket.EndReceive(AR);
                
                //_username = clientForm.GetUserName();
                Array.Resize(ref buffer,received);
                string text=Encoding.ASCII.GetString(buffer);
                string[] jsonSplitted = text.Split('>');
                int usernameLen = jsonSplitted[0].Length;
                _username = text.Substring(0, usernameLen);
                jsonText = text.Remove(0, usernameLen+1);
                User result = JsonConvert.DeserializeObject<User>(jsonText);
                tc = result.UserId;
                AppendToTextBox(text);
                btnClick_Click(null, null);
                //btnClick.Click += new EventHandler(btnClick_Click);

                ////Array.Resize(ref buffer,clientSocket.ReceiveBufferSize);

                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            catch (SocketException ex)
            {
                txtMessage.Text += "SOCKETEX HATASI";
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += "ObjectDisposedException HATASI";
            }
        }

        private void AppendToTextBox(string txt)
        {
            //ClientSide clientForm=new ClientSide();
            //Form c = clientForm.GetClientForm();
            //TextBox teext = (TextBox)clientForm.Controls.Find("txtUsername", true).FirstOrDefault();
            Invoke((Action)delegate
              {
                  txtContent.Text += Environment.NewLine+txt;
                  txtMessage.Text = String.Empty;
              });
        }
        private void SendCallback(IAsyncResult AR)
        {
            try
            {
                clientSocket.EndSend(AR);
                log.Info("SendCllback");
            }
            catch (SocketException ex)
            {
                txtMessage.Text += "SendCallback SocketException Hatası";
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += "SendCallback ObjectDisposedException Hatası";
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            log.Info("SERVER LOAD");
            Control.CheckForIllegalCrossThreadCalls = false;
            btnStop.Enabled = false;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = true;
            btnStart.Enabled = false;
            ip = IPAddress.Parse(txtHost.Text);
            port = Convert.ToInt32(txtPort.Text);
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                serverSocket.Listen(10);
                serverSocket.BeginAccept(AcceptCallback, null);
                log.Info("Server bağlantısı başlatıldı.");
            }
            catch (SocketException ex)
            {
                txtMessage.Text += "Click SocketException Hatası";
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += "Click ObjectDisposedException Hatası";
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                //PersonPackage person = new PersonPackage(checkBoxMale.Checked, (ushort)numberBoxAge.Value, textBoxEmployee.Text);
                byte[] buffer = Encoding.ASCII.GetBytes(txtMessage.Text);
                clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
                log.Info("Mesaj Gönderimi tamam.");
                txtMessage.Text = String.Empty;
            }
            catch (SocketException ex)
            {
                txtMessage.Text += "btnSend_Click SocketException Hatası";
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += "btnSend_Click ObjectDisposedException Hatası";
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            //serverSocket.Shutdown(SocketShutdown.Both);
            //serverSocket.Close();
        }

        private void btnClick_Click(object sender, EventArgs e)
        {
            GetInfo gi = new GetInfo(this);
            gi.ShowDialog();
        }
    }
}
