using Newtonsoft.Json;
using SocketCommunication.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketCommunication.Server
{
    public partial class GetInfo : Form
    {
        public static string jsonText;
        //public string tc;
        private ServerSide serverForm = null;

        public GetInfo(Form callingForm)
        {
            serverForm = callingForm as ServerSide;
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
            var client = new HttpClient();
            client.BaseAddress = new Uri(VariableConfig.BaseUrl);
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                string api = VariableConfig.endpointUrl + "11";
                response = await client.GetAsync(api);
                if (response.IsSuccessStatusCode)
                {
                    jsonText = await response.Content.ReadAsStringAsync();
                    string[] jsonSplitted = jsonText.Split('>');
                    int usernameLen = jsonSplitted[0].Length;
                    jsonText = jsonText.Remove(0, usernameLen+1);
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

        }

        private void GetInfo_Load(object sender, EventArgs e)
        {
            //string json = serverForm.SendJsonText();
            //User result = JsonConvert.DeserializeObject<User>(json);
            //var dt = FillGrid(result);
            //dataGridView1.DataSource = dt;
        }
    }
}
