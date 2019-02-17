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
            Main.GetInstance().RegisterTickHandler(this.DecreaseFood);
            Main.GetInstance().RegisterTickHandler(this.DecreaseThirst);
            return this;
        }

        public void DeinitializeSession()
        {
            Main.GetInstance().UnregisterTickHandler(this.CheckDeadStatus);
            Main.GetInstance().UnregisterTickHandler(this.SaveLastLocation);
            Main.GetInstance().UnregisterTickHandler(this.SaveCharacter);
            Main.GetInstance().UnregisterTickHandler(this.DecreaseFood);
            Main.GetInstance().UnregisterTickHandler(this.DecreaseThirst);
            // Trigger back to character screen
        }

        // Sets Characters Death Status
        private async Task CheckDeadStatus()
        {
            if (Game.Player.Character.IsDead)
            {
                if (!this.SpawnedCharacter.isDead)
                {
                    this.SpawnedCharacter.SetDeadStatus(true);
                }
            }
            else
            {
                if (this.SpawnedCharacter.isDead)
                {
                    this.SpawnedCharacter.SetDeadStatus(false);
                }
            }
            await BaseScript.Delay(1000);
        }

        // Sets Characters Last Position
        private async Task SaveLastLocation()
        {
            await BaseScript.Delay(15000);
            Vector3 CurrentPostion = Game.Player.Character.Position;
            float HeightAboveGround = Game.Player.Character.HeightAboveGround;
            this.SpawnedCharacter.LastPos[0] = CurrentPostion.X;
            this.SpawnedCharacter.LastPos[1] = CurrentPostion.Y;
            this.SpawnedCharacter.LastPos[2] = CurrentPostion.Z - HeightAboveGround;
        }

        // Saves Character
        private async Task SaveCharacter()
        {
            await BaseScript.Delay(60000);
            CitizenFX.Core.UI.Screen.LoadingPrompt.Show("Saving Character", CitizenFX.Core.UI.LoadingSpinnerType.Clockwise1);
            await BaseScript.Delay(3500);
            CitizenFX.Core.UI.Screen.LoadingPrompt.Hide();
            Main.TriggerServerEvent("FiveZ:SaveCharacter", Newtonsoft.Json.JsonConvert.SerializeObject(this.SpawnedCharacter));
        }

        // Decreases Characters Hunger
        private async Task DecreaseFood()
        {
            await BaseScript.Delay(54 * 1000);
            SpawnedCharacter.RemoveHunger(2);
            if (SpawnedCharacter.Hunger <= 0)
            {
                Game.Player.Character.Health -= 10;
            }
        }

        // Decreases Characters Thirst
        private async Task DecreaseThirst()
        {
            await BaseScript.Delay(45 * 1000);
            SpawnedCharacter.RemoveThirst(2);
            if (SpawnedCharacter.Thirst <= 0)
            {
                Game.Player.Character.Health -= 10;
            }
        }
    }
}
