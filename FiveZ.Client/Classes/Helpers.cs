using System;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace FiveZ.Client.Classes
{
    public class Helpers
    {

        public static Blip CreateBlip(Vector3 _pos, BlipSprite _sprite, BlipColor _color, string _label, bool _isShortRange, float _scale = 0.75f)
        {
            Blip b = World.CreateBlip(_pos);
            b.Sprite = _sprite;
            b.Color = _color;
            b.IsShortRange = _isShortRange;
            b.Name = _label;
            b.Scale = _scale;
            return b;
        }

        public static void Draw3DText(Vector3 _pos, Color _color, string _text, Font _font, float _scale = 1f, bool _dropshadow = false)
        {
            Vector3 p = API.GetGameplayCamCoords();
            float dist = World.GetDistance(p, _pos);
            float scale = (1 / dist) * 20;
            float fov = (1 / API.GetGameplayCamFov()) * 100;
            scale = scale * fov;
            API.SetTextScale(_scale * scale, _scale * scale);
            API.SetTextFont(Convert.ToInt32(_font));
            API.SetTextProportional(true);
            API.SetTextColour(_color.R, _color.G, _color.B, _color.A);
            if (_dropshadow)
            {
                API.SetTextDropshadow(1, 1, 1, 1, 255);
                API.SetTextDropShadow();
            }
            API.SetTextOutline();
            API.SetTextEntry("STRING");
            API.SetTextCentre(true);
            API.AddTextComponentString(_text);
            API.SetDrawOrigin(_pos.X, _pos.Y, _pos.Z, 0);
            API.DrawText(0f, 0f);
            API.ClearDrawOrigin();
        }

        public static async Task<Entity> CreateObjectFromWeaponHash(WeaponHash _hash)
        {
            Entity newObject = await World.CreateProp(new Model(_hash), Game.Player.Character.Position, true, true);
            return newObject;
        }

    }
}
