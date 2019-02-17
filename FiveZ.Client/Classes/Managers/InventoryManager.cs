using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FiveZ.Shared.Models;
using FiveZ.Shared.Enums;

namespace FiveZ.Client.Classes.Managers
{
    public class InventoryManager
    {
        private Dictionary<string, Delegate> ItemActions = new Dictionary<string, Delegate>()
        {
            ["HealPlayer"] = new Action<CharacterInventoryItem>((_item) => Actions.HealPlayer((int)_item.Data["HealAmount"])),
            ["BoundPlayer"] = new Action<CharacterInventoryItem>((_item) => Actions.BoundPlayer()),
            ["PlayerEat"] = new Action<CharacterInventoryItem>((_item) => Actions.PlayerEat((int)_item.Data["EatAmount"])),
            ["PlayerDrink"] = new Action<CharacterInventoryItem>((_item) => Actions.PlayerDrink((int)_item.Data["DrinkAmount"]))
        };
    }
}
