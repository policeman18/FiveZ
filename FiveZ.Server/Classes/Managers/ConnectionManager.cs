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
            Utils.WriteLine("ConnectionManager Loaded");
        }

        private async void PlayerConnecting([FromSource] Player _player, string _playerName, dynamic _setKickReason, dynamic _deferrals)
        {
            _deferrals.defer();
            for (int a = 0; a < ConfigManager.ServerConfig.ServerConnectionDelayTime; a++)
            {
                _deferrals.update($"Please wait {ConfigManager.ServerConfig.ServerConnectionDelayTime - a} seconds.");
                await BaseScript.Delay(1000);
            }

            Tuple<bool, User> foundUser = new Session().GetPlayerUser(_player);
            if (!foundUser.Item1)
            {

                if (ConfigManager.ServerConfig.ServerWhitelisted)
                {
                    new Session().CreatePlayerUser(_player);
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

                if (ConfigManager.ServerConfig.ServerWhitelisted)
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
