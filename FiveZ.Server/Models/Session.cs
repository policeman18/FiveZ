using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using FiveZ.Server.Classes;
using FiveZ.Shared;

namespace FiveZ.Server.Models
{
    public class Session
    {
        public Player Player { get; protected set; }
        public User User { get; protected set; }

        public int Ping => API.GetPlayerPing(this.Player.Handle);
        public int LastMsg => API.GetPlayerLastMsg(this.Player.Handle);
        public string EndPoint => API.GetPlayerEndpoint(this.Player.Handle);
        public void Drop(string _reason) => API.DropPlayer(this.Player.Handle, _reason);
        
        public List<Character> Characters { get; protected set; }

        public Session(Player _player)
        {
            this.Player = _player;

            User foundUser = Database.GetPlayerUser(_player);
            if (foundUser == null)
            {
                User newUser = Database.CreatePlayerUser(_player);
                this.User = newUser;
            }
            else
            {
                this.User = foundUser;
            }
            Utils.WriteLine($"USER FOUND: {JsonConvert.SerializeObject(foundUser)}");
        }

        public void Initialize()
        {
            SessionManager.Sessions.Add(this);
        }

        public void Deinitialize()
        {
            SessionManager.Sessions.Remove(this);
        }
    }
}
