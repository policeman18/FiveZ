using System;
using CitizenFX.Core;
using FiveZ.Server.Classes.Managers;
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
        public string Identifier { get; set; }
        public Permission PermissionFlag { get; set; }
        public BanStatus BanData { get; set; }
        public bool IsWhitelisted { get; set; }
        public DateTime LastPlayed { get; set; }
    }
}
