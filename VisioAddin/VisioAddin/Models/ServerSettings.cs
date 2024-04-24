using System.Collections.Generic;

namespace VisioAddin.Models
{
    internal class ServerSettings
    {
        public List<Server> Servers { get; set; }
        public string CurrentServer { get; set; }
    }
}