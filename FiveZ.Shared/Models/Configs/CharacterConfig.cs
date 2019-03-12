using Newtonsoft.Json;

namespace FiveZ.Shared.Models.Configs
{
    public class CharacterConfig
    {
        [JsonProperty]
        public int MaxCharacters { get; protected set; }
        [JsonProperty]
        public object StarterInventory { get; protected set; }
        [JsonProperty]
        public float[] PrimaryWeaponOnePosition { get; protected set; }
        [JsonProperty]
        public float[] PrimaryWeaponTwoPosition { get; protected set; }
        [JsonProperty]
        public int StarterInventorySize { get; protected set; }
    }
}
