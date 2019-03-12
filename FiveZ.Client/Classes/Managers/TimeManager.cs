using System;

namespace FiveZ.Client.Classes.Managers
{
    public class TimeManager
    {
        DateTime CurrentTime;

        public TimeManager()
        {
            Main.GetInstance().RegisterEventHandler("FiveZ:SetClientTime", new Action<string>(SetClientTime));

            Utils.WriteLine("TimeManager Loaded");
        }

        public void SetClientTime(string time)
        {
            CurrentTime = Newtonsoft.Json.JsonConvert.DeserializeObject<DateTime>(time);
        }
    }
}
