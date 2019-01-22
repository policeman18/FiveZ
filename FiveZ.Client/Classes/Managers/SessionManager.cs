using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FiveZ.Shared;

namespace FiveZ.Client.Classes.Managers
{
    public class SessionManager
    {

        public SessionManager()
        {
            Main.GetInstance().RegisterEventHandler("onClientResourceStart", new Action<string>(ClientResourceStarted));
            Main.GetInstance().RegisterEventHandler("FiveZ:EnableCharacterScreen", new Action<string>(EnableCharacterScreen));
            Main.GetInstance().RegisterEventHandler("FiveZ:UpdateCharacterScreen", new Action<string>(UpdateCharacterScreen));
            Main.GetInstance().RegisterNUICallback("fivez_character_deletecharacter", DeleteCharacter);
            Utils.WriteLine("SessionManager Loaded");
        }

        private void ClientResourceStarted(string _resource)
        {
            if (API.GetCurrentResourceName() == _resource)
            {
                Main.TriggerServerEvent("FiveZ:CreateSession");
            }
        }

        private void EnableCharacterScreen(string _characters)
        {
            Main.GetInstance().SetNuiFocus(true, true);
            Main.GetInstance().SendNUIData("fivez_character", "OpenMenu", _characters);
        }

        private void UpdateCharacterScreen(string _characters)
        {
            Main.GetInstance().SendNUIData("fivez_character", "UpdateCharacters", _characters);
        }

        private void DeleteCharacter(dynamic data, CallbackDelegate _callback)
        {
            Main.TriggerServerEvent("FiveZ:DeleteCharacter", data.id);
            _callback();
        }

    }
}
