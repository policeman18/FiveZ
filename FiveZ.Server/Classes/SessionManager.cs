using System;
using System.Collections.Generic;
using CitizenFX.Core;
using Newtonsoft.Json;
using FiveZ.Server.Models;
using FiveZ.Shared;

namespace FiveZ.Server.Classes
{
    public class SessionManager
    {

        public static List<Session> Sessions = new List<Session>();

        public SessionManager()
        {
            Main.GetInstance().RegisterEventHandler("FiveZ:CreateSession", new Action<Player>(CreateSession));
            Main.GetInstance().RegisterEventHandler("playerDropped", new Action<Player, string>(DeinitializeSession));
            Utils.WriteLine("SessionManager Loaded");
        }

        private void CreateSession([FromSource] Player _player)
        {
            Utils.WriteLine($"Player Joined: {_player.Name}");
            new Session().Initialize(_player);
            //Utils.WriteLine(JsonConvert.SerializeObject(Sessions));
        }

        public void DeinitializeSession([FromSource] Player _player, string _reason)
        {
            Sessions.Find(s => s.Player.Handle == _player.Handle).Deinitialize();
            Utils.WriteLine($"Player Dropped: {_player.Name} | {_reason}");
            //Utils.WriteLine(JsonConvert.SerializeObject(Sessions));
        }

    }
}
