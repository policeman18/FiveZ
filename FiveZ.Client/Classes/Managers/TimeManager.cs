using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveZ.Client.Classes.Managers
{
    public class TimeManager
    {
        DateTime CurrentTime;

        public TimeManager()
        {
            Main.GetInstance().RegisterEventHandler("FiveZ:SetClientTime", new Action<string>(SetClientTime));
        }

        public void SetClientTime(string time)
        {
            CurrentTime = Newtonsoft.Json.JsonConvert.DeserializeObject<DateTime>(time);
        }
    }
}
