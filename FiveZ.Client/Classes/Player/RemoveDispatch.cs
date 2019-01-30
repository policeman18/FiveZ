using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace FiveZ.Client.Classes.Player
{
    public class RemoveDispatch
    {
        public RemoveDispatch()
        {
            RemoveDispatched();
            Main.GetInstance().RegisterTickHandler(DisableWanted);
        }

        public void RemoveDispatched()
        {
            for (int a = 0; a < 15; a++)
            {
                API.EnableDispatchService(a, false);
            }
        }

        public async Task DisableWanted()
        {
            if (Game.Player.WantedLevel > 0)
            {
                Game.Player.WantedLevel = 0;
            }
            await BaseScript.Delay(500);
        }
    }
}
