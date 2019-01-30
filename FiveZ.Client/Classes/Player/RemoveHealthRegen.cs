using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace FiveZ.Client.Classes.Player
{
    public class RemoveHealthRegen
    {
        public RemoveHealthRegen()
        {
            Main.GetInstance().RegisterTickHandler(OnTick);
        }

        public async Task OnTick()
        {
            API.SetPlayerHealthRechargeMultiplier(Game.Player.Handle, 0f);
            await Task.FromResult(0);
        }
    }
}
