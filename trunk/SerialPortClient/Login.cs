using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tamir.SharpSsh.jsch;
using MySql.Data.MySqlClient;

namespace SerialPortClient
{
    public partial class Login : Form
    {
        const string quote = "\'";
        public Login()
        {
            InitializeComponent();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            userPassTextChange();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            userPassTextChange();
        }

        private void userPassTextChange()
        {
            btnLogin.Enabled = (CommonFunctions.NotFormOfBlank(txtUsername.Text) & CommonFunctions.NotFormOfBlank(txtPassword.Text));
        }

        

        private void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;
            txtUsername.Enabled = false;
            txtPassword.Enabled = false;
            bool authorized = false;
            MySqlConnection conn = new MySqlConnection();
            JSch jsch = new JSch();
            string host = "secs.oakland.edu";
            string user = "iafram";
            string pass = "password";
            int sshPort = 22;
            Session session = jsch.getSession(user, host, sshPort);
            session.setHost(host);
            session.setPassword(pass);
            UserInfo ui = new MyUserInfo();
            session.setUserInfo(ui);
            session.connect();
            session.setPortForwardingL(3306, "localhost", 3306);

            if (session.isConnected()){
                try
				{
					string dbhost = "localhost";
					string dbuser = "iafram";
					string dbpass = "password";
					string dbdatabase = "iafram";
					string connStr = String.Format("server={0};user id={1};password={2}; database={3}; pooling=false",
                        dbhost, dbuser, dbpass, dbdatabase);
					conn = new MySqlConnection(connStr);
					conn.Open();
					conn.ChangeDatabase(dbdatabase);

                    string query = "SELECT COUNT(*) ";
                           query += "FROM cse337_project ";
                           query += "WHERE username = " + quote + txtUsername.Text + quote;
                           query += " AND password = " + quote + txtPassword.Text + quote;

                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = query;
                    MySqlDataReader dataReader = command.ExecuteReader();
                    dataReader.Read();
                    int rows = int.Parse(dataReader.GetValue(0).ToString());
                    if (rows == 1)
                    {
                        authorized = true;
                    }
                    else
                    {
                        authorized = false;
                    }
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
					session.disconnect();
				}
				finally
				{
					conn.Close();
				}
                session.disconnect();

                if (authorized)
                {
                    this.Visible = false;
                    Main m = new Main(txtUsername.Text);
                    m.ShowDialog();
                    btnLogin.Enabled = true;
                    txtUsername.Enabled = true;
                    txtPassword.Enabled = true;
                    this.Visible = true;
                }
                else
                {
                    MessageBox.Show("Incorrect username/password combination.");
                    btnLogin.Enabled = true;
                    txtUsername.Enabled = true;
                    txtPassword.Enabled = true;
                }
            }
        }

        public class MyUserInfo : UserInfo
        {

            private String passwd;

            public String getPassword() { return passwd; }

            public bool promptYesNo(String str)
            {
                return true;
            }

            public String getPassphrase() { return null; }

            public bool promptPassphrase(String message) { return true; }

            public bool promptPassword(String message) { return true; }

            public void showMessage(String message) { }

        }
        
    }
}