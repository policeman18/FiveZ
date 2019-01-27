using FiveZ.Client.Classes;
using FiveZ.Client.Classes.Managers;
using FiveZ.Client.Classes.Player;
using FiveZ.Client.Menus;
using FiveZ.Shared;

namespace FiveZ.Client
{
    public class Loader
    {
        public static void Init()
        {
            Utils.WriteLine("Loading Classes...");

            // Managers
            new SessionManager();
            new SpawnManager();
            
            // Player
            new RemoveAI();
            new RemoveDispatch();
            new RemoveHealthRegen();

            // Menus
            new CharacterModifier();

            Utils.WriteLine("Classes Loaded!");
        }
    }
}
