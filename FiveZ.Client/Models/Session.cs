using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FiveZ.Shared.Models;

namespace FiveZ.Client.Models
{
    public class Session
    {
        public Character SpawnedCharacter { get; set; }
        public bool isDead { get; set; } = false;

        public Session(Character _character)
        {
            this.SpawnedCharacter = _character;
            Classes.Managers.SessionManager.PlayerSession = this;
        }

        public Session InitializeSession()
        {
            Main.GetInstance().RegisterTickHandler(this.CheckDeadStatus);
            return this;
        }

        private async Task CheckDeadStatus()
        {
            if (Game.Player.Character.IsDead)
            {
                if (!this.isDead)
                {
                    this.isDead = true;
                    CitizenFX.Core.UI.Screen.ShowNotification("YOU JUST DIED BITCH");
                }
            }
            await BaseScript.Delay(1000);
        }
    }
}
