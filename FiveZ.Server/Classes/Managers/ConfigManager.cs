using CitizenFX.Core.Native;
using Newtonsoft.Json;
using FiveZ.Shared.Models.Configs;
using System.Collections.Generic;

namespace FiveZ.Server.Classes.Managers
{
    public class ConfigManager
    {

        public static readonly string DBPath = $"{API.GetResourcePath(API.GetCurrentResourceName())}/data/database.db";
        public static CharacterConfig CharacterConfig = null;
        public static List<SpawningConfig> SpawningConfig = null;

        public ConfigManager()
        {
            string ResourceName = API.GetCurrentResourceName();
            CharacterConfig = JsonConvert.DeserializeObject<CharacterConfig>(API.LoadResourceFile(ResourceName, "/configs/characters.json"));
            SpawningConfig = JsonConvert.DeserializeObject<List<SpawningConfig>>(API.LoadResourceFile(ResourceName, "/configs/spawns.json"));
            Utils.WriteLine("ConfigManager Loaded");
        }
    }
}
