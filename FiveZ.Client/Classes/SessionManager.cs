using System;
using CitizenFX.Core.Native;
using FiveZ.Shared;

namespace FiveZ.Client.Classes
{
    public class SessionManager
    {

        public SessionManager()
        {
            Main.GetInstance().RegisterEventHandler("onClientResourceStart", new Action<string>(ClientResourceStarted));
            Main.GetInstance().RegisterEventHandler("FiveZ:EnableCharacterScreen", new Action<string>(EnableCharacterScreen));
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

        }

    }
}
