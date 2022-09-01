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
using log4net;
using Newtonsoft.Json;

namespace SocketCommunication.Client
{

    public partial class GetInfo : Form
    {
        public static Socket cliSocket;
        public static string tc;
        public static string nameSurname;
        public static string jsonText;
        public DataTable dt;
        private ClientSide clientForm = null;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public GetInfo()
        {
            InitializeComponent();
        }
        public GetInfo(Form callingForm)
        {
            clientForm = callingForm as ClientSide;
            InitializeComponent();
        }
        #region Client'tan json textini çekip eşitlemek için kullanılan method
        #endregion
        public void GetJsonData(string json)
        {
            jsonText = json;
        }
        #region DataGridView'i doldurmak için DataSource'un belirlendiği method
        #endregion
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
        #region Client Form'undan erişim sağlayarak dataGridView'i doldurmaya yarayan property
        #endregion
        public DataTable FillGridWithJson
        {
            get { return dt; }
            set { dataGridView1.DataSource = value; }
        }
        #region TC değerinin jsona dönüştürülmek üzere Server'a gönderilmesi için çağrılan method
        #endregion
        private void SendCallback(IAsyncResult AR)
        {
            try
            {
                cliSocket.EndSend(AR);
            }
            catch (SocketException ex)
            {
                log.Error(VariableConfig.socketExError);
            }
            catch (ObjectDisposedException ex)
            {
                log.Error(VariableConfig.disposeError);
            }
        }

        private async void btnGetir_Click(object sender, EventArgs e)
        {
            //Server'a gönder
            //int clicked = 1;
            tc = txtTC.Text;
            this.clientForm.getTC = tc;
            this.clientForm.btnSend_Click(null, null);

            //clicked = 0;
            this.Close();
        }

        private void btnGonder_Click(object sender, EventArgs e)
        {
            nameSurname = txtName.Text + " " + txtSurname.Text;
            this.clientForm.getNameSurname = nameSurname;
            this.clientForm.btnSend_Click(null, null);
            //this.clientForm.FillMessagewithJson = jsonText;
            this.Close();
        }

        private void GetInfo_Load(object sender, EventArgs e)
        {
            txtTC.Text = tc;
            //if (jsonText != null) button1_Click(null,null);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //User result = JsonConvert.DeserializeObject<User>(jsonText);
            //dt = FillGrid(result);
            //dataGridView1.DataSource = dt;

        }
    }
}
