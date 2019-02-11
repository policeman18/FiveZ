using Newtonsoft.Json;

namespace FiveZ.Shared.Models.Configs
{
    public class SpawningConfig
    {
        [JsonProperty]
        public string SpawnLabel { get; protected set; }
        [JsonProperty]
        public float X { get; protected set; }
        [JsonProperty]
        public float Y { get; protected set; }
        [JsonProperty]
        public float Z { get; protected set; }
        [JsonProperty]
        public float H { get; protected set; }
    }
}
