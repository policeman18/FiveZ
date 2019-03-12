using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace FiveZ.Client.Classes
{
    public class DevTools
    {

        public DevTools()
        {
            Main.GetInstance().RegisterEventHandler("FiveZ:ToggleDeveloperNoclip", new Action(ToggleNoclip));
            Main.GetInstance().RegisterEventHandler("FiveZ:TeleportDeveloperMarker", new Action(TeleportMarker));

            // Make this restricted later
            API.RegisterCommand("vehicle", new Action<int, List<object>, string>( async (_source, _args, _raw) =>
            {
                if (_args.Count > 0)
                {
                    Model m = new Model(_args[0].ToString());
                    if (!m.IsValid) return;
                    Vehicle v = await World.CreateVehicle(m, Game.Player.Character.GetOffsetPosition(new Vector3(0f, 5f, 0f)), Game.Player.Character.Heading);
                    v.PlaceOnGround();
                    Game.Player.Character.SetIntoVehicle(v, VehicleSeat.Driver);
                }
                else
                {
                    Model m = new Model("f620");
                    Vehicle v = await World.CreateVehicle(m, Game.Player.Character.GetOffsetPosition(new Vector3(0f, 5f, 0f)), Game.Player.Character.Heading);
                    v.PlaceOnGround();
                    Game.Player.Character.SetIntoVehicle(v, VehicleSeat.Driver);
                }
            }), false);

            Utils.WriteLine("DevTools Loaded");
        }

        // Noclip
        private bool NoclipEnabled = false;
        private Vector3 NoclipPosition;
        private float NoclipRotation;
        private Vector3 MoveForwardOffset = new Vector3(0f, -1f, -1f);
        private Vector3 MoveBackwardOffset = new Vector3(0f, 1f, -1f);
        private Vector3 MoveUpOffset = new Vector3(0f, 0f, 1f);
        private Vector3 MoveDownOffset = new Vector3(0f, 0f, -2f);
        private float TurnOffset = 5f;

        private void ToggleNoclip()
        {
            NoclipEnabled = !NoclipEnabled;
            if (NoclipEnabled)
            {
                NoclipPosition = Game.Player.Character.Position;
                NoclipRotation = Game.Player.Character.Heading;
                Main.GetInstance().RegisterTickHandler(NoclipTick);
            }
            else
            {
                Main.GetInstance().UnregisterTickHandler(NoclipTick);
            }
        }

        private async Task NoclipTick()
        {
            Game.Player.Character.Position = NoclipPosition;
            Game.Player.Character.Heading = NoclipRotation;

            if (Game.IsControlPressed(1, Control.MoveUpOnly))
            {
                NoclipPosition = Game.Player.Character.GetOffsetPosition(MoveForwardOffset);
            }
            else if (Game.IsControlPressed(1, Control.MoveDownOnly))
            {
                NoclipPosition = Game.Player.Character.GetOffsetPosition(MoveBackwardOffset);
            }

            if (Game.IsControlPressed(1, Control.MoveLeftOnly))
            {
                NoclipRotation = NoclipRotation + TurnOffset > 360f ? 0f : NoclipRotation + TurnOffset;
            }
            else if (Game.IsControlPressed(1, Control.MoveRightOnly))
            {
                NoclipRotation = NoclipRotation - TurnOffset < 0f ? 360f : NoclipRotation - TurnOffset;
            }

            if (Game.IsControlPressed(1, Control.Cover))
            {
                NoclipPosition = Game.Player.Character.GetOffsetPosition(MoveUpOffset);
            }
            else if (Game.IsControlPressed(1, Control.Pickup))
            {
                NoclipPosition = Game.Player.Character.GetOffsetPosition(MoveDownOffset);
            }

            await Task.FromResult(0);
        }

        // Teleporting Players N/A

        // Teleporting Marker
        private void TeleportMarker()
        {
            Blip waypoint = World.GetWaypointBlip();
            if (waypoint != null)
            {
                Vector3 WaypointPosition = waypoint.Position;
                Game.Player.Character.Position = new Vector3(WaypointPosition.X, WaypointPosition.Y, WaypointPosition.Z + 1000f);
                float distanceAbove = Game.Player.Character.HeightAboveGround;
                Vector3 NewPosition = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, -distanceAbove));
                Game.Player.Character.Position = NewPosition;
            }
        }

    }
}
