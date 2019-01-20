using FiveZ.Server.Classes;
using FiveZ.Shared;

namespace FiveZ.Server
{
    public class Loader
    {
        public static void Init()
        {
            Utils.WriteLine("Loading Classes...");

            new ConnectionManager();
            new SessionManager();

            Utils.WriteLine("Classes Loaded!");
        }
    }
}
