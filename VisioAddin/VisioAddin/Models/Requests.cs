using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;

namespace VisioAddin.Models
{
    internal class OnlineShape
    {
        public String Name { get; set; }
        public String Prompt { get; set; }
        public String Keywords { get; set; }
        public String DataObject { get; set; }
    }

    internal class OnlineStencil
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Author { get; set; }
        public string Manager { get; set; }
        public string Company { get; set; }
        public string Language { get; set; }
        public string Categories { get; set; }
        public string Tags { get; set; }
        public string Comments { get; set; }
        public List<OnlineShape> Shapes { get; set; }

        public OnlineStencil()
        { 
            Shapes = new List<OnlineShape>();
        }
    }
}