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

        // Menus
        private static Menu Menu = new Menu("FiveZ", "Character Editor");
        private static MenuItem ParentsMenuButton = new MenuItem("Parents Menu");
        private static MenuItem ShapeMenuButton = new MenuItem("Shape Menu");
        private static MenuItem AppearanceMenuButton = new MenuItem("Appearance Menu");
        private static MenuItem FinishEditingButton = new MenuItem("~g~Finish Character", "~r~REMINDER! You are only allowed to set this once!!!");

        // Parents Menu
        private static int FatherFace = 0;
        private static int MotherFace = 0;
        private static float ParentMix = 0f;

        private static Menu ParentsMenu = new Menu("FiveZ", "Parent Blend");
        private static MenuSliderItem ParentFatherFace = new MenuSliderItem("Fathers Face", 0, 45, 0);
        private static MenuSliderItem ParentMotherFace = new MenuSliderItem("Mothers Face", 0, 45, 0);
        private static MenuSliderItem ParentsMenuMixBar = new MenuSliderItem("Parent Mix", 0, 10, 5) { ShowDivider = true };

        // Appearance Menu
        private static int HairStyle = 0;
        private static int HairColor = 0;

        private static MenuSliderItem AppearanceHairStyles = new MenuSliderItem("Hair Styles", 0, 10, 0);
        private static MenuSliderItem AppearanceHairColors = new MenuSliderItem("Hair Colors", 0, 0, 0);

        private static Menu AppearanceMenu = new Menu("FiveZ", "Appearance");

        // Shape Menu
        private static Menu ShapeMenu = new Menu("FiveZ", "Face Shape");

        public static async void EnableCharacterModifier(Genders _gender)
        {
            // Menu Settings
            MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Left;
            MenuController.MenuToggleKey = (Control)(-1);

            // Generate Menu
            MenuController.AddMenu(Menu);
            Menu.AddMenuItem(ParentsMenuButton); MenuController.AddSubmenu(Menu, ParentsMenu); MenuController.BindMenuItem(Menu, ParentsMenu, ParentsMenuButton);
            Menu.AddMenuItem(ShapeMenuButton); MenuController.AddSubmenu(Menu, ShapeMenu); MenuController.BindMenuItem(Menu, ShapeMenu, ShapeMenuButton);
            Menu.AddMenuItem(AppearanceMenuButton); MenuController.AddSubmenu(Menu, AppearanceMenu); MenuController.BindMenuItem(Menu, AppearanceMenu, AppearanceMenuButton);            Menu.AddMenuItem(FinishEditingButton);

            // Generate Parents Menu
            ParentsMenu.AddMenuItem(ParentFatherFace);
            ParentsMenu.AddMenuItem(ParentMotherFace);
            ParentsMenu.AddMenuItem(ParentsMenuMixBar);
            ParentsMenu.OnSliderPositionChange += (_menu, _sliderItem, _oldPosition, _newPosition, _itemIndex) =>
            {
                if (_sliderItem == ParentFatherFace)
                {
                    FatherFace = _newPosition;
                    SetPedBlendData();
                }
                else if (_sliderItem == ParentMotherFace)
                {
                    MotherFace = _newPosition;
                    SetPedBlendData();
                }
                else if (_sliderItem == ParentsMenuMixBar)
                {
                    ParentMix = (float)((_newPosition / 10m) * 1m);
                    SetPedBlendData();
                }
            };

            // Generate Appearance Menu
            if (Game.Player.Character.Gender == Gender.Male)
            {
                AppearanceHairStyles = new MenuSliderItem("Hair Styles", 0, 22, 0);
            }
            else
            {
                AppearanceHairStyles = new MenuSliderItem("Hair Styles", 0, 23, 0);
            }
            AppearanceMenu.AddMenuItem(AppearanceHairStyles);
            AppearanceMenu.AddMenuItem(AppearanceHairColors);
            AppearanceMenu.OnSliderPositionChange += (_menu, _sliderItem, _oldPosition, _newPosition, _itemIndex) =>
            {
                if (_sliderItem == AppearanceHairStyles)
                {
                    HairStyle = _newPosition;
                    Game.Player.Character.Style[PedComponents.Hair].SetVariation(HairStyle, HairColor);
                    AppearanceMenu.RemoveMenuItem(AppearanceHairColors);
                    AppearanceHairColors = new MenuSliderItem("Hair Colors", 0, Game.Player.Character.Style[PedComponents.Hair].TextureCount, 0);
                    AppearanceMenu.AddMenuItem(AppearanceHairColors);
                    HairColor = 0;
                }
                else if (_sliderItem == AppearanceHairColors)
                {
                    HairColor = _newPosition;
                    Game.Player.Character.Style[PedComponents.Hair].SetVariation(0, 0);
                    Game.Player.Character.Style[PedComponents.Hair].SetVariation(HairStyle, HairColor);
                }
            };

            // Generate Shape Menu


            // Character Setup
            Main.GetInstance().RegisterTickHandler(KeepMenuEnabled);
            await BaseScript.Delay(2000);
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

            World.RenderingCamera = World.CreateCamera(Game.Player.Character.Position, new Vector3(0f, 0f, 0f), 30f);
            await BaseScript.Delay(100);
            World.RenderingCamera.AttachTo(Game.Player.Character.Bones[Bone.SKEL_Head], new Vector3(0f, 2f, 0.5f));
        }

        public static void DisableCharacterModifier()
        {
            MenuController.Menus.Remove(Menu);
            Main.GetInstance().UnregisterTickHandler(KeepMenuEnabled);
            Menu.CloseMenu();
        }

        private static async Task KeepMenuEnabled()
        {
            if (!MenuController.IsAnyMenuOpen())
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

            //TestingClothingMethod(); // REMOVE LATER
            await Task.FromResult(0);
        }

        private static void SetPedBlendData()
        {
            API.SetPedHeadBlendData(Game.Player.Character.Handle, FatherFace, MotherFace, 0, FatherFace, MotherFace, 0, ParentMix, ParentMix, 0f, true);
        }

        // REMOVE LATER
        private static void TestingClothingMethod()
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
