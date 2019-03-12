using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using FiveZ.Client.Models;

namespace FiveZ.Client.Classes.Managers
{
    public class SessionManager
    {
        private bool NUIReady = false;
        public static Session PlayerSession = null;

        public SessionManager()
        {
            // Events
            Main.GetInstance().RegisterEventHandler("onClientResourceStart", new Action<string>(ClientResourceStarted));
            Main.GetInstance().RegisterEventHandler("FiveZ:EnableCharacterScreen", new Action<string>(EnableCharacterScreen));
            Main.GetInstance().RegisterEventHandler("FiveZ:UpdateCharacterScreen", new Action<string>(UpdateCharacterScreen));

            // NUI Callbacks
            Main.GetInstance().RegisterNUICallback("fivez_nui_loaded", SetNUIReady);
            Main.GetInstance().RegisterNUICallback("fivez_character_deletecharacter", DeleteCharacter);
            Main.GetInstance().RegisterNUICallback("fivez_character_selectcharacter", SelectCharacter);
            Main.GetInstance().RegisterNUICallback("fivez_character_createcharacter", CreateCharacter);
            Main.GetInstance().RegisterNUICallback("fivez_disconnect", Disconnect);

            Utils.WriteLine("SessionManager Loaded");
        }

        private async void ClientResourceStarted(string _resource)
        {
            if (API.GetCurrentResourceName() == _resource)
            {
                Main.GetInstance().SetNuiFocus(false, false);
                int maxThreshhold = 5000;
                int timeStamp = Game.GameTime;
                Game.Player.CanControlCharacter = false;
                Game.Player.IsInvincible = true;
                Screen.LoadingPrompt.Show("Loading User Interface", LoadingSpinnerType.Clockwise1);
                do { await BaseScript.Delay(100);  } while (!NUIReady && timeStamp + maxThreshhold > Game.GameTime);
                Screen.LoadingPrompt.Hide();
                Game.Player.CanControlCharacter = true;
                Game.Player.IsInvincible = false;
                Main.TriggerServerEvent("FiveZ:CreateSession");
            }
        }

        private void EnableCharacterScreen(string _characters)
        {
            Main.GetInstance().SetNuiFocus(true, true);
            Main.GetInstance().SendNUIData("fivez_character", "OpenMenu", _characters);
            Screen.Hud.IsRadarVisible = false;
            World.RenderingCamera = World.CreateCamera(new Vector3(0f, 1500f, 500f), Vector3.Zero, 50);
        }

        private void UpdateCharacterScreen(string _characters)
        {
            Main.GetInstance().SendNUIData("fivez_character", "UpdateCharacters", _characters);
        }

        private void SetNUIReady(dynamic data, CallbackDelegate _callback)
        {
            NUIReady = true;
            Utils.WriteLine("NUI READY TO BE USED!!!");
        }

        private void DeleteCharacter(dynamic data, CallbackDelegate _callback)
        {
            Main.TriggerServerEvent("FiveZ:DeleteCharacter", data.id);
        }

        private void SelectCharacter(dynamic data, CallbackDelegate _callback)
        {
            Main.TriggerServerEvent("FiveZ:SelectCharacter", data.id);
            Main.GetInstance().SendNUIData("fivez_character", "CloseMenu");
            Main.GetInstance().SetNuiFocus(true, false);
        }

        private void CreateCharacter(dynamic data, CallbackDelegate _callback)
        {
            Main.TriggerServerEvent("FiveZ:CreateCharacter", data.first, data.last, data.gender);
        }
        
        private void Disconnect(dynamic data, CallbackDelegate _callback)
        {
            Main.TriggerServerEvent("FiveZ:Disconnect");
        }

    }
}
