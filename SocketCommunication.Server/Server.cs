using log4net;
using Newtonsoft.Json;
using Salaros.Configuration;
using SocketCommunication.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketCommunication.Server
{
    public partial class ServerSide : Form
    {
        //TcpListener? server = null;
        private static List<SocketswName> sockets = new List<SocketswName>();
        private Socket serverSocket;
        private Socket clientSocket;
        private static int receivedMessage = 0;
        private byte[] buffer;
        private byte[] buff2;
        IPAddress ip;
        int port;
        public static string _username;
        private static string tc;
        private static string jsonText;
        public static User userModel;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string appConf = @"C:\Users\sema.ozturk\Desktop\Sockettt\SocketCommunication_Async_withGetInfo_ServerBasewLog_ReadFromIniFileinUserServ_GetById_12\SocketCommunication\Variables.ini";
        #region Server Form'undan Api'ye bağlanarak Client'tan gelen Tc değerine göre json dönen method
        #endregion
        public async Task<string> ConnectApi(User user)
        {
            var cfg = new ConfigParser(appConf);
            string baseUrl = cfg.GetValue("ReadDetails", "baseUrl");
            string endpUrl = cfg.GetValue("ReadDetails", "getByIdEndpointUrl");
            try
            {
                var client = new HttpClient();
                HttpResponseMessage response = new HttpResponseMessage();
                if (user.UserId == null)
                {

                    client.BaseAddress = new Uri(baseUrl);
                    string api = "api/Users/GetByNameSurname?name=" + user.UserName + "&surname=" + user.UserSurname;
                    response = await client.GetAsync(api);
                }
                else if (user.UserSurname == null && user.UserInfo == null)
                {
                    client.BaseAddress = new Uri(baseUrl);
                    string api = endpUrl + user.UserId;
                    response = await client.GetAsync(api);
                }
                if (response.IsSuccessStatusCode)
                {
                    log.Info(VariableConfig.connectedtoApi);
                    jsonText = await response.Content.ReadAsStringAsync();
                    //jsonText = response.Content.ReadAsStringAsync().Result;
                    //var dataSource = response.Content.ReadAsStringAsync().Result;
                    //User result = JsonConvert.DeserializeObject<User>(jsonText);
                    return jsonText;
                }
                log.Error(VariableConfig.apiConnectError);
                return VariableConfig.apiConnectError;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return VariableConfig.whileApiConnectError;
            }
        }
        #region Socketten gelen mesajı textboxa yazdırma methodu
        #endregion
        private void AppendToTextBox(string txt)
        {
            Invoke((Action)delegate
            {
                txtContent.Text += Environment.NewLine + txt;
                txtMessage.Text = String.Empty;
            });
        }
        public ServerSide()
        {
            
            log4net.Config.XmlConfigurator.Configure();
            InitializeComponent();
        }
        #region Client'tan gelen bağlantı isteğini kabul eden method
        #endregion
        private void AcceptCallback(IAsyncResult AR)
        {
            try
            {
                Socket socket = serverSocket.EndAccept(AR);
                //SocketswName socketwName = new SocketswName();
                //socketwName.clientSocket = socket;
                //sockets.Add(clientSocket);
                //clientSocket = serverSocket.EndAccept(AR);
                //buffer = new byte[socket.ReceiveBufferSize];
                Array.Resize(ref buffer, socket.ReceiveBufferSize);
                //Array.Resize(ref buffer, socket.ReceiveBufferSize);
                //var sendData = Encoding.ASCII.GetBytes(txtMessage.Text);
                //socket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);

                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
                //Thread.Sleep(3500);
                //Array.Resize(ref buffer, received);
                //string text = Encoding.ASCII.GetString(buffer);
                //txtContent.Text = "123";
                //socketwName.UserName = _username;
                serverSocket.BeginAccept(AcceptCallback, null);

                //sockets.Add(socketwName);
                log.Info(VariableConfig.acceptClientConRequest);
            }
            catch (SocketException ex)
            {
                txtMessage.Text += VariableConfig.socketexError;
                log.Error(VariableConfig.socketexError);
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += VariableConfig.objectDisposedExError;
                log.Error(VariableConfig.objectDisposedExError);
            }
        }
        #region Client'tan Server'a gelen mesaj isteğini kabul eden method
        #endregion
        private async void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                Socket socket = (Socket)AR.AsyncState;
                receivedMessage++;
                int received = socket.EndReceive(AR);
                byte[] buffer2 = new byte[received];
                //Array.Resize(ref buffer2,received);//new received
                Array.Copy(buffer, buffer2, received);
                string text = Encoding.ASCII.GetString(buffer2);
                //Array.Copy(buffer, buffer2, received);
                //string text=Encoding.ASCII.GetString(buffer);
                //User userModel;
                log.Debug(VariableConfig.bufferArrivedDebug + text);
                //normal mesaj
                if (text.Contains(VariableConfig.greaterThanSign))
                {
                    string[] splitted = text.Split(VariableConfig.greaterThanSign);
                    int usernameLen = splitted[0].Length;
                    //_username = splitted[0];
                    AppendToTextBox(text);
                    log.Info($"{VariableConfig.messageArrivedtoServer1}\"{text}\"{VariableConfig.messageArrivedtoServer2}");
                }
                //socketle gelen değer json ise
                if (text.Contains('{'))
                {
                    //json gönderirken
                    //getinfoya jsonı doldurup geri döndürme
                    if (!text.Contains('>'))
                    {
                        userModel = JsonConvert.DeserializeObject<User>(text);
                        if (userModel.UserSurname == null && userModel.UserInfo == null)
                        {
                            _username = userModel.UserName;
                            string jsonFromApi = await ConnectApi(userModel);
                            //User userModel2 = JsonConvert.DeserializeObject<User>(jsonFromApi);
                            AppendToTextBox(_username + ">" + jsonFromApi);
                            //Array.Resize(ref buffer, jsonFromApi.Length);
                            byte[] data = Encoding.ASCII.GetBytes(jsonFromApi);
                            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);


                            //buffer = Encoding.ASCII.GetBytes(jsonFromApi);
                            //socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                        }
                        else if (userModel.UserId == null) //ad soyada göre arama
                        {
                            _username = userModel.UserInfo;
                            string jsonFromApi = await ConnectApi(userModel);
                            AppendToTextBox(_username + ">" + jsonFromApi);
                            //Array.Resize(ref buffer, jsonFromApi.Length);
                            byte[] data = Encoding.ASCII.GetBytes(jsonFromApi);
                            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);

                            //buffer = Encoding.ASCII.GetBytes(jsonFromApi);
                            //socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);

                        }
                        else AppendToTextBox(_username + ">" + text);
                    }
                    //jsonı mesaj olarak gönderirken
                    else
                    {
                        AppendToTextBox(_username + ">" + text);
                    }
                }
                if (!text.Contains('>') && !text.Contains(VariableConfig.bracketSign))
                {
                    _username = text;
                    SocketswName socketwName = new SocketswName();
                    socketwName.clientSocket = socket;
                    socketwName.UserName = _username;
                    sockets.Add(socketwName);
                    string cleanedUserName = text.Replace("\0", " ");
                    if (!lstUsers.Items.Contains(cleanedUserName.Trim())) lstUsers.Items.Add(_username);
                    else AppendToTextBox(text);
                    //if (receivedMessage == 3) AppendToTextBox(text);
                }
                log.Info(_username + VariableConfig.okMessageArrived);
                //Thread.Sleep(1000);
                Array.Resize(ref buffer, socket.ReceiveBufferSize);
                //btnClick_Click(null, null);
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            }
            catch (SocketException ex)
            {
                txtMessage.Text += VariableConfig.socketexError;
                log.Error(VariableConfig.socketexError);
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += VariableConfig.objectDisposedExError;
                log.Error(VariableConfig.objectDisposedExError);
            }
        }

        #region Server'dan Client'a mesaj gönderme methodu
        #endregion
        private void SendCallback(IAsyncResult AR)
        {
            try
            {
                Socket socket = (Socket)AR.AsyncState;
                socket.EndSend(AR);

            }
            catch (SocketException ex)
            {
                txtMessage.Text += VariableConfig.socketexError;
                log.Error(VariableConfig.socketexError);
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += VariableConfig.objectDisposedExError;
                log.Error(VariableConfig.objectDisposedExError);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
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
                log.Info(VariableConfig.connectStarted);
            }
            catch (SocketException ex)
            {
                txtMessage.Text += VariableConfig.socketexError;
                log.Error(VariableConfig.socketexError);
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += VariableConfig.objectDisposedExError;
                log.Error(VariableConfig.objectDisposedExError);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(VariableConfig.serverString + txtMessage.Text);
                //List<string> names = new List<string>();
                Socket selectedSocket = null;
                for (int i = 0; i < sockets.Count; i++)
                {
                    //names.Add(sockets[i].UserName.ToString());
                    if (lstUsers.SelectedItem.ToString() == sockets[i].UserName?.Trim())
                    {
                        selectedSocket = sockets[i].clientSocket;
                    }
                }
                selectedSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), selectedSocket);

                //for (int i = 0; i < sockets.Count; i++)
                //{
                //    sockets[i].BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), sockets[i]);
                //}
                log.Info(VariableConfig.okMessageSend);
                txtMessage.Text = String.Empty;
            }
            catch (SocketException ex)
            {
                txtMessage.Text += VariableConfig.socketexError;
                log.Error(VariableConfig.socketexError);
            }
            catch (ObjectDisposedException ex)
            {
                txtMessage.Text += VariableConfig.objectDisposedExError;
                log.Error(VariableConfig.objectDisposedExError);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            //serverSocket.Shutdown(SocketShutdown.Both);
            //serverSocket.Close();
            //for (int i = 0; i < sockets.Count; i++)
            //{
            //    sockets[i].Close();
            //}
        }

        private void btnClick_Click(object sender, EventArgs e)
        {
            //GetInfo gi = new GetInfo(this);
            //gi.ShowDialog();
        }
    }
}
