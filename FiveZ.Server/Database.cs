using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LiteDB;
using FiveZ.Server.Models;
using FiveZ.Shared;
using Newtonsoft.Json;

namespace FiveZ.Server
{
    public class Database
    {

        private static readonly string DBPath = $"{API.GetResourcePath(API.GetCurrentResourceName())}/data/database.db";

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
            using (LiteDatabase db = new LiteDatabase(DBPath))
            {
                LiteCollection<User> users = db.GetCollection<User>("users");
                User newUser = new User().CreatePlayerUser(_player);
                users.Insert(newUser);
                return users.FindOne(nu => nu.Identifier == _player.Identifiers["license"]);
            }
        }

        public static void UpdatePlayerUser(User _playerUsers)
        {
            using (LiteDatabase db = new LiteDatabase(DBPath))
            {
                LiteCollection<User> users = db.GetCollection<User>("users");
                users.Update(_playerUsers);
            }
        }

    }
}
