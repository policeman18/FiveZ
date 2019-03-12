using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using FiveZ.Client.Models;
using FiveZ.Shared.Models;

namespace FiveZ.Client.Classes.Managers
{
    public class InventoryManager
    {

        // Item Actions
        private Dictionary<string, Delegate> ItemActions = new Dictionary<string, Delegate>()
        {
            ["HealPlayer"] = new Action<CharacterInventoryItem>((_item) => Actions.HealPlayer((int)_item.Data["HealAmount"])),
            ["BoundPlayer"] = new Action<CharacterInventoryItem>((_item) => Actions.BoundPlayer()),
            ["PlayerEat"] = new Action<CharacterInventoryItem>((_item) => Actions.PlayerEat((int)_item.Data["EatAmount"])),
            ["PlayerDrink"] = new Action<CharacterInventoryItem>((_item) => Actions.PlayerDrink((int)_item.Data["DrinkAmount"]))
        };

        // Weapon Animations
        private Dictionary<string, string[]> WeaponStoreAnimations = new Dictionary<string, string[]>()
        {
            ["gangster"] = new string[] { "reaction@intimidation@1h", "outro" },
            ["rifle"] = new string[] { "", "" },
        };
        private Dictionary<string, string[]> WeaponDrawAnimations = new Dictionary<string, string[]>()
        {
            ["gangster"] = new string[] { "reaction@intimidation@1h", "intro" },
            ["rifle"] = new string[] { "", "" },
        };

        // Weapon Variables
        CharacterInventoryItem EquippedWeapon = null;
        Entity WeaponSlotOne = null;
        Entity WeaponSlotTwo = null;

        public InventoryManager()
        {
            Main.GetInstance().RegisterTickHandler(InventoryTask);
            Utils.WriteLine("InventoryManager Loaded");
        }

        public void OpenInventory()
        {
            CharacterInventory inventory = SessionManager.PlayerSession.SpawnedCharacter.Inventory;
            CitizenFX.Core.UI.Screen.ShowNotification("~g~Inventory Open Trigger");
        }

        private async Task InventoryTask()
        {
            
            if (ConfigManager.ControlsConfig != null)
            {
                // Disable Controls
                Game.DisableControlThisFrame(1, (Control)ConfigManager.ControlsConfig["InventoryControl"]);

                // Inventory Open Control
                if (Game.IsDisabledControlJustPressed(1, (Control)ConfigManager.ControlsConfig["InventoryControl"]))
                {
                    OpenInventory();
                }

                // Weapon Selection Controls
                if (Game.IsControlJustPressed(1, Control.SelectWeaponUnarmed))
                {
                    bool weaponAvailable = SessionManager.PlayerSession.SpawnedCharacter.Inventory.PrimaryWeaponOne == null ? false : true;
                    if (EquippedWeapon != null)
                    {
                        // Holster
                        if (weaponAvailable)
                        {

                        }
                    }
                    else
                    {
                        // Draw
                        if (weaponAvailable)
                        {

                        }
                    }
                }
                else if (Game.IsControlJustPressed(1, Control.SelectWeaponMelee))
                {
                    bool weaponAvailable = SessionManager.PlayerSession.SpawnedCharacter.Inventory.PrimaryWeaponTwo == null ? false : true;
                    if (EquippedWeapon != null)
                    {
                        // Holster
                        if (weaponAvailable)
                        {

                        }
                    }
                    else
                    {
                        // Draw
                        if (weaponAvailable)
                        {

                        }
                    }
                }
                else if (Game.IsControlJustPressed(1, Control.SelectWeaponShotgun))
                {
                    bool weaponAvailable = SessionManager.PlayerSession.SpawnedCharacter.Inventory.SecondaryWeapon == null ? false : true;
                    if (EquippedWeapon != null)
                    {
                        await StoreWeapon(EquippedWeapon.Data["WeaponAnimType"].ToString(), EquippedWeapon);
                        if (weaponAvailable)
                        {
                            CharacterInventoryItem NewWeapon = SessionManager.PlayerSession.SpawnedCharacter.Inventory.SecondaryWeapon;
                            await DrawWeapon(NewWeapon.Data["WeaponAnimType"].ToString(), NewWeapon);
                        }
                    }
                    else
                    {
                        // Draw
                        if (weaponAvailable)
                        {
                            CharacterInventoryItem NewWeapon = SessionManager.PlayerSession.SpawnedCharacter.Inventory.SecondaryWeapon;
                            await DrawWeapon(NewWeapon.Data["WeaponAnimType"].ToString(), NewWeapon);
                        }
                    }
                }
                else if (Game.IsControlJustPressed(1, Control.SelectWeaponHeavy))
                {
                    bool weaponAvailable = SessionManager.PlayerSession.SpawnedCharacter.Inventory.MeleeWeapon == null ? false : true;
                    if (EquippedWeapon != null)
                    {
                        await StoreWeapon(EquippedWeapon.Data["WeaponAnimType"].ToString(), EquippedWeapon);
                        if (weaponAvailable)
                        {
                            CharacterInventoryItem NewWeapon = SessionManager.PlayerSession.SpawnedCharacter.Inventory.MeleeWeapon;
                            await DrawWeapon(NewWeapon.Data["WeaponAnimType"].ToString(), NewWeapon);
                        }
                    }
                    else
                    {
                        if (weaponAvailable)
                        {
                            CharacterInventoryItem NewWeapon = SessionManager.PlayerSession.SpawnedCharacter.Inventory.MeleeWeapon;
                            await DrawWeapon(NewWeapon.Data["WeaponAnimType"].ToString(), NewWeapon);
                        }
                    }
                }

                // ADD HOLSTER KEYBIND (maybe the H key?)
                // Code


                if (EquippedWeapon != null)
                {
                    WeaponHash CurrentHash = Game.Player.Character.Weapons.Current.Hash;
                    WeaponHash EquippedHash = (WeaponHash)Game.GenerateHash(EquippedWeapon.Data["WeaponModel"].ToString());
                    if (CurrentHash != EquippedHash)
                    {
                        Game.Player.Character.Weapons.Select(EquippedHash, true);
                    }
                }
            }

            await Task.FromResult(0);
        }

        // Draw Weapon
        private async Task DrawWeapon(string _animType, CharacterInventoryItem _weapon)
        {
            // Animation
            await Game.Player.Character.Task.PlayAnimation(WeaponDrawAnimations[_animType][0], WeaponDrawAnimations[_animType][1], 10f, 10f, 2750, AnimationFlags.None | AnimationFlags.AllowRotation | AnimationFlags.UpperBodyOnly, -10f);

            // Slots
            if (SessionManager.PlayerSession.SpawnedCharacter.Inventory.PrimaryWeaponOne == _weapon)
            {
                if (WeaponSlotOne != null)
                {
                    WeaponSlotOne.Delete();
                    WeaponSlotOne = null;
                }
            } else if (SessionManager.PlayerSession.SpawnedCharacter.Inventory.PrimaryWeaponTwo == _weapon)
            {
                if (WeaponSlotTwo != null)
                {
                    WeaponSlotTwo.Delete();
                    WeaponSlotTwo = null;
                }
            }
            
            // Weapon Mag
            CharacterInventoryItem Magazine = _weapon.Magazine;

            int ammoCount = 0;
            if (Magazine != null)
            {
                ammoCount = Convert.ToInt32(Magazine.Data["AmmoCount"]);
            }
            ammoCount = 1;


            // Weapon Hash
            WeaponHash givenWeapon = (WeaponHash)Game.GenerateHash(_weapon.Data["WeaponModel"].ToString());

            Game.Player.Character.Weapons.Give(givenWeapon, ammoCount, true, ammoCount == 0 ? false : true);
            // Attachments [ NONE ]

            EquippedWeapon = _weapon;
            Utils.WriteLine("WEAPON DRAWING");
            Utils.WriteLine($"Weapon Equipped: {EquippedWeapon.Name}");
        }

        // Holster Weapon
        private async Task StoreWeapon(string _animType, CharacterInventoryItem _weapon)
        {
            // Animation
            await Game.Player.Character.Task.PlayAnimation(WeaponStoreAnimations[_animType][0], WeaponStoreAnimations[_animType][1], 10f, 10f, 2750, AnimationFlags.None | AnimationFlags.AllowRotation | AnimationFlags.UpperBodyOnly, -10f);

            // Weapon Hash
            WeaponHash storingWeaponHash = (WeaponHash)Game.GenerateHash(_weapon.Data["WeaponModel"].ToString());

            // Ammo Left
            int ammoLeft = Game.Player.Character.Weapons[storingWeaponHash].AmmoInClip;

            // Removing Weapon
            Game.Player.Character.Weapons.Remove(storingWeaponHash);

            // Set Slots and Save Ammo
            if (SessionManager.PlayerSession.SpawnedCharacter.Inventory.PrimaryWeaponOne == _weapon)
            {
                // New Weapon Object
                Entity newShownObject = await World.CreateProp(new Model(storingWeaponHash), Game.Player.Character.Position, true, false);

                // Remove Player Collision
                newShownObject.SetNoCollision(Game.Player.Character, true);

                // Attach Weapon
                newShownObject.AttachTo(Game.Player.Character);

                WeaponSlotOne = newShownObject;
                if (_weapon.Magazine != null)
                {
                    SessionManager.PlayerSession.SpawnedCharacter.Inventory.PrimaryWeaponOne.Magazine.Data["AmmoCount"] = ammoLeft;
                }
            }
            else if (SessionManager.PlayerSession.SpawnedCharacter.Inventory.PrimaryWeaponTwo == _weapon)
            {
                // New Weapon Object
                Entity newShownObject = await World.CreateProp(new Model(storingWeaponHash), Game.Player.Character.Position, true, false);

                // Remove Player Collision
                newShownObject.SetNoCollision(Game.Player.Character, true);

                // Attach Weapon
                newShownObject.AttachTo(Game.Player.Character);

                WeaponSlotTwo = newShownObject;
                if (_weapon.Magazine != null)
                {
                    SessionManager.PlayerSession.SpawnedCharacter.Inventory.PrimaryWeaponTwo.Magazine.Data["AmmoCount"] = ammoLeft;
                }
            }
            else if (SessionManager.PlayerSession.SpawnedCharacter.Inventory.SecondaryWeapon == _weapon)
            {
                if (_weapon.Magazine != null)
                {
                    SessionManager.PlayerSession.SpawnedCharacter.Inventory.SecondaryWeapon.Magazine.Data["AmmoCount"] = ammoLeft;
                }
            }

            EquippedWeapon = null;
            Utils.WriteLine("WEAPON HOLSTERING");
        }
    }
}
