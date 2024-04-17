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

        /// <summary>
        /// Form constructor, receives parent Visio diagram window
        /// </summary>
        /// <param name="window">Parent Visio diagram window</param>
        public TheForm(Visio.Window window)
        {
            _window = window;
            InitializeComponent();
            InitializeAsync();
        }

        async void InitializeAsync()
        {
            string userDataFolder = Path.Combine(Path.GetTempPath(), System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            CoreWebView2Environment cwv2Environment = await CoreWebView2Environment.CreateAsync(null, userDataFolder, null);
            await webView.EnsureCoreWebView2Async(cwv2Environment);
            webView.CoreWebView2.AddHostObjectToScript("WebViewDragDrop", new WebViewDragDrop(this));
            Search("");
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
                "http://127.0.0.1:5000",
                "POST",
                postDataStream,
                "Content-Type: application/x-www-form-urlencoded\r\n");
            webView.CoreWebView2.NavigateWithWebResourceRequest(webResourceRequest);
        }

        private void webView_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            if (!(e.Uri.ToString().Equals("http://127.0.0.1:5000/", StringComparison.InvariantCultureIgnoreCase)))
            {
                e.Cancel = true;
                System.Diagnostics.Process.Start(e.Uri.ToString());
            }
        }

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
    }
}