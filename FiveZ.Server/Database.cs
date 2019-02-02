using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LiteDB;
using FiveZ.Server.Classes.Managers;
using FiveZ.Server.Models;
using FiveZ.Shared.Models;
using FiveZ.Shared.Enums;

namespace FiveZ.Server
{
    public class Database
    {

        public static readonly string DBPath = $"{API.GetResourcePath(API.GetCurrentResourceName())}/data/database.db";

    }
}
