using System;
using Newtonsoft.Json;

namespace FiveZ.Shared.Models
{
    public class LootSpawnPoint
    {
        [JsonProperty]
        public float X { get; protected set; }
        [JsonProperty]
        public float Y { get; protected set; }
        [JsonProperty]
        public float Z { get; protected set; }
        [JsonProperty]
        public int[] AllowedLevels { get; protected set; }
        [JsonProperty]
        public bool isAvailable { get; protected set; } = false;
        [JsonProperty]
        public CharacterInventoryItem Loot { get; protected set; } = null;

        public void SetLootItem(CharacterInventoryItem _item)
        {
            this.Loot = _item;
            this.isAvailable = true;
        }

        public void PickupItem(Action _action)
        {
            this.isAvailable = false;
            _action.Invoke();
        }
    }
}
