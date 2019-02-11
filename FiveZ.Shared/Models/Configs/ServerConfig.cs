using Newtonsoft.Json;

namespace FiveZ.Shared.Models.Configs
{
    public class ServerConfig
    {
        [JsonProperty]
        public bool ServerWhitelisted { get; protected set; }
        [JsonProperty]
        public int ServerConnectionDelayTime { get; protected set; }
    }
}
