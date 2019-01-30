using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LiteDB;
using FiveZ.Server.Classes.Managers;
using FiveZ.Server.Models;
using FiveZ.Shared.Models;
using FiveZ.Shared.Enums;

namespace FiveZ.Server
{
    public class Database
    {

        public static readonly string DBPath = $"{API.GetResourcePath(API.GetCurrentResourceName())}/data/database.db";

        public static Tuple<bool, User> GetPlayerUser(Player _player)
        {
            using (LiteDatabase db = new LiteDatabase(DBPath))
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

        public static User CreatePlayerUser(Player _player)
        {
            try
            {
                using (LiteDatabase db = new LiteDatabase(DBPath))
                {
                    LiteCollection<User> users = db.GetCollection<User>("users");
                    User newUser = new Session().CreatePlayerUser(_player);
                    users.Insert(newUser);
                    return users.FindOne(nu => nu.Identifier == _player.Identifiers["license"]);
                }
            }
            catch(Exception ex)
            {
                Utils.Throw(ex);
                return null;
            }
        }

        public static List<Character> CreateUserCharacter(Player _player, string _firstName, string _lastName, Genders _gender)
        {
            try
            {
                using (LiteDatabase db = new LiteDatabase(DBPath))
                {
                    LiteCollection<Character> characters = db.GetCollection<Character>("characters");
                    User user = SessionManager.Sessions.Find(u => u.Player.Handle == _player.Handle).User;
                    characters.Insert(new Character() { UserId = user.Id, FirstName = _firstName, LastName = _lastName, Gender = _gender });
                    IEnumerable<Character> allCharacters = characters.Find(ac => ac.UserId == user.Id);
                    return allCharacters.ToList();
                }
            }
            catch (Exception ex)
            {
                Utils.Throw(ex);
                return null;
            }
        }

        public static List<Character> GetUserCharacters(Player _player)
        {
            try
            {
                using (LiteDatabase db = new LiteDatabase(DBPath))
                {
                    LiteCollection<Character> characters = db.GetCollection<Character>("characters");
                    User user = SessionManager.Sessions.Find(u => u.Player.Handle == _player.Handle).User;
                    IEnumerable<Character> allCharacters = characters.Find(ac => ac.UserId == user.Id);
                    return allCharacters.ToList();
                }
            }
            catch (Exception ex)
            {
                Utils.Throw(ex);
                return null;
            }
        }

        public static Character GetUserCharacter(Player _player, int _id)
        {
            try
            {
                using (LiteDatabase db = new LiteDatabase(DBPath))
                {
                    LiteCollection<Character> characters = db.GetCollection<Character>("characters");
                    User user = SessionManager.Sessions.Find(u => u.Player.Handle == _player.Handle).User;
                    Character foundCharacter = characters.FindOne(c => c.UserId == _id);
                    return foundCharacter;
                }
            }
            catch (Exception ex)
            {
                Utils.Throw(ex);
                return null;
            }
        }

    }
}
