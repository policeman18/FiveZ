using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core.Native;
using FiveZ.Server.Classes.Managers;
using FiveZ.Server.Models;
using FiveZ.Shared.Enums;

namespace FiveZ.Server.Classes
{
    public class DevTools
    {

        public DevTools()
        {
            //API.RegisterCommand("", new Action<int, List<object>, string>(), false);
            API.RegisterCommand("devnoclip", new Action<int, List<object>, string>(ToggleDevNoclip), false);
            API.RegisterCommand("devtpmarker", new Action<int, List<object>, string>(TeleportMarker), false);
            //API.RegisterCommand("devtpplayer", new Action<int, List<object>, string>(), false);

            Utils.WriteLine("DevTools Loaded");
        }

        private void ToggleDevNoclip(int _source, List<object> _args, string _raw)
        {
            Session playersession = SessionManager.Sessions.Find(s => s.Player.Handle == _source.ToString());
            Permission[] allowedPerms = new Permission[] { Permission.Developer };
            bool isAllowed = HasPermission(allowedPerms, playersession.User.PermissionFlag);
            if (isAllowed)
            {
                playersession.Player.TriggerEvent("FiveZ:ToggleDeveloperNoclip");
            }
            else
            {
                playersession.Player.TriggerEvent("chatMessage", "^1You do not have permission to noclip.");
            }
        }

        private void TeleportMarker(int _source, List<object> _args, string _raw)
        {
            Session playersession = SessionManager.Sessions.Find(s => s.Player.Handle == _source.ToString());
            Permission[] allowedPerms = new Permission[] { Permission.Developer, Permission.Administrator, Permission.Owner };
            bool isAllowed = HasPermission(allowedPerms, playersession.User.PermissionFlag);
            if (isAllowed)
            {
                playersession.Player.TriggerEvent("FiveZ:TeleportDeveloperMarker");
            }
            else
            {
                playersession.Player.TriggerEvent("chatMessage", "^1You do not have permission to teleport.");
            }
        }

        private void BringPlayer(int _source, List<object> _args, string _raw)
        {
            Session playersession = SessionManager.Sessions.Find(s => s.Player.Handle == _source.ToString());
            Permission[] allowedPerms = new Permission[] { Permission.Developer, Permission.Administrator, Permission.Owner };
            bool isAllowed = HasPermission(allowedPerms, playersession.User.PermissionFlag);
            if (isAllowed)
            {

            }
            else
            {
                playersession.Player.TriggerEvent("chatMessage", "^1You do not have permission to teleport.");
            }
        }

        private void TeleportToPlayer(int _source, List<object> _args, string _raw)
        {
            Session playersession = SessionManager.Sessions.Find(s => s.Player.Handle == _source.ToString());
            Permission[] allowedPerms = new Permission[] { Permission.Developer, Permission.Administrator, Permission.Owner };
            bool isAllowed = HasPermission(allowedPerms, playersession.User.PermissionFlag);
            if (isAllowed)
            {

            }
            else
            {
                playersession.Player.TriggerEvent("chatMessage", "^1You do not have permission to teleport.");
            }
        }

        private bool HasPermission(Permission[] _allowedPerms, Permission _playerPermission)
        {
            foreach(Permission p in _allowedPerms)
            {
                if (p == _playerPermission)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
