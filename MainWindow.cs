using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbBrowser
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //calling ConnectWindow form when btnConnect is clicked and calls dlgConnectWindowClosing() when ConnectWindow is closed.
        private void btnConnect_Click(object sender, EventArgs e)
        {
            var dlgConnectWindow = new ConnectWindow();
            dlgConnectWindow.Closing += (s, cea) => dlgConnectWindowClosing();
            dlgConnectWindow.ShowDialog();
        }

        private void dlgConnectWindowClosing()
        {
            //Check if Setting.Connected is true.
            if (Settings.Connected == true)
            {
                trvBrowser.Nodes.Clear();
            }
            //calling SqlHelper class Method Getdatabases to get all databases in the connected server and adding them to the treeview
            foreach (string database in SqlHelper.GetDatabases())
            {
                TreeNode toAdd = new TreeNode(database);
                trvBrowser.Nodes.Add(toAdd);

                //Getting all Tables and adding them to the correct treeview parents.
                foreach (string table in SqlHelper.GetTablesForDatabase(database))
                {
                    TreeNode toAddSub = new TreeNode(table);
                    toAdd.Nodes.Add(toAddSub);
                }
            }
        }

        //clearing all data in the application and setting Settings.Connected to false and clearing Settings.Connectionstring.
        private void btnExit_Click(object sender, EventArgs e)
        {
            trvBrowser.Nodes.Clear();
            textQuery.Text = "";
            grdResult.DataSource = null;
            Settings.ConnectionString = "";
            Settings.Connected = false;
        }

        //calling FillDatagrid() when btnExecute is clicked.
        private void btnExecute_Click(object sender, EventArgs e)
        {
            FillDatagrid();
        }

        //Double clicking on a table in the treeview adds a query to txtQuery and starting Filldatagrid().
        private void trvBrowser_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (trvBrowser.SelectedNode.Parent != null)
            {
                string tbl = Convert.ToString(trvBrowser.SelectedNode.Text);
                string db = Convert.ToString(trvBrowser.SelectedNode.Parent.Text);
                string query = string.Format("SELECT * FROM {0}.dbo.{1}", db, tbl);

                textQuery.Text = query;
                FillDatagrid();
            }
        }

        //calling SqlHelper.executeQuery with the text in txtquery.text and filling the datagridview with the returned datatable.
        private void FillDatagrid()
        {
            DataTable datatable = SqlHelper.ExecuteQuery(textQuery.Text);

            grdResult.DataSource = datatable;
        }
    }
}
