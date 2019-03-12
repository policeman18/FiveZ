using System;
using CitizenFX.Core;
using FiveZ.Server.Models;

namespace FiveZ.Server.Classes.Managers
{
    public enum Weather
    {
        Unknown = -1,
        ExtraSunny = 0,
        Clear = 1,
        Clouds = 2,
        Smog = 3,
        Foggy = 4,
        Overcast = 5,
        Raining = 6,
        ThunderStorm = 7,
        Clearing = 8,
        Neutral = 9,
        Snowing = 10,
        Blizzard = 11,
        Snowlight = 12,
        Christmas = 13,
        Halloween = 14
    }

    public class WeatherManager
    {
        public bool firstSetWeather = true;
        public static Weather CurrentWeather;
        public static Weather LastWeather;

        public WeatherManager()
        {
            GenerateWeather();

            Utils.WriteLine("WeatherManager Loaded");
        }

        public async void GenerateWeather()
        {
            DateTime TimeStamp = DateTime.Now.Add(TimeSpan.FromMilliseconds(ConfigManager.WeatherConfig.WeatherSwitchTime * 60000));
            Weather RandomWeather = (Weather)new Random().Next(0, 14);
            if (firstSetWeather)
            {
                CurrentWeather = RandomWeather;
                firstSetWeather = false;
            }
            else
            {
                LastWeather = CurrentWeather;
                CurrentWeather = RandomWeather;
            }

            // Set Players Weather From Session
            foreach (Session s in SessionManager.Sessions)
            {
                s.Player.TriggerEvent("FiveZ:SendClientWeather", Convert.ToInt32(CurrentWeather), Convert.ToInt32(LastWeather));
            }

            while (DateTime.Now < TimeStamp) await BaseScript.Delay(1000);

            GenerateWeather();
        }
    }
}
