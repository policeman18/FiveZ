using System;
using CitizenFX.Core;

namespace FiveZ.Server
{
    public class Utils
    {
        public static void WriteLine(string _msg)
        {
            Debug.WriteLine($"^1[Server FiveZ]: ^0{_msg}");
        }

        public static void Throw(Exception ex)
        {
            Debug.WriteLine($"^1[Server FiveZ Exception]: {ex.Message} | {ex.StackTrace}^0");
        }
    }
}
