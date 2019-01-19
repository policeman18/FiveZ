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
            Utils.WriteLine("SessionManager Loaded");
        }

        private void ClientResourceStarted(string _resource)
        {
            if (API.GetCurrentResourceName() == _resource)
            {
                Main.TriggerServerEvent("FiveZ:CreateSession");
            }
        }

    }
}
