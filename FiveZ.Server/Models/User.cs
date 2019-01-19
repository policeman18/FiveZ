using System;
using CitizenFX.Core;
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
        public int Id { get; }
        public string Name { get; set; }
        public IdentifierCollection Identifiers { get; protected set; }
        public Permission PermissionFlag { get; protected set; }
        public BanStatus BanData { get; protected set; }
        public bool IsWhitelisted { get; protected set; }
        public DateTime LastPlayed { get; protected set; }

        public User(Player _player)
        {
            
        }

        public void SetPermission(Permission _perm)
        {
            this.PermissionFlag = _perm;
            // Update
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
            // Update
        }

        public void SetWhitelisted(bool _status)
        {
            this.IsWhitelisted = _status;
            // Update
        }

        public void SetLastPlayed()
        {
            this.LastPlayed = DateTime.Now;
            // Update
        }
    }
}
