using Microsoft.Office.Tools.Ribbon;
using System.Windows.Forms;
using Visio = Microsoft.Office.Interop.Visio;

namespace VisioAddin
{
    public partial class AddinRibbonComponent
    {
        private void btnOnlineShapes_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.TogglePanel();
        }

        private void sBtnContribute_Click(object sender, RibbonControlEventArgs e)
        {
            Visio.Shape shape = null;
            if (Globals.ThisAddIn.Application.ActiveWindow.Type == (short)Visio.VisWinTypes.visDrawing)
            {
                shape = Globals.ThisAddIn.Application.ActiveWindow.Selection.PrimaryItem;
            }
            if (shape == null)
            {
                MessageBox.Show("No shape selected.", "Contribute Shape");
                return;
            }
            var form = new Ui.FrmContributeShape(shape);
            form.ShowDialog();
        }
    }
}