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

        public Session(Character _character)
        {
            this.SpawnedCharacter = _character;
            Classes.Managers.SessionManager.PlayerSession = this;
        }

        public Session InitializeSession()
        {
            Main.GetInstance().RegisterTickHandler(this.CheckDeadStatus);
            Main.GetInstance().RegisterTickHandler(this.SaveLastLocation);
            Main.GetInstance().RegisterTickHandler(this.SaveCharacter);
            return this;
        }

        private async Task CheckDeadStatus()
        {
            if (Game.Player.Character.IsDead)
            {
                if (!this.SpawnedCharacter.isDead)
                {
                    this.SpawnedCharacter.isDead = true;
                }
            }
            await BaseScript.Delay(1000);
        }

        private async Task SaveLastLocation()
        {
            Vector3 CurrentPostion = Game.Player.Character.Position;
            float HeightAboveGround = Game.Player.Character.HeightAboveGround;
            this.SpawnedCharacter.LastPos[0] = CurrentPostion.X;
            this.SpawnedCharacter.LastPos[1] = CurrentPostion.Y;
            this.SpawnedCharacter.LastPos[2] = CurrentPostion.Z - HeightAboveGround;
            Utils.WriteLine("Saving Last Location");
            await BaseScript.Delay(15000);
        }

        private async Task SaveCharacter()
        {
            await BaseScript.Delay(60000);
            CitizenFX.Core.UI.Screen.LoadingPrompt.Show("Saving Character", CitizenFX.Core.UI.LoadingSpinnerType.Clockwise1);
            await BaseScript.Delay(3500);
            CitizenFX.Core.UI.Screen.LoadingPrompt.Hide();
            Main.TriggerServerEvent("FiveZ:SaveCharacter", Newtonsoft.Json.JsonConvert.SerializeObject(this.SpawnedCharacter));
        }
    }
}
