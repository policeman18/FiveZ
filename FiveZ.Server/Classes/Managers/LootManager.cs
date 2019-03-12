using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using FiveZ.Server.Models;
using FiveZ.Shared.Enums;
using FiveZ.Shared.Models;

namespace FiveZ.Server.Classes.Managers
{
    public class LootManager
    {
        // Hardcoded Items
        public static Dictionary<string, CharacterInventoryItem> ItemList = new Dictionary<string, CharacterInventoryItem>()
        {
            // Food
            ["mre"] = new CharacterInventoryItem("Meal Ready to Eat", "Military MRE", DefinedItems.MRE, "mre.png", 0, false, new DefinedItems[] { }, "PlayerEat", new Dictionary<string, object>() {
                ["EatAmount"] = 50
            }),

            // Drink
            ["water"] = new CharacterInventoryItem("Water Bottle", "Bottle of Water", DefinedItems.WaterBottle, "waterbottle.png", 0, false, new DefinedItems[] { }, "PlayerDrink", new Dictionary<string, object>() {
                ["DrinkAmount"] = 25
            }),
            ["coke"] = new CharacterInventoryItem("CocaCola", "Can of Soda", DefinedItems.Coke, "coke.png", 0, false, new DefinedItems[] { }, "PlayerDrink", new Dictionary<string, object>()
            {
                ["DrinkAmount"] = 15
            }),

            // Bounding

            // Medical

            // Weapons
            ["pistol"] = new CharacterInventoryItem("Baretta M9", "Handgun", DefinedItems.Pistol, "pistol.png", 0, true, new DefinedItems[] { DefinedItems.PistolMag }, "", new Dictionary<string, object>()
            {
                ["WeaponType"] = "secondary",
                ["WeaponModel"] = "weapon_pistol",
                ["WeaponAnimType"] = "gangster"
            }),

            // Magazines
            ["pistolmag"] = new CharacterInventoryItem("Baretta M9 Mag", "Handgun Mag", DefinedItems.PistolMag, "pistolmag.png", 0, false, new DefinedItems[] { }, "", new Dictionary<string, object>()
            {
                ["MagType"] = "pistol",
                ["AmmoCount"] = 15
            })
        };

        // Loaded Items
        public static List<CharacterInventoryItem> Items = new List<CharacterInventoryItem>();

        public static Dictionary<int, LootSpawnPoint> ItemSpawns = new Dictionary<int, LootSpawnPoint>();

        public LootManager()
        {
            Main.GetInstance().RegisterEventHandler("FiveZ:PlayerPickupItem", new Action<Player, int>(PlayerPickupItem));
            Main.GetInstance().RegisterEventHandler("FiveZ:PlayerDropItem", new Action<Player>(PlayerDropItem));
            LoadLoot();

            Utils.WriteLine("LootManager Loaded");
        }

        public void LoadLoot()
        {
            string ResourceName = API.GetCurrentResourceName();

            // Loading Items
            Dictionary<string, CharacterInventoryItem> ModifiedItems = JsonConvert.DeserializeObject<Dictionary<string, CharacterInventoryItem>>(API.LoadResourceFile(ResourceName, $"/configs/items.json"));
            foreach(KeyValuePair<string, CharacterInventoryItem> item in ModifiedItems)
            {
                ItemList[item.Key].UpdateItem(item.Value.Name, item.Value.Description, item.Value.Icon, item.Value.Level);
                Items.Add(ItemList[item.Key]);
            }

            // Loading Item Spawns
            string loadedspawns = API.LoadResourceFile(ResourceName, $"/configs/itemspawns.json");
            ItemSpawns = JsonConvert.DeserializeObject<Dictionary<int, LootSpawnPoint>>(loadedspawns);

            // Populating item spawns
            PopulateItemSpawns();
        }

        private void PopulateItemSpawns()
        {
            foreach (KeyValuePair<int, LootSpawnPoint> spawn in ItemSpawns)
            {
                CharacterInventoryItem allowedItem = null;
                int _attempts = 0;
                allowedItem = Items[Items.Count == 0 ? 0 : new Random(DateTime.Now.Millisecond).Next(0, Items.Count)];
                do
                {
                    allowedItem = Items[Items.Count == 0 ? 0 : new Random(DateTime.Now.Millisecond).Next(0, Items.Count)];
                    _attempts += 1;
                } while (!isLevelAllowed(allowedItem.Level, spawn.Value.AllowedLevels) && _attempts <= 20);
                ItemSpawns[spawn.Key].SetLootItem(allowedItem);
            }
        }

        private void PlayerPickupItem([FromSource] Player _player, int _itemIndex)
        {
            try
            {
                Utils.WriteLine($"Item Index Grabbed: {ItemSpawns[_itemIndex].Loot.Name}");
                ItemSpawns[_itemIndex].PickupItem(() =>
                {
                    foreach (Session s in SessionManager.Sessions)
                    {
                        s.Player.TriggerEvent("FiveZ:RecieveLootSpawns", JsonConvert.SerializeObject(ItemSpawns));
                    }
                });
            }
            catch (Exception ex)
            {
                Utils.WriteLine($"Index Passed: {_itemIndex}");
                Utils.Throw(ex);
            }
        }

        // Not Implemented
        private void PlayerDropItem([FromSource] Player _player)
        {

        }

        private bool isLevelAllowed(int _level, int[] _levels)
        {
            for (int a = 0; a < _levels.Length; a++)
            {
                if (_levels[a] == _level) return true;
            }
            return false;
        }
    }
}