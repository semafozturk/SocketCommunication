using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace SocketCommunication.Client
{
    
    public partial class GetInfo : Form
    {
        //public static Socket cliSocket;
        public static string jsonText;
        private ClientSide clientForm = null;
        public GetInfo(Form callingForm)
        {
            clientForm = callingForm as ClientSide;
            InitializeComponent();
        }
        public string GetJsonText()
        {
            return jsonText;
        }
        public string SendJsonData()
        {
            return jsonText;
        }
        public DataTable FillGrid(User user)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TC NO");
            dt.Columns.Add("User Name");
            dt.Columns.Add("User Surname");
            dt.Columns.Add("User Info");
            DataRow dr = dt.NewRow();
            dr[0] = user.UserId;
            dr[1] = user.UserName;
            dr[2] = user.UserSurname;
            dr[3] = user.UserInfo;
            dt.Rows.Add(dr);
            return dt;
        }
        public GetInfo()
        {
            InitializeComponent();
        }
        

        private async void btnGetir_Click(object sender, EventArgs e)
        {
            ////ClientSide clientForm = new ClientSide();
            ////cliSocket = clientForm.sendSocket();
            var client = new HttpClient();
            client.BaseAddress = new Uri(VariableConfig.BaseUrl);
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                string api = VariableConfig.endpointUrl + txtTC.Text;
                response = await client.GetAsync(api);
                if (response.IsSuccessStatusCode)
                {
                    jsonText = await response.Content.ReadAsStringAsync();
                    var dataSource = response.Content.ReadAsStringAsync().Result;

                    User result = JsonConvert.DeserializeObject<User>(jsonText);
                    var dt = FillGrid(result);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnGonder_Click(object sender, EventArgs e)
        {
            this.clientForm.FillMessagewithJson = jsonText;
            this.Close();
        }
    }
}
