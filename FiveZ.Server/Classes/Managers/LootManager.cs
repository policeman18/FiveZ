using System;
using System.Collections.Generic;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using FiveZ.Server.Models;
using FiveZ.Shared.Models;
using System.Threading.Tasks;

namespace FiveZ.Server.Classes.Managers
{
    public class LootManager
    {
        public static List<CharacterInventoryItem> Items = new List<CharacterInventoryItem>();
        public static List<LootSpawnPoint> ItemSpawns = new List<LootSpawnPoint>();

        public LootManager()
        {
            string ResourceName = API.GetCurrentResourceName();

            // Loading Items
            string loadeditems = API.LoadResourceFile(ResourceName, $"/configs/items.json");
            Items = JsonConvert.DeserializeObject<List<CharacterInventoryItem>>(loadeditems);

            // Loading Item Spawns
            string loadedspawns = API.LoadResourceFile(ResourceName, $"/configs/itemspawns.json");
            ItemSpawns = JsonConvert.DeserializeObject<List<LootSpawnPoint>>(loadedspawns);

            // Populating item spawns
            PopulateItemSpawns();
        }

        public async void PopulateItemSpawns()
        {
            foreach (LootSpawnPoint spawn in ItemSpawns)
            {
                CharacterInventoryItem allowedItem = null;
                allowedItem = Items[new Random().Next(0, Items.Count - 1)];
                do
                {
                    allowedItem = Items[new Random().Next(0, Items.Count - 1)];
                } while (!isLevelAllowed(allowedItem.Level, spawn.AllowedLevels));
                spawn.SetLootItem(allowedItem);
            }
        }

        public bool isLevelAllowed(int _level, int[] _levels)
        {
            for (int a = 0; a < _levels.Length; a++)
            {
                if (_levels[a] == _level) return true;
            }
            return false;
        }
    }
}