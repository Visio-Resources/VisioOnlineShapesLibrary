using System;
using System.Windows.Forms;

namespace VisioAddin.Ui
{
    public partial class FrmContributeStencil : Form
    {
        public FrmContributeStencil()
        {
            InitializeComponent();
        }

        private void btnContribute_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
