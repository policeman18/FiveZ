using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using MenuAPI;
using FiveZ.Shared.Enums;

namespace FiveZ.Client.Menus
{
    public class CharacterModifier
    {

        private static Menu Menu = new Menu("FiveZ", "Character Editor");
        private static Menu Parents = new Menu("FiveZ", "Parent Blend");
        private static Menu Appearamce = new Menu("FiveZ", "Appearance");
        private static Menu FaceShape = new Menu("FiveZ", "Face Shape");

        public CharacterModifier()
        {
            // Nothing Yet. Can probably remove this later.
        }

        public static async void EnableCharacterModifier(Genders _gender)
        {
            MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Left;
            MenuController.MenuToggleKey = (Control)(-1);
            MenuController.AddMenu(Menu);
            Main.GetInstance().RegisterTickHandler(KeepMenuEnabled);

            await BaseScript.Delay(2000);

            // Set Default Clothing
            if (_gender == Genders.Male)
            {
                Game.Player.Character.Style[PedComponents.Torso].SetVariation(0, 0);
                Game.Player.Character.Style[PedComponents.Torso2].SetVariation(146, 0);
                Game.Player.Character.Style[PedComponents.Legs].SetVariation(5, 7);
                Game.Player.Character.Style[PedComponents.Special2].SetVariation(57, 0);
                Game.Player.Character.Style[PedComponents.Shoes].SetVariation(6, 0);
            }
            else
            {
                Game.Player.Character.Style[PedComponents.Torso].SetVariation(4, 0);
                Game.Player.Character.Style[PedComponents.Torso2].SetVariation(118, 0);
                Game.Player.Character.Style[PedComponents.Legs].SetVariation(66, 6);
                Game.Player.Character.Style[PedComponents.Special2].SetVariation(2, 0);
                Game.Player.Character.Style[PedComponents.Shoes].SetVariation(5, 0);
            }
        }

        public static void DisableCharacterModifier()
        {
            MenuController.Menus.Remove(Menu);
            Main.GetInstance().UnregisterTickHandler(KeepMenuEnabled);
            Menu.CloseMenu();
        }

        private void SetInheritance()
        {
            //API.SetPedHeadBlendData(Game.Player.Character.Handle, )
        }

        private static async Task KeepMenuEnabled()
        {
            if (MenuController.GetCurrentMenu() != Menu)
            {
                Menu.OpenMenu();
            }
            Game.DisableControlThisFrame(0, Control.MoveUpOnly);
            Game.DisableControlThisFrame(0, Control.MoveDownOnly);
            Game.DisableControlThisFrame(0, Control.MoveLeftOnly);
            Game.DisableControlThisFrame(0, Control.MoveRightOnly);
            Game.DisableControlThisFrame(0, Control.MoveLeftRight);
            Game.DisableControlThisFrame(0, Control.MoveUpDown);
            Game.DisableControlThisFrame(0, Control.MoveUp);
            Game.DisableControlThisFrame(0, Control.MoveDown);
            Game.DisableControlThisFrame(0, Control.MoveLeft);
            Game.DisableControlThisFrame(0, Control.MoveRight);

            //TestingClothingMethod();
            await Task.FromResult(0);
        }

        public static void TestingClothingMethod()
        {
            PedComponents c = PedComponents.Torso;
            int maxComponents = Game.Player.Character.Style[c].Count;
            int currentComponent = Game.Player.Character.Style[c].Index;
            int maxTextures = Game.Player.Character.Style[c].TextureCount;
            int currentComponentTexture = Game.Player.Character.Style[c].TextureIndex;

            if (Game.IsControlJustPressed(0, Control.PhoneRight))
            {
                if (currentComponent != maxComponents - 1)
                {
                    Game.Player.Character.Style[c].Index += 1;
                }
                else
                {
                    Game.Player.Character.Style[c].Index = 0;
                }
                Screen.ShowSubtitle($"~b~Next Component {Game.Player.Character.Style[c].Index}");
            }
            else if (Game.IsControlJustPressed(0, Control.PhoneLeft))
            {
                if (currentComponent != 0)
                {
                    Game.Player.Character.Style[c].Index -= 1;
                }
                else
                {
                    Game.Player.Character.Style[c].Index = maxComponents - 1;
                }
                Screen.ShowSubtitle($"~r~Previous Component {Game.Player.Character.Style[c].Index}");
            }
            else if (Game.IsControlJustPressed(0, Control.PhoneUp))
            {
                if (currentComponentTexture != maxTextures - 1)
                {
                    Game.Player.Character.Style[c].TextureIndex += 1;
                }
                else
                {
                    Game.Player.Character.Style[c].TextureIndex = 0;
                }
                Screen.ShowSubtitle($"~b~Next Texture {Game.Player.Character.Style[c].TextureIndex}");
            }
            else if (Game.IsControlJustPressed(0, Control.PhoneDown))
            {
                if (currentComponentTexture == 0)
                {
                    Game.Player.Character.Style[c].TextureIndex = maxTextures - 1;
                }
                else
                {
                    Game.Player.Character.Style[c].TextureIndex -= 1;
                }
                Screen.ShowSubtitle($"~r~Previous Texture {Game.Player.Character.Style[c].TextureIndex}");
            }
        }

    }
}
