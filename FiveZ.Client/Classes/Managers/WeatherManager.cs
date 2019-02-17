using System;
using CitizenFX.Core;

namespace FiveZ.Client.Classes.Managers
{
    public class WeatherManager
    {
        public WeatherManager()
        {
            Main.GetInstance().RegisterEventHandler("FiveZ:SendClientWeather", new Action<int, int>(SendClientWeather));
        }

        public void SendClientWeather(int _current, int _last)
        {
            World.TransitionToWeather((Weather)_current, 1f);
            World.NextWeather = (Weather)_current;
        }
    }
}