using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using FiveZ.Client.Menus;
using FiveZ.Shared.Models;
using System.Dynamic;

namespace FiveZ.Client.Classes.Managers
{
    public class SpawnManager
    {
        public SpawnManager()
        {
            // Events
            Main.GetInstance().RegisterEventHandler("FiveZ:HandlePlayerSpawn", new Action<string>(HandlePlayerSpawn));

            // Commands
            API.RegisterCommand("revive", new Action<int, List<object>, string>(Revive), false);

            // Disable Respawning
            DisableAutoRespawning();
        }

        public async void DisableAutoRespawning()
        {
            await BaseScript.Delay(5000);
            Main.GetInstance().CallExport()["spawnmanager"].setAutoSpawn(false);
        }

        // Remove Later When Doing The Revive / Medical System
        public void Revive(int _source, List<object> _args, string _raw)
        {
            if (Game.Player.Character.IsDead)
            {
                Game.Player.Character.Resurrect();
            }
        }

        public async void HandlePlayerSpawn(string _character)
        {
            Character chardata = JsonConvert.DeserializeObject<Character>(_character);
            dynamic spawnData = new ExpandoObject();
            if (chardata.isNew)
            {
                CharacterModifier.EnableCharacterModifier(chardata.Gender);
                spawnData.x = 403.009f;
                spawnData.y = -996.653f;
                spawnData.z = -99.0003f;
                if (chardata.Gender == Shared.Enums.Genders.Male)
                {
                    spawnData.model = API.GetHashKey("mp_m_freemode_01");
                }
                else
                {
                    spawnData.model = API.GetHashKey("mp_f_freemode_01");
                }
                Main.GetInstance().CallExport()["spawnmanager"].spawnPlayer(spawnData);
                await BaseScript.Delay(1000);
                Game.Player.Character.Heading = 179.8418f;
                API.SetPedDefaultComponentVariation(Game.Player.Character.Handle);
                World.RenderingCamera = null;
                Main.GetInstance().SetNuiFocus(false, false);
            }
            else
            {
                spawnData.x = chardata.LastPos[0];
                spawnData.y = chardata.LastPos[1];
                spawnData.z = chardata.LastPos[2];
                if (chardata.Gender == Shared.Enums.Genders.Male)
                {
                    spawnData.model = API.GetHashKey("mp_m_freemode_01");
                }
                else
                {
                    spawnData.model = API.GetHashKey("mp_f_freemode_01");
                }
                Main.GetInstance().CallExport()["spawnmanager"].spawnPlayer(spawnData);

                World.RenderingCamera = null;
                Main.GetInstance().SetNuiFocus(false, false);
                await BaseScript.Delay(300);
                Game.Player.Character.Style[PedComponents.Head].SetVariation(23, 1);
                Game.Player.Character.IsVisible = true;
                Game.Player.Character.IsInvincible = false;
                // Set Player Looks
                // Set Player Inventory
                // Start Player Skills
            }
        }
    }
}
