using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using FiveZ.Server.Classes.Managers;
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
        public Character Character { get;  set; }

        public void Initialize(Player _player)
        {
            SessionManager.Sessions.Add(this);
            this.Player = _player;
            
            Tuple<bool, User> user = this.GetPlayerUser(_player);
            if (user.Item1 == false)
            {
                User createdUser = this.CreatePlayerUser(_player);
                this.User = createdUser;
            }
            else
            {
                this.User = user.Item2;
                this.Characters = this.GetUserCharacters(_player);
            }

            this.SetUserLastPlayed();

            // Load Client Configs
            this.Player.TriggerEvent("FiveZ:SendClientConfigs", "spawns", JsonConvert.SerializeObject(ConfigManager.SpawningConfig));

            // Enable Main Screen
            this.Player.TriggerEvent("FiveZ:EnableCharacterScreen", JsonConvert.SerializeObject(this.Characters));
        }

        public void Deinitialize()
        {
            SessionManager.Sessions.Remove(this);
        }

        // User Methods
        public Tuple<bool, User> GetPlayerUser(Player _player)
        {
            try
            {
                using (LiteDatabase db = new LiteDatabase(ConfigManager.DBPath))
                {
                    LiteCollection<User> users = db.GetCollection<User>("users");
                    User foundUser = users.FindOne(u => u.Identifier == _player.Identifiers["license"]);
                    if (foundUser == null)
                    {
                        return Tuple.Create<bool, User>(false, null);
                    }
                    else
                    {
                        return Tuple.Create<bool, User>(true, foundUser);
                    }
                }
            }
            catch(Exception ex)
            {
                Utils.Throw(ex);
                return Tuple.Create<bool, User>(false, null);
            }
        }

        public User CreatePlayerUser(Player _player)
        {
            try
            {
                User newUser = new User();
                newUser.Name = _player.Name;
                newUser.Identifier = _player.Identifiers["license"];
                newUser.PermissionFlag = Permission.User;
                newUser.BanData = new BanStatus();
                newUser.IsWhitelisted = false;
                newUser.LastPlayed = DateTime.Now;
                using (LiteDatabase db = new LiteDatabase(ConfigManager.DBPath))
                {
                    LiteCollection<User> users = db.GetCollection<User>("users");
                    users.Insert(newUser);
                }
                return this.User;
            }
            catch(Exception ex)
            {
                Utils.Throw(ex);
                return null;
            }
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
                using (LiteDatabase db = new LiteDatabase(ConfigManager.DBPath))
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
                using (LiteDatabase db = new LiteDatabase(ConfigManager.DBPath))
                {
                    LiteCollection<Character> characters = db.GetCollection<Character>("characters");
                    characters.Insert(new Character(this.User.Id, _firstName, _lastName, _gender));
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

        public List<Character> GetUserCharacters(Player _player)
        {
            try
            {
                using (LiteDatabase db = new LiteDatabase(ConfigManager.DBPath))
                {
                    LiteCollection<Character> characters = db.GetCollection<Character>("characters");
                    User user = SessionManager.Sessions.Find(u => u.Player.Handle == _player.Handle).User;
                    IEnumerable<Character> allCharacters = characters.Find(ac => ac.UserId == user.Id);
                    return allCharacters.ToList();
                }
            }
            catch(Exception ex)
            {
                Utils.Throw(ex);
                return null;
            }
        }

        public void UpdateUserCharacter()
        {
            try
            {
                using (LiteDatabase db = new LiteDatabase(ConfigManager.DBPath))
                {
                    LiteCollection<Character> characters = db.GetCollection<Character>("characters");
                    characters.Update(this.Character);
                }
            }
            catch(Exception ex)
            {
                Utils.Throw(ex);
            }
        }

        public void SelectUserCharacter(int _charID)
        {
            try
            {
                using (LiteDatabase db = new LiteDatabase(ConfigManager.DBPath))
                {
                    LiteCollection<Character> characters = db.GetCollection<Character>("characters");
                    Character foundCharacter = characters.FindOne(c => c.Id == _charID);
                    this.Character = foundCharacter;
                    this.Player.TriggerEvent("FiveZ:HandlePlayerSpawn", JsonConvert.SerializeObject(foundCharacter));
                }
            }
            catch(Exception ex)
            {
                Utils.Throw(ex);
            }
        }
            
        public List<Character> DeleteUserCharacter(int _charID)
        {
            try
            {
                using (LiteDatabase db = new LiteDatabase(ConfigManager.DBPath))
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
