using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Visio = Microsoft.Office.Interop.Visio;

namespace VisioAddin
{
    public partial class TheForm : Form
    {
        private readonly Visio.Window _window;
        private bool initializing = true;

        /// <summary>
        /// Form constructor, receives parent Visio diagram window
        /// </summary>
        /// <param name="window">Parent Visio diagram window</param>
        public TheForm(Visio.Window window)
        {
            _window = window;
            InitializeComponent();
            InitializeAsync();
            Globals.ThisAddIn.OnContribute += OnContribute_Raised;
            InitializeForms();
        }

        private void InitializeForms()
        {
            initializing = true;

            cbServer.Items.Clear();

            if (Globals.ThisAddIn.ServerHandler.ServerSettings.Servers.Count > 0)
            {
                foreach (var server in Globals.ThisAddIn.ServerHandler.ServerSettings.Servers)
                {
                    cbServer.Items.Add(server.Name);
                }
                cbServer.SelectedItem = Globals.ThisAddIn.ServerHandler.CurrentServer;
            }

            initializing = false;
        }

        async void InitializeAsync()
        {
            string userDataFolder = Path.Combine(Path.GetTempPath(), System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            CoreWebView2Environment cwv2Environment = await CoreWebView2Environment.CreateAsync(null, userDataFolder, null);
            await webView.EnsureCoreWebView2Async(cwv2Environment);
            webView.CoreWebView2.AddHostObjectToScript("WebViewDragDrop", new WebViewDragDrop(this));
            Search("");
        }

        delegate void SearchCallback(string search);

        private void SetSearch(string search)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (webView.InvokeRequired)
            {
                SearchCallback d = new SearchCallback(SetSearch);
                this.Invoke(d, new object[] { search });
            }
            else
            {
                Search(search);
            }
        }

        private void OnContribute_Raised(object sender, EventArgs e)
        {
            SetSearch(sender as string);
        }

        private void Search(string search)
        {
            string postDataString = "search=" + search;
            UTF8Encoding utfEncoding = new UTF8Encoding();
            byte[] postData = utfEncoding.GetBytes(postDataString);
            MemoryStream postDataStream = new MemoryStream(postDataString.Length);
            postDataStream.Write(postData, 0, postData.Length);
            postDataStream.Seek(0, SeekOrigin.Begin);

            CoreWebView2WebResourceRequest webResourceRequest =
                webView.CoreWebView2.Environment.CreateWebResourceRequest(
                Globals.ThisAddIn.ServerHandler.CurrentServerUrl,
                "POST",
                postDataStream,
                "Content-Type: application/x-www-form-urlencoded\r\n");
            try
            {
                webView.CoreWebView2.NavigateWithWebResourceRequest(webResourceRequest);
            }
            catch { }
        }

        // Open original preview in standard browser
        private void webView_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            if (!(e.Uri.ToString().Equals(Globals.ThisAddIn.ServerHandler.CurrentServerUrl + "/", StringComparison.InvariantCultureIgnoreCase)))
            {
                e.Cancel = true;
                System.Diagnostics.Process.Start(e.Uri.ToString());
            }
        }

        // Accessed from JavaScript
        [ComVisible(true)]
        public class WebViewDragDrop
        {
            TheForm M;

            public WebViewDragDrop(TheForm m)
            {
                this.M = m;
            }

            public void DragDropShape(string shapeData)
            {
                try
                {
                    MemoryStream stream = new MemoryStream(Convert.FromBase64String(shapeData));
                    DataObject obj = new DataObject("Visio 15.0 Shapes", stream);
                    M.DoDragDrop(obj, DragDropEffects.All);
                }
                catch { }
            }
        }

        private void btnContributeShape_Click(object sender, EventArgs e)
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

        private void btnContributeStencil_Click(object sender, EventArgs e)
        {
            var form = new Ui.FrmContributeStencil();
            form.ShowDialog();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Ui.frmSettings form = new Ui.frmSettings();
            form.ShowDialog();
            InitializeForms();
        }

        private void cbServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initializing) return;
            Globals.ThisAddIn.ServerHandler.CurrentServer = cbServer.ComboBox.SelectedItem.ToString();
            Search("");
        }
    }
}