using System;
using CitizenFX.Core;

namespace FiveZ.Client
{
    public class Utils
    {
        public static void WriteLine(string _msg)
        {
            Debug.WriteLine($"[Client FiveZ]: {_msg}");
        }

        public static void Throw(Exception ex)
        {
            Debug.WriteLine($"[FiveZ Exception]: {ex.Message} | {ex.StackTrace}");
        }
    }
}
