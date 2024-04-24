using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;
using Visio = Microsoft.Office.Interop.Visio;

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

            DataObject dataObj = new DataObject(Shape);
            MemoryStream stream = (MemoryStream)dataObj.GetData("Visio 15.0 Shapes");
            string shapeData = Convert.ToBase64String(stream.ToArray());

            string imagePath = Path.Combine(Path.GetTempPath(), "image.png");
            Shape.Export(imagePath);
            Image image;
            using (FileStream fileStream = new FileStream(imagePath, FileMode.Open))
            {
                image = Image.FromStream(fileStream);
            }
            File.Delete(imagePath);
            ImageConverter _imageConverter = new ImageConverter();
            byte[] paramFileStream = ImageToByte2(image);

            var formContent = new MultipartFormDataContent
            {
                {new StringContent(tbName.Text), "name"},
                {new StringContent(tbPrompt.Text), "prompt"},
                {new StringContent(tbKeywords.Text), "keywords"},
                {new StringContent(shapeData), "dataObject"},
                {new StreamContent(new MemoryStream(paramFileStream)), "image", "image.png"}
            };

            var myHttpClient = new HttpClient();
            if (Globals.ThisAddIn.ServerHandler.CurrentServerUrl.StartsWith("https"))
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            }

            var response = await myHttpClient.PostAsync(Globals.ThisAddIn.ServerHandler.CurrentServerUrl + "/addShape", formContent);
            string stringContent = await response.Content.ReadAsStringAsync();

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