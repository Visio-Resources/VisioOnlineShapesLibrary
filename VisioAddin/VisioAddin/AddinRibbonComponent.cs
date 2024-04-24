using Microsoft.Office.Tools.Ribbon;

namespace VisioAddin
{
    public partial class AddinRibbonComponent
    {
        private void btnOnlineShapes_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.TogglePanel();
        }
    }
}