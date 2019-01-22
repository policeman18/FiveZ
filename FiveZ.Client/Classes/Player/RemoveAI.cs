using System.Threading.Tasks;
using CitizenFX.Core.Native;
using CitizenFX.Core;
using FiveZ.Shared;

namespace FiveZ.Client.Classes.Player
{
    public class RemoveAI
    {
        public RemoveAI()
        {
            Main.GetInstance().RegisterTickHandler(OnTick);
            Utils.WriteLine("RemoveAI Loaded");
        }

        private async Task OnTick()
        {
            await Task.FromResult(0);
            API.SetVehicleDensityMultiplierThisFrame(0f);
            API.SetPedDensityMultiplierThisFrame(0f);
            API.SetRandomVehicleDensityMultiplierThisFrame(0f);
            API.SetParkedVehicleDensityMultiplierThisFrame(0f);
            API.SetScenarioPedDensityMultiplierThisFrame(0f, 0f);

            Vector3 PlayerPos = Game.Player.Character.Position;
            API.RemoveVehiclesFromGeneratorsInArea(PlayerPos.X - 500f, PlayerPos.Y - 500f, PlayerPos.Z - 500f, PlayerPos.X + 500f, PlayerPos.Y + 500f, PlayerPos.Z + 500f, 0);

            API.SetGarbageTrucks(false);
            API.SetRandomBoats(false);
        }
    }
}
