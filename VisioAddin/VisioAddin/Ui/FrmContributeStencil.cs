using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;
using Visio = Microsoft.Office.Interop.Visio;
using System.Text;
using VisioAddin.Models;
using System.Runtime.InteropServices.ComTypes;

namespace VisioAddin.Ui
{
    public partial class FrmContributeStencil : Form
    {
        public FrmContributeStencil()
        {
            InitializeComponent();

            foreach (Visio.Document doc in Globals.ThisAddIn.Application.Documents)
            {
                if (doc.Type == Visio.VisDocumentTypes.visTypeStencil)
                {
                    lbStencils.Items.Add(doc.Name);
                }
            }
        }

        private async void btnContribute_Click(object sender, EventArgs e)
        {
            Visio.Document stencil = null;

            foreach (Visio.Document doc in Globals.ThisAddIn.Application.Documents)
            {
                if (doc.Type == Visio.VisDocumentTypes.visTypeStencil)
                {
                    if (doc.Name == lbStencils.SelectedItem.ToString())
                    {
                        stencil = doc;
                        break;
                    }
                }
            }

            if (stencil == null) return;

            var content = new MultipartFormDataContent();
            var stream = File.OpenRead(stencil.FullName);
            var streamContent = new StreamContent(stream);
            content.Add(streamContent, "stencil", stencil.Name);

            Models.OnlineStencil onlineStencil = new Models.OnlineStencil();
            onlineStencil.FileName = stencil.Name;
            onlineStencil.Title = stencil.Title;
            onlineStencil.Subject = stencil.Subject;
            onlineStencil.Author = stencil.Creator;
            onlineStencil.Manager = stencil.Manager;
            onlineStencil.Company = stencil.Company;
            onlineStencil.Language = stencil.Language.ToString();
            onlineStencil.Categories = stencil.Category;
            onlineStencil.Tags = stencil.Keywords;
            onlineStencil.Comments = stencil.Description;

            int i = 0;

            foreach (Visio.Master master in stencil.Masters)
            {
                if (master.Shapes.Count > 1)
                {
                    Visio.Selection selection = master.CreateSelection(Visio.VisSelectionTypes.visSelTypeAll);
                    selection.Group();
                }

                DataObject dataObject = new DataObject(master.Shapes[1]);
                Dictionary<string, string> dictDataObject = new Dictionary<string, string>();

                foreach (string format in dataObject.GetFormats(false))
                {
                    MemoryStream streamData;

                    if (format == "Visio 15.0 Shapes")
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

                Models.OnlineShape onlineShape = new Models.OnlineShape();
                onlineShape.Name = master.Name;
                onlineShape.Prompt = master.Prompt;
                onlineShape.Keywords = master.PageSheet.CellsSRC[(short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowMisc, (short)Visio.VisCellIndices.visObjKeywords].ResultStr[""];
                onlineShape.DataObject = JsonConvert.SerializeObject(dictDataObject);
                onlineStencil.Shapes.Add(onlineShape);

                string imagePath = Path.Combine(Path.GetTempPath(), "image.png");
                master.Export(imagePath);
                Image image;
                using (FileStream fileStream = new FileStream(imagePath, FileMode.Open))
                {
                    image = Image.FromStream(fileStream);
                }
                File.Delete(imagePath);
                byte[] paramFileStream = ImageToByte2(image);
                MemoryStream memoryStream = new MemoryStream(paramFileStream);
                streamContent = new StreamContent(memoryStream);

                i++;
                content.Add(streamContent, "images", i.ToString() + ".png");
            }

            string json = JsonConvert.SerializeObject(onlineStencil);

            StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            content.Add(stringContent, "json");

            var httpClient = new HttpClient();
            string token = Globals.ThisAddIn.ServerHandler.CurrentServerToken;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            if (Globals.ThisAddIn.ServerHandler.CurrentServerUrl.StartsWith("https"))
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            }

            var response = await httpClient.PostAsync(Globals.ThisAddIn.ServerHandler.CurrentServerUrl + "/add_stencil", content);
            string strResponse = await response.Content.ReadAsStringAsync();
            MessageBox.Show(strResponse);
            if (strResponse == "Failed")
            {
                MessageBox.Show("Upload failed, check creditals.");
                return;
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lbStencils_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbStencils.SelectedItems.Count > 0)
            {
                btnContribute.Enabled = true;
            }
            else
            {
                btnContribute.Enabled = false;
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
    }
}
