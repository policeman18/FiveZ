﻿using System;
using System.Collections.Generic;
using CitizenFX.Core;
using Newtonsoft.Json;
using FiveZ.Server.Models;
using FiveZ.Shared.Models;

namespace FiveZ.Server.Classes.Managers
{
    public class SessionManager
    {

        public static List<Session> Sessions = new List<Session>();

        public SessionManager()
        {
            Main.GetInstance().RegisterEventHandler("FiveZ:CreateSession", new Action<Player>(CreateSession));
            Main.GetInstance().RegisterEventHandler("playerDropped", new Action<Player, string>(DeinitializeSession));
            Main.GetInstance().RegisterEventHandler("FiveZ:DeleteCharacter", new Action<Player, int>(DeleteCharacter));
            Main.GetInstance().RegisterEventHandler("FiveZ:CreateCharacter", new Action<Player, string, string, int>(CreateCharacter));
            Main.GetInstance().RegisterEventHandler("FiveZ:SelectCharacter", new Action<Player, int>(SelectCharacter));
            Main.GetInstance().RegisterEventHandler("FiveZ:Disconnect", new Action<Player>(Disconnect));
            Main.GetInstance().RegisterEventHandler("FiveZ:FinishCharacterEditing", new Action<Player, string>(FinishedCharacterEditing));
            Main.GetInstance().RegisterEventHandler("FiveZ:SaveCharacter", new Action<Player, string>(SaveCharacter));
            Utils.WriteLine("SessionManager Loaded");
        }

        private void CreateSession([FromSource] Player _player)
        {
            Utils.WriteLine($"Player Joined: {_player.Name}");
            new Session().Initialize(_player);
        }

        public void DeinitializeSession([FromSource] Player _player, string _reason)
        {
            Session foundsession = Sessions.Find(s => s.Player.Handle == _player.Handle);
            if (foundsession != null)
            {
                foundsession.Deinitialize();
                Utils.WriteLine($"Player Dropped: {_player.Name} | {_reason}");
            }
        }

        public void DeleteCharacter([FromSource] Player _player, int _charID)
        {
            Session playersession = Sessions.Find(s => s.Player.Handle == _player.Handle);
            if (playersession != null)
            {
                List<Character> newList = playersession.DeleteUserCharacter(_charID);
                playersession.Player.TriggerEvent("FiveZ:UpdateCharacterScreen", JsonConvert.SerializeObject(newList));
            }
        }

        public void CreateCharacter([FromSource] Player _player, string _first, string _last, int _gender)
        {
            Session playersession = Sessions.Find(s => s.Player.Handle == _player.Handle);
            if (playersession != null)
            {
                List<Character> newList = playersession.CreateUserCharacter(_first, _last, (Shared.Enums.Genders)_gender);
                playersession.Player.TriggerEvent("FiveZ:UpdateCharacterScreen", JsonConvert.SerializeObject(newList));
            }
        }

        public void SelectCharacter([FromSource] Player _player, int _charID)
        {
            Session playersession = Sessions.Find(s => s.Player.Handle == _player.Handle);
            if (playersession != null)
            {
                playersession.SelectUserCharacter(_charID);
            }
        }

        private void Disconnect([FromSource] Player _player)
        {
            Session playersession = Sessions.Find(s => s.Player.Handle == _player.Handle);
            if (playersession != null)
            {
                playersession.Drop("Disconnected");
            }
        }

        private void FinishedCharacterEditing([FromSource] Player _player, string _characterchar)
        {
            Session playersession = Sessions.Find(s => s.Player.Handle == _player.Handle);
            if (playersession != null)
            {
                Character FinishedCharacter = JsonConvert.DeserializeObject<Character>(_characterchar);
                playersession.Character = FinishedCharacter;
                playersession.UpdateUserCharacter();
                playersession.Player.TriggerEvent("FiveZ:HandlePlayerSpawn", JsonConvert.SerializeObject(playersession.Character));
            }
        }

        private void SaveCharacter([FromSource] Player _player, string _characterchar)
        {
            Session playersession = Sessions.Find(s => s.Player.Handle == _player.Handle);
            if (playersession != null)
            {
                Character SpawnedCharacter = JsonConvert.DeserializeObject<Character>(_characterchar);
                playersession.Character = SpawnedCharacter;
                playersession.UpdateUserCharacter();
                Utils.WriteLine($"Saved Player: {_player.Name}'s character {SpawnedCharacter.FirstName} {SpawnedCharacter.LastName}");
            }
        }

    }
}
