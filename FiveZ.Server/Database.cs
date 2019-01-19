using CitizenFX.Core;
using CitizenFX.Core.Native;
using LiteDB;
using FiveZ.Server.Models;

namespace FiveZ.Server
{
    public class Database
    {

        private static readonly string DBPath = $"{API.GetResourcePath(API.GetCurrentResourceName())}/data/database.db";

        public static User GetPlayerUser(Player _player)
        {
            using (LiteDatabase db = new LiteDatabase(DBPath))
            {
                LiteCollection<User> users = db.GetCollection<User>("users");
                return users.FindOne(u => u.Identifiers["license"] == _player.Identifiers["license"]);
                //if (foundUser == null) { return null; } else { return foundUser; }
            }
        }

        public static User CreatePlayerUser(Player _player)
        {
            using (LiteDatabase db = new LiteDatabase(DBPath))
            {
                LiteCollection<User> users = db.GetCollection<User>("users");
                User newUser = new User(_player);
                users.Insert(newUser);
                return users.FindOne(u => u.Identifiers["license"] == _player.Identifiers["license"]);
            }
        }

    }
}
