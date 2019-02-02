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
            try
            {
                // Events
                Main.GetInstance().RegisterEventHandler("FiveZ:HandlePlayerSpawn", new Action<string>(HandlePlayerSpawn));

                // Commands
                API.RegisterCommand("revive", new Action<int, List<object>, string>(Revive), false);

                // Disable Respawning
                DisableAutoRespawning();
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
            if (chardata.isNew)
            {
                await Game.Player.ChangeModel(new Model(chardata.Model));
                Vector3 CreatorPosition = new Vector3(403.009f, -996.653f, -99.0003f);
                Game.Player.Character.Position = CreatorPosition;
                Game.Player.Character.Heading = 179.8418f;


                // Set Clothing
                Game.Player.Character.Style[PedComponents.Hair].SetVariation(chardata.Clothing[Convert.ToInt32(PedComponents.Hair)][0], chardata.Clothing[Convert.ToInt32(PedComponents.Hair)][1]);
                Game.Player.Character.Style[PedComponents.Torso].SetVariation(chardata.Clothing[Convert.ToInt32(PedComponents.Torso)][0], chardata.Clothing[Convert.ToInt32(PedComponents.Torso)][1]);
                Game.Player.Character.Style[PedComponents.Legs].SetVariation(chardata.Clothing[Convert.ToInt32(PedComponents.Legs)][0], chardata.Clothing[Convert.ToInt32(PedComponents.Legs)][1]);
                Game.Player.Character.Style[PedComponents.Hands].SetVariation(chardata.Clothing[Convert.ToInt32(PedComponents.Hands)][0], chardata.Clothing[Convert.ToInt32(PedComponents.Hands)][1]);
                Game.Player.Character.Style[PedComponents.Shoes].SetVariation(chardata.Clothing[Convert.ToInt32(PedComponents.Shoes)][0], chardata.Clothing[Convert.ToInt32(PedComponents.Shoes)][1]);
                Game.Player.Character.Style[PedComponents.Special1].SetVariation(chardata.Clothing[Convert.ToInt32(PedComponents.Special1)][0], chardata.Clothing[Convert.ToInt32(PedComponents.Special1)][1]);
                Game.Player.Character.Style[PedComponents.Special2].SetVariation(chardata.Clothing[Convert.ToInt32(PedComponents.Special2)][0], chardata.Clothing[Convert.ToInt32(PedComponents.Special2)][1]);
                Game.Player.Character.Style[PedComponents.Special3].SetVariation(chardata.Clothing[Convert.ToInt32(PedComponents.Special3)][0], chardata.Clothing[Convert.ToInt32(PedComponents.Special3)][1]);
                Game.Player.Character.Style[PedComponents.Textures].SetVariation(chardata.Clothing[Convert.ToInt32(PedComponents.Textures)][0], chardata.Clothing[Convert.ToInt32(PedComponents.Textures)][1]);
                Game.Player.Character.Style[PedComponents.Torso2].SetVariation(chardata.Clothing[Convert.ToInt32(PedComponents.Torso2)][0], chardata.Clothing[Convert.ToInt32(PedComponents.Torso2)][1]);

                // Set Appearance

                CharacterModifier.EnableCharacterModifier();
            }
            else
            {
                await Game.Player.ChangeModel(new Model(chardata.Model));
                Vector3 SpawnPosition = new Vector3(chardata.LastPos[0], chardata.LastPos[1], chardata.LastPos[2]);
                Game.Player.Character.Position = SpawnPosition;
                Game.Player.Character.Heading = new Random().Next(0, 359);

                // Set Clothing
                // Set Inventiry
                // Start Other Scripts
            }

            Main.GetInstance().SetNuiFocus(false, false);
        }
    }
}
