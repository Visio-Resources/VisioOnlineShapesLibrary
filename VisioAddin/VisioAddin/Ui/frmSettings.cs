using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace VisioAddin.Ui
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
            InitForms();
        }

        private void InitForms()
        {
            dataGridView.Rows.Clear();

            foreach (var server in Globals.ThisAddIn.ServerHandler.Servers)
            {
                dataGridView.Rows.Add(server.Name, server.Url, server.User, server.Password);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Models.ServerSettings serverSettings = new Models.ServerSettings();
            serverSettings.Servers = new List<Models.Server>();

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.IsNewRow) continue;
                Models.Server server = new Models.Server
                {
                    Name = row.Cells["Name"].Value?.ToString() ?? "",
                    Url = row.Cells["URL"].Value?.ToString() ?? "",
                    User = row.Cells["User"].Value?.ToString() ?? "",
                    Password = row.Cells["Password"].Value?.ToString() ?? ""
                };
                serverSettings.Servers.Add(server);
                serverSettings.CurrentServer = server.Name;
            }

            if (serverSettings.Servers.Where(s => s.Name == Globals.ThisAddIn.ServerHandler.CurrentServer).Count() == 1)
            {
                serverSettings.CurrentServer = Globals.ThisAddIn.ServerHandler.CurrentServer;
            }

            Globals.ThisAddIn.ServerHandler.ServerSettings = serverSettings;
            Globals.ThisAddIn.ServerHandler.Save();

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
