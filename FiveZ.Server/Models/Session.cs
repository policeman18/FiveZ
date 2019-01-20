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

        public void Initialize(Player _player)
        {
            this.Player = _player;

            Tuple<bool, User> user = Database.GetPlayerUser(_player);
            if (user.Item1 == false)
            {
                User createdUser = Database.CreatePlayerUser(_player);
                this.User = createdUser;
            }
            else
            {
                this.User = user.Item2;
            }

            this.User.SetLastPlayed();
            SessionManager.Sessions.Add(this);
        }

        public void Deinitialize()
        {
            SessionManager.Sessions.Remove(this);
        }
    }
}
