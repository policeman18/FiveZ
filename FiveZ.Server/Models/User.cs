using System;
using CitizenFX.Core;
using FiveZ.Server.Classes;
using FiveZ.Shared.Enums;

namespace FiveZ.Server.Models
{
    public class BanStatus
    {
        public bool IsBanned { get; set; } = false;
        public string Reason { get; set; } = "";
        public DateTime BanTime { get; set; }
        public string BannedBy { get; set; } = "";
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Identifier { get; protected set; }
        public Permission PermissionFlag { get; protected set; }
        public BanStatus BanData { get; protected set; }
        public bool IsWhitelisted { get; protected set; }
        public DateTime LastPlayed { get; protected set; }

        public User CreatePlayerUser(Player _player)
        {
            this.Name = _player.Name;
            this.Identifier = _player.Identifiers["license"];
            this.PermissionFlag = Permission.User;
            this.BanData = new BanStatus();
            this.IsWhitelisted = false;
            this.LastPlayed = DateTime.Now;
            return this;
        }

        public void SetPermission(Permission _perm)
        {
            this.PermissionFlag = _perm;
            Database.UpdatePlayerUser(this);
        }

        public void SetBan(Player _player, string _reason, string _banType, int _banCount, string _bannerName)
        {
            this.BanData.IsBanned = true;
            this.BanData.Reason = _reason;
            DateTime banTime = new DateTime();
            switch (_banType)
            {
                case "second":
                    banTime.AddSeconds(_banCount);
                    break;
                case "minute":
                    banTime.AddMinutes(_banCount);
                    break;
                case "hour":
                    banTime.AddHours(_banCount);
                    break;
                case "day":
                    banTime.AddDays(_banCount);
                    break;
                case "month":
                    banTime.AddMonths(_banCount);
                    break;
                default:
                    banTime.AddMinutes(_banCount);
                    break;
            }
            this.BanData.BanTime = banTime;
            this.BanData.BannedBy = _bannerName;
            Database.UpdatePlayerUser(this);
            SessionManager.Sessions.Find(s => s.User.Id == this.Id).Drop($"You have been banned from the server until {this.BanData.BanTime}");
        }

        public void SetWhitelisted(bool _status)
        {
            this.IsWhitelisted = _status;
            Database.UpdatePlayerUser(this);
        }

        public void SetLastPlayed()
        {
            this.LastPlayed = DateTime.Now;
            Database.UpdatePlayerUser(this);
        }
    }
}
