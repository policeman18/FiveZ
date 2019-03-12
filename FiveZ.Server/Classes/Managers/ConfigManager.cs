using CitizenFX.Core.Native;
using Newtonsoft.Json;
using FiveZ.Shared.Models.Configs;
using System.Collections.Generic;

namespace FiveZ.Server.Classes.Managers
{
    public class ConfigManager
    {

        // Database Path
        public static readonly string DBPath = $"{API.GetResourcePath(API.GetCurrentResourceName())}/data/database.db";

        // Config Variables
        public static ServerConfig ServerConfig = null;
        public static CharacterConfig CharacterConfig = null;
        public static List<SpawningConfig> SpawningConfig = null;
        public static WeatherConfig WeatherConfig = null;
        public static Dictionary<string, int> ControlsConfig = null;

        public ConfigManager()
        {
            string ResourceName = API.GetCurrentResourceName();
            ServerConfig = JsonConvert.DeserializeObject<ServerConfig>(API.LoadResourceFile(ResourceName, "/configs/server.json"));
            CharacterConfig = JsonConvert.DeserializeObject<CharacterConfig>(API.LoadResourceFile(ResourceName, "/configs/characters.json"));
            SpawningConfig = JsonConvert.DeserializeObject<List<SpawningConfig>>(API.LoadResourceFile(ResourceName, "/configs/playerspawns.json"));
            WeatherConfig = JsonConvert.DeserializeObject<WeatherConfig>(API.LoadResourceFile(ResourceName, "/configs/weather.json"));
            ControlsConfig = JsonConvert.DeserializeObject<Dictionary<string, int>>(API.LoadResourceFile(ResourceName, "/configs/controls.json"));

            Utils.WriteLine("ConfigManager Loaded");
        }
    }
}
