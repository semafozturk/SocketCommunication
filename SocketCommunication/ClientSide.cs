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
        public ClientSide()
        {
            InitializeComponent();
        }
        public Socket sendSocket()
        {
            return clientSocket;
        }
        public string GetUserName()
        {
            ClientSide clientF = this;
            return clientF.txtUsername.Text;
        }
        public Form GetClientForm()
        {
            //username = txtUsername.Text;
            return this;
        }
        
        private void AppendToTextBox(string txt)
        {
            Invoke((Action)delegate {
                txtContent.Text += Environment.NewLine +"Server: " +txt;
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
                txtMessage.Text += "SOCKETEX HATASI";
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += "ObjectDisposedException HATASI";
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
                txtMessage.Text += "ConnectCallback SocketException Hatası";
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += "ConnectCallback ObjectDisposedException Hatası";
            }
        }


        private void SendCallback(IAsyncResult AR)
        {
            try
            {
                username = txtUsername.Text;
               var asd= clientSocket.EndSend(AR);
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

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                var endPoint = new IPEndPoint(IPAddress.Parse(txtHost.Text), 9000);
                clientSocket.BeginConnect(endPoint, ConnectCallback, null);
            }
            catch (SocketException ex)
            {
                txtMessage.Text += "btnStart_Click SocketException Hatası";
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += "btnStart_Click ObjectDisposedException Hatası";
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                username = txtUsername.Text;
                byte[] buffer = Encoding.ASCII.GetBytes(username+">"+txtMessage.Text);
                clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
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
            if (txtUsername.Text == "username") txtUsername.Text = String.Empty;
        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (txtUsername.Text == String.Empty) txtUsername.Text = "username";
        }
    }
}
