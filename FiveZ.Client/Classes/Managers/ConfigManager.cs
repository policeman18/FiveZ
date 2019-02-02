using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using FiveZ.Shared.Models.Configs;

namespace FiveZ.Client.Classes.Managers
{
    public class ConfigManager
    {
        public static List<SpawningConfig> SpawningConfig = null;

        public ConfigManager()
        {
            Main.GetInstance().RegisterEventHandler("FiveZ:SendClientConfigs", new Action<string, string>(SetConfigs));
        }

        private void SetConfigs(string _type, string _config)
        {
            switch (_type)
            {
                case "spawns":
                    SpawningConfig = JsonConvert.DeserializeObject<List<SpawningConfig>>(_config);
                    Utils.WriteLine(_config);
                    break;
                default:
                    Utils.WriteLine($"Couldn't Load Config {_type}");
                    break;
            }
        }
    }
}
