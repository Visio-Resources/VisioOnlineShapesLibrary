using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace VisioAddin.Handlers
{
    internal class SettingsHandler
    {
        public Models.ServerSettings ServerSettings { get; set; }

        public SettingsHandler()
        {
            string json = Properties.Settings.Default.ServerSettings;

            if (json != "")
            {
                ServerSettings = JsonConvert.DeserializeObject<Models.ServerSettings>(json);
            }
            else
            {
                ServerSettings = new Models.ServerSettings();
                ServerSettings.Servers = new List<Models.Server>();

                Models.Server server = new Models.Server
                {
                    Name = "www.visio-shapes.com",
                    Url = "https://www.visio-shapes.com",
                    User = "",
                    Password = ""
                };
                ServerSettings.Servers.Add(server);

                server = new Models.Server
                {
                    Name = "localhost",
                    Url = "http://127.0.0.1:5000",
                    User = "",
                    Password = ""
                };
                ServerSettings.Servers.Add(server);

                ServerSettings.CurrentServer = "www.visio-shapes.com";
            }
        }

        public string CurrentServer
        {
            get
            {
                return ServerSettings.CurrentServer;
            }
            set
            {
                ServerSettings.CurrentServer = value;
                Save();
            }
        }

        public string CurrentServerUrl
        {
            get
            {
                if (ServerSettings.Servers.Count == 0) return "https://www.visio-shapes.com";
                var server = ServerSettings.Servers.Where(s => s.Name == this.CurrentServer).FirstOrDefault();
                if (server == null) return "https://www.visio-shapes.com";
                return server.Url;
            }
        }

        public List<Models.Server> Servers
        {
            get
            {
                return ServerSettings.Servers;
            }
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(ServerSettings);
            Properties.Settings.Default.ServerSettings = json;
            Properties.Settings.Default.Save();
        }
    }
}