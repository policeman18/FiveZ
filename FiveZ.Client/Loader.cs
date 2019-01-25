using FiveZ.Client.Classes;
using FiveZ.Client.Classes.Managers;
using FiveZ.Client.Classes.Player;
using FiveZ.Shared;

namespace FiveZ.Client
{
    public class Loader
    {
        public static void Init()
        {
            Utils.WriteLine("Loading Classes...");

            new SessionManager();
            new RemoveAI();
            new RemoveDispatch();
            new SpawnManager();

            Utils.WriteLine("Classes Loaded!");
        }
    }
}
