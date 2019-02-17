using Newtonsoft.Json;

namespace FiveZ.Shared.Models.Configs
{
    public class WeatherConfig
    {
        [JsonProperty]
        public int WeatherSwitchTime { get; protected set; }
    }
}
