using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

//
// USED NATIVEUI SCALEFORM AS AN EXAMPLE OF HOW TO CALL SCALEFORMS IN C#
//

namespace FiveZ.Client.Classes.Managers
{
    public class ScaleformManager
    {
        private static Scaleform _sc;
        private static int _start;
        private static int _timer;

        public static async Task Load()
        {
            if (_sc != null) return;
            _sc = new Scaleform("MP_BIG_MESSAGE_FREEMODE");
            int timeout = 1000;
            int start = Game.GameTime;
            while (!_sc.IsLoaded && Game.GameTime < start + timeout)
            {
                Screen.ShowSubtitle("LOADING SCALEFORM");
                await BaseScript.Delay(0);
            }
        }

        public static void Dispose()
        {
            Main.GetInstance().UnregisterTickHandler(DisplayScaleform);
            _sc.Dispose();
            _sc = null;
        }

        public static async void ShowRankupMessage(string _message, string _subtitle, int _rank, int _time = 5000)
        {
            await Load();
            _start = Game.GameTime;
            _sc.CallFunction("SHOW_BIG_MP_MESSAGE", _message, _subtitle, _rank, "", "");
            _timer = _time;
            Main.GetInstance().RegisterTickHandler(DisplayScaleform);
        }

        private static async Task DisplayScaleform()
        {
            if (_start + _timer < Game.GameTime) { Dispose(); }
            if (_sc == null) { Dispose(); } else { _sc.Render2D(); }
            await Task.FromResult(0);
        }
    }
}
