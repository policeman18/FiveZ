using FiveZ.Client.Classes;
using FiveZ.Shared;

namespace FiveZ.Client
{
    public class Loader
    {
        public static void Init()
        {
            Utils.WriteLine("Loading Classes...");

            new SessionManager();

            Utils.WriteLine("Classes Loaded!");
        }
    }
}
