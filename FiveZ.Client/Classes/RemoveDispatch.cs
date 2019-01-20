using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FiveZ.Shared;

namespace FiveZ.Client.Classes
{
    public class RemoveDispatch
    {
        public RemoveDispatch()
        {
            RemoveDispatched();
            Main.GetInstance().RegisterTickHandler(DisableWanted);
            Utils.WriteLine("RemoveDispatch Loaded");
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
