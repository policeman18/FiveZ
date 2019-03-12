using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using MenuAPI;
using FiveZ.Shared.Models;

namespace FiveZ.Client.Menus
{
    public class CharacterModifier
    {
        // Character
        private static Character CurrentCharacter = new Character();

        // Menus
        private static Menu Menu = new Menu("FiveZ", "Character Editor");
        private static MenuItem ParentsMenuButton = new MenuItem("Parents Menu");
        private static MenuItem ShapeMenuButton = new MenuItem("Shape Menu");
        private static MenuItem AppearanceMenuButton = new MenuItem("Appearance Menu");
        private static MenuItem FinishEditingButton = new MenuItem("~g~Finish Character", "~r~REMINDER! You are only allowed to set this once!!!");

        // Parents Menu
        private static Menu ParentsMenu = new Menu("FiveZ", "Parent Blend");
        private static MenuSliderItem ParentFatherFace = new MenuSliderItem("Fathers Face", 0, 45, 0);
        private static MenuSliderItem ParentMotherFace = new MenuSliderItem("Mothers Face", 0, 45, 0);
        private static MenuSliderItem ParentsMenuMixBar = new MenuSliderItem("Parent Mix", 0, 10, 5) { ShowDivider = true };

        // Shape Menu
        private static Dictionary<int, float> FaceFeatures = new Dictionary<int, float>()
        {
            [0] = 0f, [1] = 0f, [2] = 0f, [3] = 0f, [4] = 0f, [5] = 0f, [6] = 0f, [7] = 0f, [8] = 0f, [9] = 0f,
            [10] = 0f, [11] = 0f, [12] = 0f, [13] = 0f, [14] = 0f, [15] = 0f, [16] = 0f, [17] = 0f, [18] = 0f, [19] = 0f
        };
        private static Menu ShapeMenu = new Menu("FiveZ", "Face Shape");
        private static MenuSliderItem NoseWidth = new MenuSliderItem("Nose Width", -10, 10, 0) { ShowDivider = true };
        private static MenuSliderItem NosePeakHeight = new MenuSliderItem("Nose Peak Height", -10, 10, 0) { ShowDivider = true };
        private static MenuSliderItem NosePeakLength = new MenuSliderItem("Node Peak Length", -10, 10, 0) { ShowDivider = true };
        private static MenuSliderItem NoseBoneHeight = new MenuSliderItem("Nose Bone Height", -10, 10, 0) { ShowDivider = true };
        private static MenuSliderItem NosePeakLowering = new MenuSliderItem("Nose Peak Lowering", -10, 10, 0) { ShowDivider = true };
        private static MenuSliderItem NoseBoneTwist = new MenuSliderItem("Nose Bone Twist", -10, 10, 0) { ShowDivider = true };
        private static MenuSliderItem EyeBrowHeight = new MenuSliderItem("Eye Brow Height", -10, 10, 0) { ShowDivider = true };
        private static MenuSliderItem EyeBrowLength = new MenuSliderItem("Eye Brow Length", -10, 10, 0) { ShowDivider = true };
        private static MenuSliderItem CheekBoneHeight = new MenuSliderItem("Cheek Bone Height", -10, 10, 0) { ShowDivider = true };
        private static MenuSliderItem CheekBoneWidth = new MenuSliderItem("Cheek Bone Width", -10, 10, 0) { ShowDivider = true };
        private static MenuSliderItem CheekWidth = new MenuSliderItem("Cheek Width", -10, 10, 0) { ShowDivider = true };
        private static MenuSliderItem EyeOpenings = new MenuSliderItem("Eye Openings", -10, 10, 0) { ShowDivider = true };
        private static MenuSliderItem LipThickness = new MenuSliderItem("Lip Thickness", -10, 10, 0) { ShowDivider = true };
        private static MenuSliderItem JawBoneWidth = new MenuSliderItem("Jaw Bone Width", -10, 10, 0) { ShowDivider = true };
        private static MenuSliderItem JawBoneLength = new MenuSliderItem("Jaw Bone Length", -10, 10, 0) { ShowDivider = true };
        private static MenuSliderItem ChinBoneLowering = new MenuSliderItem("Chin Bone Lowering", -10, 10, 0) { ShowDivider = true };
        private static MenuSliderItem ChinBoneLength = new MenuSliderItem("Chin Bone Length", -10, 10, 0) { ShowDivider = true };
        private static MenuSliderItem ChinBoneWidth = new MenuSliderItem("Chin Bone Width", -10, 10, 0) { ShowDivider = true };
        private static MenuSliderItem ChinHole = new MenuSliderItem("Chin Hole", -10, 10, 0) { ShowDivider = true };
        private static MenuSliderItem NeckThickness = new MenuSliderItem("Neck Thickness", -10, 10, 0) { ShowDivider = true };

        // Appearance Menu
        private static Menu AppearanceMenu = new Menu("FiveZ", "Appearance");
        private static MenuSliderItem AppearanceBlemishes = new MenuSliderItem("Blemishes", 0, 23, 0);
        private static MenuSliderItem AppearanceBlemishesOpacity = new MenuSliderItem("Blemishes Opacity", 0, 10, 0);
        private static MenuSliderItem AppearanceFacialHair = new MenuSliderItem("Facial Hair", 0, 28, 0);
        private static MenuSliderItem AppearanceFacialHairOpacity = new MenuSliderItem("Facial Hair Opacity", 0, 10, 0);
        private static MenuSliderItem AppearanceEyebrows = new MenuSliderItem("Eyebrows", 0, 33, 0);
        private static MenuSliderItem AppearanceEyebrowsOpacity = new MenuSliderItem("Eyebrow Opacity", 0, 10, 0);
        private static MenuSliderItem AppearanceAgeing = new MenuSliderItem("Ageing", 0, 14, 0);
        private static MenuSliderItem AppearanceAgeingOpacity = new MenuSliderItem("Ageing Opacity", 0, 10, 0);
        private static MenuSliderItem AppearanceComplexion = new MenuSliderItem("Complexion", 0, 11, 0);
        private static MenuSliderItem AppearanceComplexionOpacity = new MenuSliderItem("Complexion Opacity", 0, 10, 0);
        private static MenuSliderItem AppearanceSunDamage = new MenuSliderItem("Sun Damage", 0, 10, 0);
        private static MenuSliderItem AppearanceSunDamageOpacity = new MenuSliderItem("Sun Damage Opacity", 0, 10, 0);
        private static MenuSliderItem AppearanceHairStyles = new MenuSliderItem("Hair Styles", 0, 10, 0);
        private static MenuSliderItem AppearanceHairColors = new MenuSliderItem("Hair Colors", 0, 0, 0);
        private static MenuSliderItem AppearanceHairHighlights = new MenuSliderItem("Hair Highlights", 0, 0, 0);

        public static async void EnableCharacterModifier(Character _character)
        {
            // Character Setup
            CurrentCharacter = _character;
            World.RenderingCamera = World.CreateCamera(Game.Player.Character.Position, new Vector3(0f, 0f, 0f), 30f);
            await BaseScript.Delay(50);
            World.RenderingCamera.AttachTo(Game.Player.Character.Bones[Bone.SKEL_Head], new Vector3(0f, 2f, 0.5f));
            SetPedBlendData();
            CreateMenu();
        }

        private static void CreateMenu()
        {
            // Menu Settings
            MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Left;
            MenuController.MenuToggleKey = (Control)(-1);

            // Generate Menu
            MenuController.AddMenu(Menu);
            Menu.AddMenuItem(ParentsMenuButton); MenuController.AddSubmenu(Menu, ParentsMenu); MenuController.BindMenuItem(Menu, ParentsMenu, ParentsMenuButton);
            Menu.AddMenuItem(ShapeMenuButton); MenuController.AddSubmenu(Menu, ShapeMenu); MenuController.BindMenuItem(Menu, ShapeMenu, ShapeMenuButton);
            Menu.AddMenuItem(AppearanceMenuButton); MenuController.AddSubmenu(Menu, AppearanceMenu); MenuController.BindMenuItem(Menu, AppearanceMenu, AppearanceMenuButton);
            Menu.AddMenuItem(FinishEditingButton);
            Menu.OnItemSelect += (_menu, _item, _index) =>
            {
                if (_item == FinishEditingButton)
                {
                    CurrentCharacter.SetNoLongerNew();
                    Main.TriggerServerEvent("FiveZ:FinishCharacterEditing", Newtonsoft.Json.JsonConvert.SerializeObject(CurrentCharacter));
                    DisableCharacterModifier();
                }
            };

            // Generate Parents Menu
            ParentsMenu.AddMenuItem(ParentFatherFace);
            ParentsMenu.AddMenuItem(ParentMotherFace);
            ParentsMenu.AddMenuItem(ParentsMenuMixBar);
            ParentsMenu.OnSliderPositionChange += (_menu, _sliderItem, _oldPosition, _newPosition, _itemIndex) =>
            {
                if (_sliderItem == ParentFatherFace)
                {
                    CurrentCharacter.SetParents(_newPosition, CurrentCharacter.Parents.Mother, CurrentCharacter.Parents.Mix);
                    SetPedBlendData();
                }
                else if (_sliderItem == ParentMotherFace)
                {
                    CurrentCharacter.SetParents(CurrentCharacter.Parents.Father, _newPosition, CurrentCharacter.Parents.Mix);
                    SetPedBlendData();
                }
                else if (_sliderItem == ParentsMenuMixBar)
                {
                    CurrentCharacter.SetParents(CurrentCharacter.Parents.Father, CurrentCharacter.Parents.Mother, (float)((_newPosition / 10m) * 1m));
                    SetPedBlendData();
                }
            };

            // Generate Shape Menu
            ShapeMenu.AddMenuItem(NoseWidth);
            ShapeMenu.AddMenuItem(NosePeakHeight);
            ShapeMenu.AddMenuItem(NosePeakLength);
            ShapeMenu.AddMenuItem(NoseBoneHeight);
            ShapeMenu.AddMenuItem(NosePeakLowering);
            ShapeMenu.AddMenuItem(NoseBoneTwist);
            ShapeMenu.AddMenuItem(EyeBrowHeight);
            ShapeMenu.AddMenuItem(EyeBrowLength);
            ShapeMenu.AddMenuItem(CheekBoneHeight);
            ShapeMenu.AddMenuItem(CheekBoneWidth);
            ShapeMenu.AddMenuItem(CheekWidth);
            ShapeMenu.AddMenuItem(EyeOpenings);
            ShapeMenu.AddMenuItem(LipThickness);
            ShapeMenu.AddMenuItem(JawBoneWidth);
            ShapeMenu.AddMenuItem(JawBoneLength);
            ShapeMenu.AddMenuItem(ChinBoneLowering);
            ShapeMenu.AddMenuItem(ChinBoneLength);
            ShapeMenu.AddMenuItem(ChinBoneWidth);
            ShapeMenu.AddMenuItem(ChinHole);
            ShapeMenu.AddMenuItem(NeckThickness);
            ShapeMenu.OnSliderPositionChange += (_menu, _sliderItem, _oldPosition, _newPosition, _itemIndex) =>
            {
                if (_sliderItem == NoseWidth)
                {
                    CurrentCharacter.FaceFeatures[0] = (float)((_newPosition / 10m) * 1m);
                    SetPedFaceFeature(0, CurrentCharacter.FaceFeatures[0]);
                }
                else if (_sliderItem == NosePeakHeight)
                {
                    CurrentCharacter.FaceFeatures[1] = (float)((_newPosition / 10m) * 1m);
                    SetPedFaceFeature(1, CurrentCharacter.FaceFeatures[1]);
                }
                else if (_sliderItem == NosePeakLength)
                {
                    CurrentCharacter.FaceFeatures[2] = (float)((_newPosition / 10m) * 1m);
                    SetPedFaceFeature(2, CurrentCharacter.FaceFeatures[2]);
                }
                else if (_sliderItem == NoseBoneHeight)
                {
                    CurrentCharacter.FaceFeatures[3] = (float)((_newPosition / 10m) * 1m);
                    SetPedFaceFeature(3, CurrentCharacter.FaceFeatures[3]);
                }
                else if (_sliderItem == NosePeakLowering)
                {
                    CurrentCharacter.FaceFeatures[4] = (float)((_newPosition / 10m) * 1m);
                    SetPedFaceFeature(4, CurrentCharacter.FaceFeatures[4]);
                }
                else if (_sliderItem == NoseBoneTwist)
                {
                    CurrentCharacter.FaceFeatures[5] = (float)((_newPosition / 10m) * 1m);
                    SetPedFaceFeature(5, CurrentCharacter.FaceFeatures[5]);
                }
                else if (_sliderItem == EyeBrowHeight)
                {
                    CurrentCharacter.FaceFeatures[6] = (float)((_newPosition / 10m) * 1m);
                    SetPedFaceFeature(6, CurrentCharacter.FaceFeatures[6]);
                }
                else if (_sliderItem == EyeBrowLength)
                {
                    CurrentCharacter.FaceFeatures[7] = (float)((_newPosition / 10m) * 1m);
                    SetPedFaceFeature(7, CurrentCharacter.FaceFeatures[7]);
                }
                else if (_sliderItem == CheekBoneHeight)
                {
                    CurrentCharacter.FaceFeatures[8] = (float)((_newPosition / 10m) * 1m);
                    SetPedFaceFeature(8, CurrentCharacter.FaceFeatures[8]);
                }
                else if (_sliderItem == CheekBoneWidth)
                {
                    CurrentCharacter.FaceFeatures[9] = (float)((_newPosition / 10m) * 1m);
                    SetPedFaceFeature(9, CurrentCharacter.FaceFeatures[9]);
                }
                else if (_sliderItem == CheekWidth)
                {
                    CurrentCharacter.FaceFeatures[10] = (float)((_newPosition / 10m) * 1m);
                    SetPedFaceFeature(10, CurrentCharacter.FaceFeatures[10]);
                }
                else if (_sliderItem == EyeOpenings)
                {
                    CurrentCharacter.FaceFeatures[11] = (float)((_newPosition / 10m) * 1m);
                    SetPedFaceFeature(11, CurrentCharacter.FaceFeatures[11]);
                }
                else if (_sliderItem == LipThickness)
                {
                    CurrentCharacter.FaceFeatures[12] = (float)((_newPosition / 10m) * 1m);
                    SetPedFaceFeature(12, CurrentCharacter.FaceFeatures[12]);
                }
                else if (_sliderItem == JawBoneWidth)
                {
                    CurrentCharacter.FaceFeatures[13] = (float)((_newPosition / 10m) * 1m);
                    SetPedFaceFeature(13, CurrentCharacter.FaceFeatures[13]);
                }
                else if (_sliderItem == JawBoneLength)
                {
                    CurrentCharacter.FaceFeatures[14] = (float)((_newPosition / 10m) * 1m);
                    SetPedFaceFeature(14, CurrentCharacter.FaceFeatures[14]);
                }
                else if (_sliderItem == ChinBoneLowering)
                {
                    CurrentCharacter.FaceFeatures[15] = (float)((_newPosition / 10m) * 1m);
                    SetPedFaceFeature(15, CurrentCharacter.FaceFeatures[15]);
                }
                else if (_sliderItem == ChinBoneLength)
                {
                    CurrentCharacter.FaceFeatures[16] = (float)((_newPosition / 10m) * 1m);
                    SetPedFaceFeature(16, CurrentCharacter.FaceFeatures[16]);
                }
                else if (_sliderItem == ChinBoneWidth)
                {
                    CurrentCharacter.FaceFeatures[17] = (float)((_newPosition / 10m) * 1m);
                    SetPedFaceFeature(17, CurrentCharacter.FaceFeatures[17]);
                }
                else if (_sliderItem == ChinHole)
                {
                    CurrentCharacter.FaceFeatures[18] = (float)((_newPosition / 10m) * 1m);
                    SetPedFaceFeature(18, CurrentCharacter.FaceFeatures[18]);
                }
                else if (_sliderItem == NeckThickness)
                {
                    CurrentCharacter.FaceFeatures[19] = (float)((_newPosition / 10m) * 1m);
                    SetPedFaceFeature(19, CurrentCharacter.FaceFeatures[19]);
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
            AppearanceMenu.AddMenuItem(AppearanceBlemishes);
            AppearanceMenu.AddMenuItem(AppearanceBlemishesOpacity);
            AppearanceMenu.AddMenuItem(AppearanceFacialHair);
            AppearanceMenu.AddMenuItem(AppearanceFacialHairOpacity);
            AppearanceMenu.AddMenuItem(AppearanceEyebrows);
            AppearanceMenu.AddMenuItem(AppearanceEyebrowsOpacity);
            AppearanceMenu.AddMenuItem(AppearanceAgeing);
            AppearanceMenu.AddMenuItem(AppearanceAgeingOpacity);
            AppearanceMenu.AddMenuItem(AppearanceComplexion);
            AppearanceMenu.AddMenuItem(AppearanceComplexionOpacity);
            AppearanceMenu.AddMenuItem(AppearanceSunDamage);
            AppearanceMenu.AddMenuItem(AppearanceSunDamageOpacity);
            AppearanceMenu.AddMenuItem(AppearanceHairStyles);
            AppearanceMenu.AddMenuItem(AppearanceHairColors);
            AppearanceMenu.AddMenuItem(AppearanceHairHighlights);
            AppearanceMenu.OnSliderPositionChange += (_menu, _sliderItem, _oldPosition, _newPosition, _itemIndex) =>
            {
                if (_sliderItem == AppearanceHairStyles)
                {
                    CurrentCharacter.Appearance.SetHairStyle(_newPosition);
                    Game.Player.Character.Style[PedComponents.Hair].SetVariation(CurrentCharacter.Appearance.HairStyle, CurrentCharacter.Appearance.HairColor);
                    ResetHairColors();
                }
                else if (_sliderItem == AppearanceHairColors)
                {
                    SetPedHairColor(_newPosition);
                }
                else if (_sliderItem == AppearanceHairHighlights)
                {
                    SetPedHairHighlightColor(_newPosition);
                }
                else if (_sliderItem == AppearanceBlemishes)
                {
                    SetOverlay(0, _newPosition, CurrentCharacter.Appearance.Overlays[0].Opacity);
                }
                else if (_sliderItem == AppearanceBlemishesOpacity)
                {
                    SetOverlay(0, CurrentCharacter.Appearance.Overlays[0].Index, (float)((_newPosition / 10m) * 1m));
                }
                else if (_sliderItem == AppearanceFacialHair)
                {
                    SetOverlay(1, _newPosition, CurrentCharacter.Appearance.Overlays[1].Opacity, true);
                }
                else if (_sliderItem == AppearanceFacialHairOpacity)
                {
                    SetOverlay(1, CurrentCharacter.Appearance.Overlays[1].Index, (float)((_newPosition / 10m) * 1m));
                }
                else if (_sliderItem == AppearanceEyebrows)
                {
                    SetOverlay(2, _newPosition, CurrentCharacter.Appearance.Overlays[2].Opacity, true);
                }
                else if (_sliderItem == AppearanceEyebrowsOpacity)
                {
                    SetOverlay(2, CurrentCharacter.Appearance.Overlays[2].Index, (float)((_newPosition / 10m) * 1m));
                }
                else if (_sliderItem == AppearanceAgeing)
                {
                    SetOverlay(3, _newPosition, CurrentCharacter.Appearance.Overlays[3].Opacity, false);
                }
                else if (_sliderItem == AppearanceAgeingOpacity)
                {
                    SetOverlay(3, CurrentCharacter.Appearance.Overlays[3].Index, (float)((_newPosition / 10m) * 1m));
                }
                else if (_sliderItem == AppearanceComplexion)
                {
                    SetOverlay(6, _newPosition, CurrentCharacter.Appearance.Overlays[6].Opacity);
                }
                else if (_sliderItem == AppearanceComplexionOpacity)
                {
                    SetOverlay(6, CurrentCharacter.Appearance.Overlays[6].Index, (float)((_newPosition / 10m) * 1m));
                }
                else if (_sliderItem == AppearanceSunDamage)
                {
                    SetOverlay(7, _newPosition, CurrentCharacter.Appearance.Overlays[7].Opacity);
                }
                else if (_sliderItem == AppearanceSunDamageOpacity)
                {
                    SetOverlay(7, CurrentCharacter.Appearance.Overlays[7].Index, (float)((_newPosition / 10m) * 1m));
                }
            };

            Main.GetInstance().RegisterTickHandler(KeepMenuEnabled);
        }

        public static void DisableCharacterModifier()
        {
            Main.GetInstance().UnregisterTickHandler(KeepMenuEnabled);
            MenuController.Menus.Remove(Menu);
            World.RenderingCamera = null;
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

            if (Game.IsControlPressed(0, Control.MoveLeftOnly))
            {
                Game.Player.Character.Task.LookAt(Game.Player.Character.GetOffsetPosition(new Vector3(2f, 2f, 1f)), 100);
            }
            else if (Game.IsControlPressed(0, Control.MoveRightOnly))
            {
                Game.Player.Character.Task.LookAt(Game.Player.Character.GetOffsetPosition(new Vector3(-2f, 2f, 1f)), 100);
            }
            else
            {
                Game.Player.Character.Task.LookAt(Game.Player.Character.GetOffsetPosition(new Vector3(0f, 2f, 1f)), 100);
            }

            Game.Player.Character.Task.ClearAll();

            PlayerList players = new PlayerList();
            foreach (Player p in players)
            {
                if (p.Handle != Game.Player.Handle)
                {
                    API.SetEntityLocallyInvisible(p.Character.Handle);
                    Game.Player.Character.SetNoCollision(p.Character, true);
                }
            }

            await Task.FromResult(0);
        }

        private static void SetPedBlendData()
        {
            API.SetPedHeadBlendData(Game.Player.Character.Handle, CurrentCharacter.Parents.Father, CurrentCharacter.Parents.Mother, 0, CurrentCharacter.Parents.Father, CurrentCharacter.Parents.Mother, 0, CurrentCharacter.Parents.Mix, CurrentCharacter.Parents.Mix, -1f, false);
        }

        private static void SetPedHairColor(int _color)
        {
            CurrentCharacter.Appearance.SetHairColor(_color);
            API.SetPedHairColor(Game.Player.Character.Handle, CurrentCharacter.Appearance.HairColor, CurrentCharacter.Appearance.HairHighlightColor);
            SetOverlay(1, CurrentCharacter.Appearance.Overlays[1].Index, CurrentCharacter.Appearance.Overlays[1].Opacity, true);
            SetOverlay(2, CurrentCharacter.Appearance.Overlays[2].Index, CurrentCharacter.Appearance.Overlays[2].Opacity, true);
        }

        private static void SetPedHairHighlightColor(int _color)
        {
            CurrentCharacter.Appearance.SetHairHighlightColor(_color);
            API.SetPedHairColor(Game.Player.Character.Handle, CurrentCharacter.Appearance.HairColor, CurrentCharacter.Appearance.HairHighlightColor);
        }

        private static void ResetHairColors()
        {
            CurrentCharacter.Appearance.SetHairColor(0);
            CurrentCharacter.Appearance.SetHairHighlightColor(0);

            AppearanceMenu.RemoveMenuItem(AppearanceHairColors);
            AppearanceMenu.RemoveMenuItem(AppearanceHairHighlights);

            int updatedNumHairColors = API.GetNumHairColors();
            AppearanceHairColors = new MenuSliderItem("Hair Colors", CurrentCharacter.Appearance.HairColor, updatedNumHairColors, 0);
            AppearanceHairHighlights = new MenuSliderItem("Hair Highlights", CurrentCharacter.Appearance.HairHighlightColor, updatedNumHairColors, 0);

            AppearanceMenu.AddMenuItem(AppearanceHairColors);
            AppearanceMenu.AddMenuItem(AppearanceHairHighlights);
            SetPedHairColor(0);
            SetPedHairHighlightColor(0);
        }

        private static void SetOverlay(int _id, int _index, float _opacity, bool isHairColor = false)
        {
            API.SetPedHeadOverlay(Game.Player.Character.Handle, _id, _index, _opacity);
            CurrentCharacter.Appearance.SetOverlay(_id, _index, _opacity);
            if (isHairColor)
            {
                API.SetPedHeadOverlayColor(Game.Player.Character.Handle, _id, 1, CurrentCharacter.Appearance.HairColor, CurrentCharacter.Appearance.HairHighlightColor);
            }
        }

        private static void SetPedFaceFeature(int _index, float _scale)
        {
            API.SetPedFaceFeature(Game.Player.Character.Handle, _index, _scale);
        }
    }
}
