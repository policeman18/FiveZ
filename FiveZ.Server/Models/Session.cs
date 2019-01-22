using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using FiveZ.Server.Classes.Managers;
using FiveZ.Shared;
using FiveZ.Shared.Models;
using FiveZ.Shared.Enums;
using LiteDB;

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

        public List<Character> Characters { get; protected set; } = new List<Character>();

        public void Initialize(Player _player)
        {
            SessionManager.Sessions.Add(this);
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
                this.Characters = Database.GetUserCharacters(_player);
            }

            this.SetUserLastPlayed();

            this.Player.TriggerEvent("FiveZ:EnableCharacterScreen", JsonConvert.SerializeObject(this.Characters));
        }

        public void Deinitialize()
        {
            SessionManager.Sessions.Remove(this);
        }

        // User Methods
        public User CreatePlayerUser(Player _player)
        {
            this.User.Name = _player.Name;
            this.User.Identifier = _player.Identifiers["license"];
            this.User.PermissionFlag = Permission.User;
            this.User.BanData = new BanStatus();
            this.User.IsWhitelisted = false;
            this.User.LastPlayed = DateTime.Now;
            return this.User;
        }

        public void SetUserLastPlayed()
        {
            this.User.LastPlayed = DateTime.Now;
            this.SetUpdateUser();
        }

        public void SetUserPermission(Permission _perm)
        {
            this.User.PermissionFlag = _perm;
            this.SetUpdateUser();
        }

        public void SetUserBan(Player _player, string _reason, string _banType, int _banCount, string _bannerName)
        {
            this.User.BanData.IsBanned = true;
            this.User.BanData.Reason = _reason;
            DateTime banTime = new DateTime();
            switch (_banType)
            {
                case "second":
                    banTime.AddSeconds(_banCount);
                    break;
                case "minute":
                    banTime.AddMinutes(_banCount);
                    break;
                case "hour":
                    banTime.AddHours(_banCount);
                    break;
                case "day":
                    banTime.AddDays(_banCount);
                    break;
                case "month":
                    banTime.AddMonths(_banCount);
                    break;
                default:
                    banTime.AddMinutes(_banCount);
                    break;
            }
            this.User.BanData.BanTime = banTime;
            this.User.BanData.BannedBy = _bannerName;
            this.SetUpdateUser();
            SessionManager.Sessions.Find(s => s.User.Id == this.User.Id).Drop($"You have been banned from the server until {this.User.BanData.BanTime}");
        }

        public void SetUserWhitelisted(bool _status)
        {
            this.User.IsWhitelisted = _status;
            this.SetUpdateUser();
        }

        public void SetUpdateUser()
        {
            try
            {
                using (LiteDatabase db = new LiteDatabase(Database.DBPath))
                {
                    LiteCollection<User> users = db.GetCollection<User>("user");
                    users.Update(this.User);
                }
            }
            catch(Exception ex)
            {
                Utils.Throw(ex);
            }
        }

        // Character Methods
        public List<Character> CreateUserCharacter(string _firstName, string _lastName, Genders _gender)
        {
            try
            {
                using (LiteDatabase db = new LiteDatabase(Database.DBPath))
                {
                    LiteCollection<Character> characters = db.GetCollection<Character>("characters");
                    characters.Insert(new Character() { UserId = this.User.Id, FirstName = _firstName, LastName = _lastName, Gender = _gender });
                    IEnumerable<Character> allCharacters = characters.Find(ac => ac.UserId == this.User.Id);
                    return allCharacters.ToList();
                }
            }
            catch (Exception ex)
            {
                Utils.Throw(ex);
                return null;
            }
        }

        public void UpdateUserCharacter()
        {

        } // ND

        public void SelectUserCharacter()
        {

        } // ND
            
        public List<Character> DeleteUserCharacter(int _charID)
        {
            try
            {
                using (LiteDatabase db = new LiteDatabase(Database.DBPath))
                {
                    LiteCollection<Character> characters = db.GetCollection<Character>("characters");
                    characters.Delete(c => c.Id == _charID);
                    IEnumerable<Character> allCharacters = characters.Find(ac => ac.UserId == this.User.Id);
                    return allCharacters.ToList();
                }
            }
            catch(Exception ex)
            {
                Utils.Throw(ex);
                return null;
            }
        }

    }
}
