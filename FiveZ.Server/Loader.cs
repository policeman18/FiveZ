using FiveZ.Server.Classes;
using FiveZ.Server.Classes.Managers;

namespace FiveZ.Server
{
    public class Loader
    {
        public static void Init()
        {
            Utils.WriteLine("Loading Classes...");

            new ConfigManager();
            new ConnectionManager();
            new SessionManager();

            Utils.WriteLine("Classes Loaded!");
        }
    }
}
