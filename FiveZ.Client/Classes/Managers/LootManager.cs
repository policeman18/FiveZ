using System;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Newtonsoft.Json;
using FiveZ.Shared.Models;

namespace FiveZ.Client.Classes.Managers
{
    public class LootManager
    {

        // ANIM DICT - pickup_object
        // ANIM NAMES - pickup_low | putdown_low

        private readonly int CooldownTime = 250;
        private bool CooldownActive = false;
        private int GrabTimestamp = 0;
        private Dictionary<int, LootSpawnPoint> ItemSpawns = new Dictionary<int, LootSpawnPoint>();

        public LootManager()
        {
            Main.GetInstance().RegisterEventHandler("FiveZ:RecieveLootSpawns", new Action<string>((spawns) => {
                ItemSpawns = JsonConvert.DeserializeObject<Dictionary<int, LootSpawnPoint>>(spawns);
            }));
            API.RegisterCommand("pos", new Action<int, List<object>, string>(GetPosition), false);
            Main.GetInstance().RegisterTickHandler(DisplayLoot);

            Utils.WriteLine("LootManager Loaded");
        }

        public void GetPosition(int _source, List<object> args, string _raw)
        {
            Utils.WriteLine($"Position: {Game.Player.Character.Position.ToString()}");
        }

        private async Task DisplayLoot()
        {

            //int activeCount = 0;
            //foreach (KeyValuePair<int, LootSpawnPoint> point in ItemSpawns)
            //{
            //    if (point.Value.isAvailable)
            //    {
            //        activeCount += 1;
            //    }
            //}

            //Screen.ShowSubtitle($"Displaying {activeCount} items");

            // Reset Cooldown
            if (CooldownActive)
            {
                if (Game.GameTime >= GrabTimestamp)
                {
                    CooldownActive = false;
                }
            }

            // Displaying Items
            foreach (KeyValuePair<int, LootSpawnPoint> loot in ItemSpawns)
            {
                LootSpawnPoint point = loot.Value;
                Vector3 MarkerPosition = new Vector3(point.X, point.Y, point.Z - 0.95f);
                Vector3 TextPosition = new Vector3(point.X, point.Y, point.Z - 0.50f);
                if (point.isAvailable)
                {
                    float playerDistance = Game.Player.Character.Position.DistanceToSquared(MarkerPosition);
                    if (playerDistance < 150f)
                    {
                        World.DrawMarker(MarkerType.HorizontalCircleSkinny, MarkerPosition, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(1f, 1f, 1f), Color.FromArgb(255, 0, 0), false, false, false, null, null, false);
                        PointF ScreenPos = Screen.WorldToScreen(MarkerPosition, true);
                        float scrWCenter = Screen.Width / 2;
                        float scrXCenterDistance = ScreenPos.X - scrWCenter;
                        float scrHCenter = Screen.Height / 2;
                        float scrYCenterDistance = ScreenPos.Y - scrHCenter;
                        float distanceAllowed = 100f;
                        if (playerDistance < 2.2f)
                        {
                            if (scrXCenterDistance < distanceAllowed && scrXCenterDistance > -distanceAllowed && scrYCenterDistance < distanceAllowed && scrYCenterDistance > -distanceAllowed)
                            {
                                Helpers.Draw3DText(TextPosition, Color.FromArgb(255, 255, 255), point.Loot.Name.ToUpper(), Font.ChaletComprimeCologne, 0.07f, true);
                                Screen.DisplayHelpTextThisFrame($"Press ~INPUT_PICKUP~ to pickup");
                                if (Game.IsControlJustPressed(0, Control.Pickup) && !CooldownActive)
                                {
                                    CooldownActive = true;
                                    GrabTimestamp = Game.GameTime + CooldownTime;
                                    if (point.Loot.IsWeapon)
                                    {
                                        PickupWeapon(point.Loot, loot.Key);
                                    }
                                    else
                                    {
                                        PickupItem(point.Loot, loot.Key);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            await Task.FromResult(0);
        }

        // Convert code into these methods instead of hardcode in the thread
        private void PickupWeapon(CharacterInventoryItem _item, int _index)
        {
            if (_item.Data["WeaponType"] != null)
            {
                switch (_item.Data["WeaponType"].ToString())
                {
                    case "primary":
                        bool foundSlot = false;
                        SessionManager.PlayerSession.SpawnedCharacter.Inventory.SetPrimaryWeaponOne(_item, new Action<bool>(async (canPickup) =>
                        {
                            if (canPickup && !foundSlot)
                            {
                                foundSlot = true;
                                await Game.Player.Character.Task.PlayAnimation("pickup_object", "pickup_low", 8f, -8f, -1, AnimationFlags.None, -8f);
                                Main.TriggerServerEvent("FiveZ:PlayerPickupItem", _index);
                            }
                        }));

                        SessionManager.PlayerSession.SpawnedCharacter.Inventory.SetPrimaryWeaponTwo(_item, new Action<bool>(async (canPickup) =>
                        {
                            if (canPickup && !foundSlot)
                            {
                                foundSlot = true;
                                await Game.Player.Character.Task.PlayAnimation("pickup_object", "pickup_low", 8f, -8f, -1, AnimationFlags.None, -8f);
                                Main.TriggerServerEvent("FiveZ:PlayerPickupItem", _index);
                            }
                        }));
                        
                        if (foundSlot)
                        {
                            Screen.ShowNotification($"~g~Picked up item: ~w~{_item.Name}");
                        }
                        else
                        {
                            Screen.ShowNotification($"~r~Can't pickup item: ~w~{_item.Name}");
                        }
                        break;
                    case "secondary":
                        SessionManager.PlayerSession.SpawnedCharacter.Inventory.SetSecondaryWeapon(_item, new Action<bool>(async (canPickup) =>
                        {
                            if (canPickup)
                            {
                                await Game.Player.Character.Task.PlayAnimation("pickup_object", "pickup_low", 8f, -8f, -1, AnimationFlags.None, -8f);
                                Main.TriggerServerEvent("FiveZ:PlayerPickupItem", _index);
                                Screen.ShowNotification($"~g~Picked up item: ~w~{_item.Name}");
                            }
                            else
                            {
                                Screen.ShowNotification($"~r~Can't pickup item: ~w~{_item.Name}");
                            }
                        }));
                        break;
                    case "melee":
                        SessionManager.PlayerSession.SpawnedCharacter.Inventory.SetMeleeWeapon(_item, new Action<bool>(async (canPickup) =>
                        {
                            if (canPickup)
                            {
                                await Game.Player.Character.Task.PlayAnimation("pickup_object", "pickup_low", 8f, -8f, -1, AnimationFlags.None, -8f);
                                Main.TriggerServerEvent("FiveZ:PlayerPickupItem", _index);
                                Screen.ShowNotification($"~g~Picked up item: ~w~{_item.Name}");
                            }
                            else
                            {
                                Screen.ShowNotification($"~r~Can't pickup item: ~w~{_item.Name}");
                            }
                        }));
                        break;
                    default:
                        Utils.WriteLine($"Couldn't Find Slot For Weapon Type: {_item.Data["WeaponType"]}");
                        break;
                }
            }
        }

        private void PickupItem(CharacterInventoryItem _item, int _index)
        {
            SessionManager.PlayerSession.SpawnedCharacter.Inventory.AddItem(_item, new Action<bool>(async (canPickup) =>
            {
                Utils.WriteLine($"Can Pickup: {canPickup}");
                if (canPickup)
                {
                    await Game.Player.Character.Task.PlayAnimation("pickup_object", "pickup_low", 8f, -8f, -1, AnimationFlags.None, -8f);
                    Main.TriggerServerEvent("FiveZ:PlayerPickupItem", _index);
                    Screen.ShowNotification($"~g~Picked up item: ~w~{_item.Name}");
                }
                else
                {
                    Screen.ShowNotification($"~r~Can't pickup item: ~w~{_item.Name}");
                }
            }));
        }
    }
}
