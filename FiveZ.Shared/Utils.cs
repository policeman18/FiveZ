using System;
using CitizenFX.Core;

namespace FiveZ.Shared
{
    public class Utils
    {
        
        public static void WriteLine(string _msg)
        {
            Debug.WriteLine($"^1[FiveZ]: ^0{_msg}");
        }

        public static void Throw(Exception ex)
        {
            Debug.WriteLine($"^1[FiveZ Exception]: {ex.Message}^0");
        }

    }
}
