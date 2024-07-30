using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;
using Visio = Microsoft.Office.Interop.Visio;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace VisioAddin.Ui
{
    public partial class FrmContributeShape : Form
    {
        private Visio.Shape Shape;
        private static readonly HttpClient client = new HttpClient();

        public FrmContributeShape(Visio.Shape shape)
        {
            InitializeComponent();
            Shape = shape;
            Visio.Master master = shape.Master;

            tbName.Text = shape.Name;

            if (master != null)
            {
                tbPrompt.Text = master.Prompt;
                tbKeywords.Text = master.PageSheet.CellsSRC[(short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowMisc, (short)Visio.VisCellIndices.visObjKeywords].ResultStr[""];
            }
        }

        public async void Submit()
        {
            if (tbName.Text == "")
            {
                MessageBox.Show("No name defined.", "Contribute Shape");
                return;
            }

            DataObject dataObject = new DataObject(Shape);
            Dictionary<string, string> dictDataObject = new Dictionary<string, string>();
            foreach (string format in dataObject.GetFormats(false))
            {
                MemoryStream streamData = null;

                // -: is null
                // + "Visio 15.0 Shapes"
                // - "Visio 15.0 Text"
                // - "Visio 11.0 Shapes"
                // + "UnicodeText"
                // - "Embed Source"
                // + "Link Source"
                // + "Object Descriptor"
                // - "Link Source Descriptor"
                // - "NoteshNote"
                // - "NotesDocAction"
                // - "EnhancedMetafile"
                // - "MetaFilePict"
                // + "Text"
                // + "DeviceIndependentBitmap"

                if (format == "UnicodeText")
                {
                    //byte[] byteArray = Encoding.UTF8.GetBytes((string)dataObject.GetData(format));
                    //streamData = new MemoryStream(byteArray);
                }
                else if (format == "Text")
                {
                    //byte[] byteArray = Encoding.ASCII.GetBytes((string)dataObject.GetData(format));
                    //streamData = new MemoryStream(byteArray);
                }
                else if (format == "Visio 15.0 Shapes")
                {
                    streamData = (MemoryStream)dataObject.GetData(format);
                }
                else if (format == "Object Descriptor")
                {
                    streamData = (MemoryStream)dataObject.GetData(format);
                }
                else
                {
                    continue;
                }

                if (streamData is null) continue;

                dictDataObject[format] = Convert.ToBase64String(streamData.ToArray());
            }

            string imagePath = Path.Combine(Path.GetTempPath(), "image.png");
            Shape.Export(imagePath);
            Image image;
            using (FileStream fileStream = new FileStream(imagePath, FileMode.Open))
            {
                image = Image.FromStream(fileStream);
            }
            File.Delete(imagePath);
            byte[] paramFileStream = ImageToByte2(image);

            Models.OnlineShape onlineShape = new Models.OnlineShape();

            onlineShape.Name = tbName.Text;
            onlineShape.Prompt = tbPrompt.Text;
            onlineShape.Keywords = tbKeywords.Text;
            onlineShape.DataObject = JsonConvert.SerializeObject(dictDataObject);

            string json = JsonConvert.SerializeObject(onlineShape);

            var content = new MultipartFormDataContent()
            {
                {new StringContent(json, Encoding.UTF8, "application/json"), "json"},
                {new StreamContent(new MemoryStream(paramFileStream)), "image", "image.png"}
            };

            var httpClient = new HttpClient();
            string token = Globals.ThisAddIn.ServerHandler.CurrentServerToken;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            if (Globals.ThisAddIn.ServerHandler.CurrentServerUrl.StartsWith("https"))
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            }

            var response = await httpClient.PostAsync(Globals.ThisAddIn.ServerHandler.CurrentServerUrl + "/add_shape", content);
            string strResponse = await response.Content.ReadAsStringAsync();
            MessageBox.Show(strResponse);
            if (strResponse == "Failed")
            {
                MessageBox.Show("Upload failed, check creditals.");
                return;
            }

            if (Globals.ThisAddIn._panelManager.IsPanelOpened(Globals.ThisAddIn.Application.ActiveWindow))
            {
                Globals.ThisAddIn.RaiseEventOnContribute(tbName.Text);
            }
        }

        public static byte[] ImageToByte2(Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        private void btnContribute_Click(object sender, EventArgs e)
        {
            Submit();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}