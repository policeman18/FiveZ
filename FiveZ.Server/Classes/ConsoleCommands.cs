using System;
using System.Collections.Generic;

namespace FiveZ.Server.Classes
{
    public class ConsoleCommands
    {

        public static bool ServerWipingInProgress = false;

        public ConsoleCommands()
        {
            Main.GetInstance().RegisterCommand("serverwipe", new Action<int, List<object>, string>(TriggerServerWipe), false);
        }

        public void TriggerServerWipe(int _source, List<object> _args, string _raw)
        {
            ServerWipingInProgress = true;

            // Wipe Inventories
            // Reset Items
            // Reset Player Clothing
        }

    }
}
