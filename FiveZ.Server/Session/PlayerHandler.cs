using CitizenFX.Core;
using FiveZ.Server.Models;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FiveZ.Server.Session
{
    public class PlayerHandler
    {

        public static List<SessionPlayerData> SessionPlayers = new List<SessionPlayerData>();

        public static void AddPlayerIntoSession(Player p, Models.Database.FiveZPlayer zp)
        {
            SessionPlayers.Add(new SessionPlayerData(p, zp));
            // Player added to the session. Handle spawning etc.
            Debug.WriteLine(JsonConvert.SerializeObject(SessionPlayers));
        }

        public static void RemovePlayerFromSession(string handle)
        {
            SessionPlayerData foundPlayer = SessionPlayers.Find(a => a.fPlayer.Handle == handle);
            if (foundPlayer != null)
            {
                SessionPlayers.Remove(foundPlayer);
                // Player removed from the session. Handle removing etc.
                Debug.WriteLine(JsonConvert.SerializeObject(SessionPlayers));
            }
        }

    }
}
