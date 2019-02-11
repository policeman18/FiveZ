using CitizenFX.Core;
using CitizenFX.Core.Native;

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

    }
}
