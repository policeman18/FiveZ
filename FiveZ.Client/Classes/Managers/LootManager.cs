using System;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Newtonsoft.Json;
using FiveZ.Shared.Models;

namespace FiveZ.Client.Classes.Managers
{
    public class LootManager
    {
        private List<LootSpawnPoint> ItemSpawns = new List<LootSpawnPoint>();

        public LootManager()
        {
            Main.GetInstance().RegisterEventHandler("FiveZ:RecieveLootSpawns", new Action<string>((spawns) => {
                ItemSpawns = JsonConvert.DeserializeObject<List<LootSpawnPoint>>(spawns);
                Utils.WriteLine(JsonConvert.SerializeObject(ItemSpawns));
            }));
            API.RegisterCommand("tp", new Action<int, List<object>, string>(TeleportCommand), false);
            API.RegisterCommand("pos", new Action<int, List<object>, string>(GetPosition), false);
            Main.GetInstance().RegisterTickHandler(DisplayLoot);
        }

        public void GetPosition(int _source, List<object> args, string _raw)
        {
            Utils.WriteLine($"Position: {Game.Player.Character.Position.ToString()}");
        }

        public void TeleportCommand(int _source, List<object> args, string _raw)
        {
            Vector3 position = new Vector3(-30.38f, -98.90f, 57.35f);
            Game.Player.Character.Position = position;
        }

        private async Task DisplayLoot()
        {

            int activeCount = 0;
            foreach (LootSpawnPoint point in ItemSpawns)
            {
                if (point.isAvailable)
                {
                    activeCount += 1;
                }
            }

            Screen.ShowSubtitle($"Displaying {activeCount} items");

            for (int a = 0; a < ItemSpawns.Count; a++)
            {
                LootSpawnPoint spawn = ItemSpawns[a];
                Vector3 MarkerPosition = new Vector3(spawn.X, spawn.Y, spawn.Z - 0.95f);
                Vector3 TextPosition = new Vector3(spawn.X, spawn.Y, spawn.Z - 0.50f);
                if (spawn.isAvailable)
                {
                    if (Game.Player.Character.Position.DistanceToSquared(MarkerPosition) < 150f)
                    {
                        World.DrawMarker(MarkerType.HorizontalCircleSkinny, MarkerPosition, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(1f, 1f, 1f), Color.FromArgb(255, 0, 0), false, false, false, null, null, false);
                        if (Game.Player.Character.Position.DistanceToSquared(MarkerPosition) <= 1.8f)
                        {
                            Helpers.Draw3DText(TextPosition, Color.FromArgb(255, 255, 255), spawn.Loot.Name.ToUpper(), Font.ChaletComprimeCologne, 0.07f, true);
                            Screen.DisplayHelpTextThisFrame($"Press ~INPUT_PICKUP~ to pickup");
                        }
                    }
                }
            }
            await Task.FromResult(0);
        }
    }
}
