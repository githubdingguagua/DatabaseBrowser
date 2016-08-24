using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace DbBrowser
{
    public partial class ConnectWindow : Form
    {
        public ConnectWindow()
        {
            InitializeComponent();
        }

        //Check to enable lblUsername, txtusername, lblPassword and txtPassword when chkLocalAccount is unchecked or diable them when chkLocalAccount is checked
        private void chkLocalAccount_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                labelUsername.Enabled = false;
                labelPassword.Enabled = false;

                txtUsername.Enabled = false;
                txtPassword.Enabled = false;
            }
            else
            {
                labelUsername.Enabled = true;
                labelPassword.Enabled = true;

                txtUsername.Enabled = true;
                txtPassword.Enabled = true;
            }
        }

        //Setting Settings.Connected and Settings.ConnectionString.
        private void btnConnect_Click(object sender, EventArgs e)
        {
            //if chkLocalAccount is checked Settings.ConnectionString will be set to use a integrated account.
            if(chkLocalAccount.Checked == true)
            {
                Settings.ConnectionString =
                    String.Format("Server={0};Integrated Security=true;", txtServer.Text);
            }
            //if chkLocalAccount is unchecked Settings.ConnectionString will be set to use a username and password.
            else
            {
                Settings.ConnectionString =
                    String.Format("Server={0};User Id={1};Password={2}", txtServer.Text, txtUsername.Text, txtPassword.Text);
            }
   
            Task.Run(() =>
            {
                bool isConnected = true;
                //try connection to the server using Settings.ConnectionString.
                try
                {
                    using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
                        connection.Open();
                }
                //If the connection failed for any reason the bool isConnected is set to false 
                catch
                {
                    isConnected = false;
                }

                //Connection status is set to Settings.Connected
                Settings.Connected = isConnected;

                //If the connection fails show the user a MessageBox.(Could be more detailed)
                if (!isConnected)
                {
                    MessageBox.Show("Unable to connect", "Error");
                    return;
                }
            });
            this.Close();
        }
    }
}
