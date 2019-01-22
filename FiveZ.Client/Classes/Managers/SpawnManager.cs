using System;
using System.Collections.Generic;

namespace FiveZ.Client.Classes.Managers
{
    public class SpawnManager
    {
        public SpawnManager()
        {
            Main.GetInstance().CallExport()["spawnmanager"].spawnPlayer();
            Main.GetInstance().CallExport()["spawnmanager"].setAutoSpawn(false);
            CitizenFX.Core.Native.API.RegisterCommand("revive", new Action<int, List<object>, string>(Revive), false);
        }

        // Remove Later When Doing The Revive / Medical System
        public void Revive(int _source, List<object> _args, string _raw)
        {
            if (CitizenFX.Core.Game.Player.Character.IsDead)
            {
                CitizenFX.Core.Game.Player.Character.Resurrect();
            }
        }
    }
}
