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
                dataGridView.Rows.Add(server.Name, server.Url, server.Token);
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
                    Name = row.Cells["colName"].Value?.ToString() ?? "",
                    Url = row.Cells["colUrl"].Value?.ToString() ?? "",
                    Token = row.Cells["colToken"].Value?.ToString() ?? ""
                };
                serverSettings.Servers.Add(server);
                serverSettings.CurrentServer = server.Name;
            }

            if (serverSettings.Servers.Where(s => s.Name == Globals.ThisAddIn.ServerHandler.CurrentServerName).Count() == 1)
            {
                serverSettings.CurrentServer = Globals.ThisAddIn.ServerHandler.CurrentServerName;
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