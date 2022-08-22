using log4net;
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
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Socket clientSocket;
        private byte[] buffer;
        private static string username;
        public ClientSide()
        {
            log4net.Config.XmlConfigurator.Configure();
            InitializeComponent();
        }
        #region Server socketinden Client Socketinin bilgilerine ulaşabilmek için kullanılan method
        #endregion
        public Socket sendSocket()
        {
            return clientSocket;
        }
        #region Server socketinden Client Socketi ile bağlanan userın usernameine ulaşabilmek için kullanılan method
        #endregion
        //public string GetUserName()
        //{
        //    ClientSide clientF = this;
        //    return clientF.txtUsername.Text;
        //}
        public Form GetClientForm()
        {
            //username = txtUsername.Text;
            return this;
        }
        
        private void AppendToTextBox(string txt)
        {
            Invoke((Action)delegate {
                txtContent.Text += Environment.NewLine +VariableConfig.serverString +txt;
                //connnect butonu, server-->dan clienta
            });
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                int received = clientSocket.EndReceive(AR);

                Array.Resize(ref buffer, received);
                string text = Encoding.ASCII.GetString(buffer);

                ////Invoke((Action)delegate
                ////{
                ////    text = "Server says: " + text+ Environment.NewLine;
                ////});

                AppendToTextBox(text);
                //txtContent.Text += text + "\n";

                //txtMessage.Text += text;
                //string message = Encoding.ASCII.GetString(buffer);

                Array.Resize(ref buffer, clientSocket.ReceiveBufferSize);

                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }

            catch (SocketException ex)
            {
                log.Error(VariableConfig.clienttoSocketConnectError);
                txtMessage.Text += VariableConfig.socketexError;
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += VariableConfig.objectDisposedExError;
            }
        }
        private void ConnectCallback(IAsyncResult AR)
        {
            try
            {
                clientSocket.EndConnect(AR);
                buffer = new byte[clientSocket.ReceiveBufferSize];
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            catch (SocketException ex)
            {
                log.Error(VariableConfig.connectCallbackError1+ username + VariableConfig.connectCallbackError2);
                txtMessage.Text += VariableConfig.socketexError;
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += VariableConfig.objectDisposedExError;
            }
        }


        private void SendCallback(IAsyncResult AR)
        {
            try
            {
                username = txtUsername.Text;
               var asd= clientSocket.EndSend(AR);
                log.Info(username+VariableConfig.sendCallbackError);
            }
            catch (SocketException ex)
            {
                txtMessage.Text += VariableConfig.socketexError;
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

                var endPoint = new IPEndPoint(IPAddress.Parse(txtHost.Text), int.Parse(txtPort.Text));
                clientSocket.BeginConnect(endPoint, ConnectCallback, null);
                log.Info($"{username}{VariableConfig.requestConnectServerError1}{txtHost.Text}:{txtPort.Text}{VariableConfig.requestConnectServerError2}");
            }
            catch (SocketException ex)
            {
                txtMessage.Text += VariableConfig.socketexError;
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += VariableConfig.objectDisposedExError;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                username = txtUsername.Text;
                byte[] buffer = Encoding.ASCII.GetBytes(username+">"+txtMessage.Text);
                log.Info(username+ VariableConfig.requestSendMessageError1 + txtMessage.Text + VariableConfig.requestSendMessageError2);
                clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
                
                txtMessage.Text = String.Empty;
            }
            catch (SocketException ex)
            {
                txtMessage.Text += VariableConfig.socketexError;
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
        public string FillMessagewithJson
        {
            get { return txtMessage.Text; }
            set { txtMessage.Text = value; }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            GetInfo gi = new GetInfo(this);
            gi.Show();
        }

        private void txtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text == VariableConfig.usernameString) txtUsername.Text = String.Empty;
        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (txtUsername.Text == String.Empty) txtUsername.Text = VariableConfig.usernameString;
        }
    }
}
