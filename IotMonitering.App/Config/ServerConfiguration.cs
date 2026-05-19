namespace IoTMonitoring.Config
{
    public class ServerConfig
    {
        public int Port { get; set; }
    }

    public class HubConfig : ServerConfig
    {
        public string Route { get; set; } = "Hub";
    }
    public class ServerConfiguration
    {
        public ServerConfig RestApi { get; set; } = new ServerConfig { Port = 5000 };
        public ServerConfig Tcp { get; set; } = new ServerConfig { Port = 6000 };
        public ServerConfig Udp { get; set; } = new ServerConfig { Port = 6001 };
        public HubConfig SignalR { get; set; } = new HubConfig { Port = 5001};
        public ServerConfig Grpc { get; set; } = new ServerConfig { Port = 5002 };
    }
}
