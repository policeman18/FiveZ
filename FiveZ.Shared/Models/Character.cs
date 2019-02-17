using System;
using System.Collections.Generic;
using CitizenFX.Core;
using Newtonsoft.Json;
using FiveZ.Shared.Enums;

namespace FiveZ.Shared.Models
{
    /// <summary>
    ///     Character Parents Data Class
    /// </summary>
    public class CharacterParents
    {
        [JsonProperty]
        public int Father { get; protected set; } = 0;
        [JsonProperty]
        public int Mother { get; protected set; } = 0;
        [JsonProperty]
        public float Mix { get; protected set; } = 0.5f;

        // Sets Father Property
        public void SetFather(int _father)
        {
            this.Father = _father;
        }

        // Sets Mother Property
        public void SetMother(int _mother)
        {
            this.Mother = _mother;
        }

        // Sets Parent Mix Property
        public void SetMix(float _mix)
        {
            this.Mix = _mix;
        }

        // Base Constructor
        public CharacterParents() { }
    }

    /// <summary>
    ///     Character Overlay Data Class
    /// </summary>
    public class Overlay
    {
        [JsonProperty]
        public int Index { get; protected set; } = 0;
        [JsonProperty]
        public float Opacity { get; protected set; } = 0f;
        [JsonProperty]
        public bool IsHair { get; protected set; } = false;

        public Overlay() { }
        public Overlay(int _index, float _opacity, bool _isHair)
        {
            this.Index = _index;
            this.Opacity = _opacity;
            this.IsHair = _isHair;
        }

        public void SetIndex(int _index)
        {
            this.Index = _index;
        }

        public void SetOpacity(float _opacity)
        {
            this.Opacity = _opacity;
        }
    }

    /// <summary>
    ///     Character Appearance Data Class
    /// </summary>
    public class CharacterAppearance
    {
        // Hair Appearance
        [JsonProperty]
        public int HairStyle { get; protected set; } = 0;
        [JsonProperty]
        public int HairColor { get; protected set; } = 0;
        [JsonProperty]
        public int HairHighlightColor { get; protected set; } = 0;

        // Appearance Overlays
        [JsonProperty]
        public Dictionary<int, Overlay> Overlays { get; protected set; } = new Dictionary<int, Overlay>()
        {
            [0] = new Overlay(0, 0f, false),
            [1] = new Overlay(0, 0f, true),
            [2] = new Overlay(0, 0f, true),
            [3] = new Overlay(0, 0f, false),
            [4] = new Overlay(0, 0f, false),
            [5] = new Overlay(0, 0f, false),
            [6] = new Overlay(0, 0f, false),
            [7] = new Overlay(0, 0f, false),
            [8] = new Overlay(0, 0f, false),
            [9] = new Overlay(0, 0f, false),
            [10] = new Overlay(0, 0f, false),
            [11] = new Overlay(0, 0f, false),
            [12] = new Overlay(0, 0f, false)
        };

        public void SetHairStyle(int _style)
        {
            this.HairStyle = _style;
        }

        public void SetHairColor(int _color)
        {
            this.HairColor = _color;
        }

        public void SetHairHighlightColor(int _color)
        {
            this.HairHighlightColor = _color;
        }

        public void SetOverlay(int _key, int _index, float _opacity)
        {
            this.Overlays[_key].SetIndex(_index);
            this.Overlays[_key].SetOpacity(_opacity);
        }
        
        public CharacterAppearance() { }
    }

    /// <summary>
    ///     Character Skill Data Class
    /// </summary>
    public class CharacterSkill
    {
        [JsonProperty]
        public string Skill { get; set; }
        [JsonProperty]
        public int Level { get; set; }
        [JsonProperty]
        public int XP { get; set; }
        [JsonProperty]
        public int MaxLevel { get; set; }

        public CharacterSkill(string _skill, int _maxlevel = 100)
        {
            this.Skill = _skill;
            this.Level = 1;
            this.XP = 100;
            this.MaxLevel = _maxlevel;
        }
        public CharacterSkill() { }

        public Tuple<bool, string> AddExperience(int _xp)
        {
            this.XP += _xp;
            int newLevel = (int)(0.205f * Math.Sqrt(this.XP));
            if (newLevel != this.Level && newLevel > this.Level && this.Level != this.MaxLevel)
            {
                this.Level = newLevel;
                return Tuple.Create<bool, string>(true, this.Skill);
            }
            return Tuple.Create<bool, string>(false, this.Skill);
        }

        public void ExperienceNeeded()
        {
            // Returns the amount of XP needed for the next level
        }
    }

    /// <summary>
    ///     Character Inventory Item Data Class
    /// </summary>
    public class CharacterInventoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public int Level { get; set; }
        public string Action { get; set; }
        public object[] ActionArgs { get; set; }
        public Dictionary<string, object> Data = new Dictionary<string, object>();
    }

    /// <summary>
    ///     Character Inventory Data Class
    /// </summary>
    public class CharacterInventory
    {

        // Inventory Space
        [JsonProperty]
        public int InventorySpace { get; protected set; }

        [JsonProperty]
        public CharacterInventoryItem WeaponOne { get; protected set; }

        [JsonProperty]
        public CharacterInventoryItem WeaponTwo { get; protected set; }

        [JsonProperty]
        public CharacterInventoryItem WeaponThree { get; protected set; }

        [JsonProperty]
        public List<CharacterInventoryItem> Items { get; protected set; } = new List<CharacterInventoryItem>();

        // Base Constructor
        public CharacterInventory() { }

        // New Inventory Constructor
        public CharacterInventory(int _space)
        {
            this.InventorySpace = _space;
        }

        // Sets inventory max space
        public void SetInventorySpace(int _space)
        {
            this.InventorySpace = _space;
        }

        // Sets weapon slot one
        public void SetWeaponOne(CharacterInventoryItem _item)
        {
            this.WeaponOne = _item;
        }

        // Removes weapon slot one
        public void RemoveWeaponOne()
        {
            this.WeaponOne = null;
        }

        public void SetWeaponTwo(CharacterInventoryItem _item)
        {
            this.WeaponTwo = _item;
        }
        public void RemoveWeaponTwo()
        {
            this.WeaponTwo = null;
        }
        public void SetWeaponThree(CharacterInventoryItem _item)
        {
            this.WeaponThree = _item;
        }
        public void RemoveWeaponThree()
        {
            this.WeaponThree = null;
        }
        public void AddItem(CharacterInventoryItem _item)
        {
            this.Items.Add(_item);
        }
        public void DeleteItem(CharacterInventoryItem _item)
        {
            this.Items.Remove(_item);
        }
    }
    
    /// <summary>
    ///     Character Data Class
    /// </summary>
    public class Character
    {

        // Character Unique Identifier
        [JsonProperty]
        public int Id { get; protected set; }

        // Players Unique User Identifier
        [JsonProperty]
        public int UserId { get; protected set; }

        // Characters First Name
        [JsonProperty]
        public string FirstName { get; protected set; }

        // Characters Last Name
        [JsonProperty]
        public string LastName { get; protected set; }

        // Characters Gender
        [JsonProperty]
        public Genders Gender { get; protected set; }

        // Defines if the character is new or not
        [JsonProperty]
        public bool isNew { get; protected set; } = true;

        // Defines if the character is currently dead
        [JsonProperty]
        public bool isDead { get; protected set; } = true;

        // Defines characters last position on save
        [JsonProperty]
        public float[] LastPos { get; protected set; } = new float[] { 0f, 0f, 0f };

        // Defines characters model
        [JsonProperty]
        public string Model { get; protected set; }

        // Defines Characters Parents
        [JsonProperty]
        public CharacterParents Parents { get; protected set; } = new CharacterParents();

        // Defines Characters Facial Features
        [JsonProperty]
        public Dictionary<int, float> FaceFeatures { get; protected set; } = new Dictionary<int, float>()
        {
            [0] = 0f,
            [1] = 0f,
            [2] = 0f,
            [3] = 0f,
            [4] = 0f,
            [5] = 0f,
            [6] = 0f,
            [7] = 0f,
            [8] = 0f,
            [9] = 0f,
            [10] = 0f,
            [11] = 0f,
            [12] = 0f,
            [13] = 0f,
            [14] = 0f,
            [15] = 0f,
            [16] = 0f,
            [17] = 0f,
            [18] = 0f,
            [19] = 0f
        };

        // Defines Characters Appearance
        [JsonProperty]
        public CharacterAppearance Appearance { get; protected set; } = new CharacterAppearance();

        // Defines Characters Clothing
        [JsonProperty]
        public Dictionary<int, int[]> Clothing { get; protected set; } = new Dictionary<int, int[]>()
        {
            [0] = new int[] { 0, 0 },
            [1] = new int[] { 0, 0 },
            [2] = new int[] { 0, 0 },
            [3] = new int[] { 0, 0 },
            [4] = new int[] { 0, 0 },
            [5] = new int[] { 0, 0 },
            [6] = new int[] { 0, 0 },
            [7] = new int[] { 0, 0 },
            [8] = new int[] { 0, 0 },
            [9] = new int[] { 0, 0 },
            [10] = new int[] { 0, 0 },
            [11] = new int[] { 0, 0 }
        };

        // Defines Characters Props
        [JsonProperty]
        public Dictionary<int, int[]> Props { get; protected set; } = new Dictionary<int, int[]>()
        {
            [0] = new int[] { 0, 0 },
            [1] = new int[] { 0, 0 },
            [2] = new int[] { 0, 0 },
            [3] = new int[] { 0, 0 },
            [4] = new int[] { 0, 0 },
            [5] = new int[] { 0, 0 },
            [6] = new int[] { 0, 0 },
            [7] = new int[] { 0, 0 },
            [8] = new int[] { 0, 0 },
            [9] = new int[] { 0, 0 }
        };

        // Defines Characters Skills | Levels / XP
        [JsonProperty]
        public Dictionary<string, CharacterSkill> Skills { get; protected set; } = new Dictionary<string, CharacterSkill>()
        {
            ["stamina"] = new CharacterSkill("Stamina", 10),
            ["crafting"] = new CharacterSkill("Crafting", 25),
        };

        // Defines Characters Inventory | Weapons / Items
        [JsonProperty]
        public CharacterInventory Inventory { get; protected set; } = new CharacterInventory();

        // Defines Chracters Hunger
        [JsonProperty]
        public int Hunger { get; protected set; }

        // Defines Characters Thirst
        [JsonProperty]
        public int Thirst { get; protected set; }

        // Defines Characters Temperature
        [JsonProperty]
        public int Temperature { get; protected set; }

        // Defines Characters Sickness
        [JsonProperty]
        public int Sickness { get; protected set; }

        // Base Class Constructor
        public Character() {  }

        // New Character Constructor
        public Character(int _userID, string _first, string _last, Genders _gender)
        {
            this.UserId = _userID;
            this.FirstName = _first;
            this.LastName = _last;
            this.Gender = _gender;

            if (this.Gender == Genders.Male)
            {
                this.Model = "mp_m_freemode_01";
                this.Clothing[0] = new int[] { 0, 0 }; // FACE
                this.Clothing[1] = new int[] { 0, 0 }; // HEAD
                this.Clothing[2] = new int[] { 0, 0 }; // HAIR
                this.Clothing[3] = new int[] { 0, 0 }; // TORSO
                this.Clothing[4] = new int[] { 5, 7 }; // LEGS
                this.Clothing[5] = new int[] { 0, 0 }; // HANDS
                this.Clothing[6] = new int[] { 6, 0 }; // SHOES
                this.Clothing[7] = new int[] { 0, 0 }; // SPECIAL1
                this.Clothing[8] = new int[] { 57, 0 }; // SPECIAL2
                this.Clothing[9] = new int[] { 0, 0 }; // SPECIAL3
                this.Clothing[10] = new int[] { 0, 0 }; // TEXTURES
                this.Clothing[11] = new int[] { 146, 0 }; // TORSO2
            }
            else
            {
                this.Model = "mp_f_freemode_01";
                this.Clothing[0] = new int[] { 0, 0 }; // FACE
                this.Clothing[1] = new int[] { 0, 0 }; // HEAD
                this.Clothing[2] = new int[] { 0, 0 }; // HAIR
                this.Clothing[3] = new int[] { 4, 0 }; // TORSO
                this.Clothing[4] = new int[] { 66, 6 }; // LEGS
                this.Clothing[5] = new int[] { 0, 0 }; // HANDS
                this.Clothing[6] = new int[] { 5, 0 }; // SHOES
                this.Clothing[7] = new int[] { 0, 0 }; //SPECIAL1
                this.Clothing[8] = new int[] { 2, 0 }; // SPECIAL2
                this.Clothing[9] = new int[] { 0, 0 }; // SPECIAL3
                this.Clothing[10] = new int[] { 0, 0 }; // TEXTURES
                this.Clothing[11] = new int[] { 118, 0 }; // TORSO2
            }
        }

        // Sets character as no longer new
        public void SetNoLongerNew()
        {
            this.isNew = false;
        }

        // Sets characters current death status
        public void SetDeadStatus(bool _isDead)
        {
            this.isDead = _isDead;
        }

        // Sets characters last position
        public void SetLastPosition(float[] args)
        {
            this.LastPos[0] = args[0];
            this.LastPos[1] = args[1];
            this.LastPos[2] = args[2];
        }

        // Sets characters parent properties
        public void SetParents(int _father, int _mother, float _mix)
        {
            this.Parents.SetFather(_father);
            this.Parents.SetMother(_mother);
            this.Parents.SetMix(_mix);
        }

        // Sets characters facial properties
        public void SetFaceFeature(int _key, float _featureMix)
        {
            this.FaceFeatures[_key] = _featureMix;
        }

        // Sets characters clothing properties
        public void SetClothing(int _key, int[] args)
        {
            this.Clothing[_key] = new int[] { args[0], args[1] };
        }

        // Sets characters prop properties
        public void SetProps(int _key, int[] args)
        {
            this.Props[_key] = new int[] { args[0], args[1] };
        }

        // Adds XP to a defined skill
        public Tuple<bool, string> AddExperience(string _skill, int _amount)
        {
            if (this.Skills[_skill] == null) { return Tuple.Create<bool, string>(false, null); }
            return this.Skills[_skill].AddExperience(_amount);
        }

        // Add Hunger
        public void AddHunger(int _amount)
        {
            int NewHunger = this.Hunger + _amount;
            if (NewHunger > 100) { NewHunger = 100; }
            this.Hunger = NewHunger;
        }

        // Remove Hunger
        public void RemoveHunger(int _amount)
        {
            int NewHunger = this.Hunger - _amount;
            if (NewHunger < 0) { NewHunger = 0; }
            this.Hunger = NewHunger;
        }

        // Add Thirst
        public void AddThirst(int _amount)
        {
            int NewThirst = this.Thirst + _amount;
            if (NewThirst > 100) { NewThirst = 100; }
            this.Thirst = NewThirst;
        }

        // Remove Thirst
        public void RemoveThirst(int _amount)
        {
            int NewThirst = this.Thirst - _amount;
            if (NewThirst < 0) { NewThirst = 0; }
            this.Thirst = NewThirst;
        }

        // Add Sickness
        public void AddSickness(int _amount)
        {
            int NewSickness = this.Sickness + _amount;
            if (NewSickness > 100) { NewSickness = 100; }
            this.Sickness = NewSickness;
        }

        // Remove Sickness
        public void RemoveSickness(int _amount)
        {
            int NewSickness = this.Sickness - _amount;
            if (NewSickness < 0) { NewSickness = 0; }
            this.Sickness = NewSickness;
        }
    }
}
