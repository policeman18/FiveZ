using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using FiveZ.Client.Menus;
using FiveZ.Client.Models;
using FiveZ.Shared.Models;
using FiveZ.Shared.Models.Configs;

namespace FiveZ.Client.Classes.Managers
{
    public class SpawnManager
    {
        public SpawnManager()
        {
            try
            {
                // Events
                Main.GetInstance().RegisterEventHandler("FiveZ:HandlePlayerSpawn", new Action<string>(HandlePlayerSpawn));

                // Commands
                API.RegisterCommand("revive", new Action<int, List<object>, string>(Revive), false);

                // Disable Respawning
                DisableAutoRespawning();

                Utils.WriteLine("SpawnManager Loaded");
            }
            catch(Exception ex)
            {
                Utils.Throw(ex);
            }
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
            await Game.Player.ChangeModel(new Model(chardata.Model));
            Vector3 SpawnPosition = new Vector3();
            float SpawnHeading = 0f;

            // Set Clothing
            Game.Player.Character.Style[PedComponents.Hair].SetVariation(chardata.Appearance.HairStyle, 0);
            Game.Player.Character.Style[PedComponents.Torso].SetVariation(chardata.Clothing[Convert.ToInt32(PedComponents.Torso)][0], chardata.Clothing[Convert.ToInt32(PedComponents.Torso)][1]);
            Game.Player.Character.Style[PedComponents.Legs].SetVariation(chardata.Clothing[Convert.ToInt32(PedComponents.Legs)][0], chardata.Clothing[Convert.ToInt32(PedComponents.Legs)][1]);
            Game.Player.Character.Style[PedComponents.Hands].SetVariation(chardata.Clothing[Convert.ToInt32(PedComponents.Hands)][0], chardata.Clothing[Convert.ToInt32(PedComponents.Hands)][1]);
            Game.Player.Character.Style[PedComponents.Shoes].SetVariation(chardata.Clothing[Convert.ToInt32(PedComponents.Shoes)][0], chardata.Clothing[Convert.ToInt32(PedComponents.Shoes)][1]);
            Game.Player.Character.Style[PedComponents.Special1].SetVariation(chardata.Clothing[Convert.ToInt32(PedComponents.Special1)][0], chardata.Clothing[Convert.ToInt32(PedComponents.Special1)][1]);
            Game.Player.Character.Style[PedComponents.Special2].SetVariation(chardata.Clothing[Convert.ToInt32(PedComponents.Special2)][0], chardata.Clothing[Convert.ToInt32(PedComponents.Special2)][1]);
            Game.Player.Character.Style[PedComponents.Special3].SetVariation(chardata.Clothing[Convert.ToInt32(PedComponents.Special3)][0], chardata.Clothing[Convert.ToInt32(PedComponents.Special3)][1]);
            Game.Player.Character.Style[PedComponents.Textures].SetVariation(chardata.Clothing[Convert.ToInt32(PedComponents.Textures)][0], chardata.Clothing[Convert.ToInt32(PedComponents.Textures)][1]);
            Game.Player.Character.Style[PedComponents.Torso2].SetVariation(chardata.Clothing[Convert.ToInt32(PedComponents.Torso2)][0], chardata.Clothing[Convert.ToInt32(PedComponents.Torso2)][1]);

            // Set Parent Blend
            API.SetPedHeadBlendData(Game.Player.Character.Handle, chardata.Parents.Father, chardata.Parents.Mother, 0, chardata.Parents.Father, chardata.Parents.Mother, 0, chardata.Parents.Mix, chardata.Parents.Mix, -1, false);
            
            // Set Face Features
            for (int a = 0; a < chardata.FaceFeatures.Count - 1; a++)
            {
                API.SetPedFaceFeature(Game.Player.Character.Handle, a, chardata.FaceFeatures[a]);
            }

            // Set Overlays
            for (int a = 0; a < chardata.Appearance.Overlays.Count - 1; a++)
            {
                API.SetPedHeadOverlay(Game.Player.Character.Handle, a, chardata.Appearance.Overlays[a].Index, chardata.Appearance.Overlays[a].Opacity);
                if (chardata.Appearance.Overlays[a].IsHair)
                {
                    API.SetPedHeadOverlayColor(Game.Player.Character.Handle, a, 1, chardata.Appearance.HairColor, chardata.Appearance.HairHighlightColor);
                }
            }

            // Set Hair Colors
            API.SetPedHairColor(Game.Player.Character.Handle, chardata.Appearance.HairColor, chardata.Appearance.HairHighlightColor);

            if (chardata.isNew)
            {
                SpawnPosition.X = 403.009f; SpawnPosition.Y = -996.653f; SpawnPosition.Z = -99.0003f;
                SpawnHeading = 179.8418f;
                Game.Player.Character.Position = SpawnPosition;
                Game.Player.Character.Heading = SpawnHeading;

                // Start Character Modifier
                CharacterModifier.EnableCharacterModifier(chardata);
            }
            else
            {
                World.RenderingCamera = null;
                if (chardata.isDead)
                {
                    // Setting Position
                    SpawningConfig newSpawn = ConfigManager.SpawningConfig[new Random().Next(0, ConfigManager.SpawningConfig.Count - 1)];

                    SpawnPosition.X = newSpawn.X; SpawnPosition.Y = newSpawn.Y; SpawnPosition.Z = newSpawn.Z;
                    SpawnHeading = newSpawn.H;

                    // Setting Weapons
                    Game.Player.Character.Weapons.Give(WeaponHash.Parachute, 1, true, true);
                    Game.Player.Character.OpenParachute();

                    // Start Other Scripts
                    new Session(chardata).InitializeSession();
                }
                else
                {
                    // Setting Position 
                    SpawnPosition.X = chardata.LastPos[0]; SpawnPosition.Y = chardata.LastPos[1]; SpawnPosition.Z = chardata.LastPos[2];
                    SpawnHeading = new Random().Next(0, 359);

                    // Start Other Scripts
                    new Session(chardata).InitializeSession();
                }
            }

            // Setting Character Position
            Game.Player.Character.Position = SpawnPosition;
            Game.Player.Character.Heading = SpawnHeading;

            Main.GetInstance().SetNuiFocus(false, false);
        }
    }
}
