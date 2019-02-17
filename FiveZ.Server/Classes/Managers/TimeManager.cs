using System;
using CitizenFX.Core;
using FiveZ.Server.Models;

namespace FiveZ.Server.Classes.Managers
{
    public class TimeManager
    {
        public static DateTime CurrentTime;

        public TimeManager()
        {
            CurrentTime = DateTime.Now;
            IncreaseSecond();
        }

        public async void IncreaseSecond()
        {
            DateTime TimeStamp = DateTime.Now;
            CurrentTime = CurrentTime.AddMinutes(1);
            foreach(Session s in SessionManager.Sessions)
            {
                s.Player.TriggerEvent("FiveZ:SetClientTime", Newtonsoft.Json.JsonConvert.SerializeObject(CurrentTime));
            }

            while (DateTime.Now < TimeStamp.AddMinutes(1)) await BaseScript.Delay(1000);
            IncreaseSecond();
        }
    }
}
