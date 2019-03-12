using CitizenFX.Core;
using CitizenFX.Core.Native;
using FiveZ.Client.Classes.Managers;

namespace FiveZ.Client.Classes
{
    public class Actions
    {

        public static void HealPlayer(int _healAmount)
        {
            int CurrentHealth = Game.Player.Character.Health;
            int MaxHealth = Game.Player.Character.MaxHealth;
            int NewHealth = CurrentHealth + _healAmount;
            if (NewHealth > MaxHealth) { NewHealth = MaxHealth; }
            Game.Player.Character.Health = NewHealth;
        }

        public static void BoundPlayer()
        {
            
        }

        public static void PlayerEat(int _eatAmount)
        {
            SessionManager.PlayerSession.SpawnedCharacter.AddHunger(_eatAmount);
        }

        public static void PlayerDrink(int _drinkAmount)
        {
            SessionManager.PlayerSession.SpawnedCharacter.AddThirst(_drinkAmount);
        }

    }
}
