using System;
using System.Dynamic;
using CitizenFX.Core;
using FiveZ.Server.Models;

namespace FiveZ.Server.Classes.Managers
{
    public class ConnectionManager
    {

        public static bool isServerWhitlisted = true;
        public static int serverConnectionWait = 10;

        public ConnectionManager()
        {
            Main.GetInstance().RegisterEventHandler("playerConnecting", new Action<Player, string, CallbackDelegate, ExpandoObject>(PlayerConnecting));
        }

        private async void PlayerConnecting([FromSource] Player _player, string _playerName, dynamic _setKickReason, dynamic _deferrals)
        {
            _deferrals.defer();
            for (int a = 0; a < serverConnectionWait; a++)
            {
                _deferrals.update($"Please wait {serverConnectionWait - a} seconds.");
                await BaseScript.Delay(1000);
            }

            Tuple<bool, User> foundUser = Database.GetPlayerUser(_player);
            if (!foundUser.Item1)
            {

                if (isServerWhitlisted)
                {
                    Database.CreatePlayerUser(_player);
                    _deferrals.done($"You are not whitelisted.");
                    return;
                }

                _deferrals.done();
            }
            else
            {
                User u = foundUser.Item2;

                if (u.BanData.IsBanned)
                {
                    TimeSpan timeLeft = u.BanData.BanTime - DateTime.Now;
                    _deferrals.done($"You have been banned for [{u.BanData.Reason}] by [{u.BanData.BannedBy}] for [ Days: {timeLeft.Days} Hours: {timeLeft.Hours} Minutes:{timeLeft.Minutes} Seconds:{timeLeft.Seconds} ]");
                    return;
                }

                if (isServerWhitlisted)
                {
                    if (!u.IsWhitelisted)
                    {
                        _deferrals.done($"You are not whitelisted.");
                    }
                }

                _deferrals.done();
            }
        }
    }
}
