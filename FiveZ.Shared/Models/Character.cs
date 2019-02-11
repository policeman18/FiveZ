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
        public string Name { get; set; }
        public LootType Type { get; set; }
        public string ActionString { get; set; }
        public object[] ActionArguments { get; set; }

        // Base Constructor
        public CharacterInventoryItem() { }

        // Food, Drink, Health
        public CharacterInventoryItem(string _name, LootType _type, string _action, object[] args)
        {
            this.Name = _name;
            this.Type = _type;
            this.ActionString = _action;
            this.ActionArguments = args;
        }

        // PrimaryWeapon
        // MeleeWeapon
        // WeaponAttachment
        // RifleAmmo
        // ShotgunAmmo
        // PistolAmmo
        // Placements
        // Regular
    }

    /// <summary>
    ///     Character Inventory Data Class
    /// </summary>
    public class CharacterInventory
    {
        [JsonProperty]
        public int InventorySpace { get; protected set; }
        [JsonProperty]
        public CharacterInventoryItem WeaponOne { get; protected set; }
        [JsonProperty]
        public CharacterInventoryItem WeaponTwo { get; protected set; }
        [JsonProperty]
        public CharacterInventoryItem WeaponThree { get; protected set; }
        [JsonProperty]
        public List<CharacterInventoryItem> Inventory { get; protected set; } = new List<CharacterInventoryItem>();

        // Constructors
        public CharacterInventory() { }
        public CharacterInventory(int _space)
        {
            this.InventorySpace = _space;
        }

        public void SetWeaponOne(CharacterInventoryItem _item)
        {
            this.WeaponOne = _item;
        }
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
            this.Inventory.Add(_item);
        }
        public void DeleteItem(CharacterInventoryItem _item)
        {
            this.Inventory.Remove(_item);
        }
    }
    
    /// <summary>
    ///     Character Data Class
    /// </summary>
    public class Character
    {
        [JsonProperty]
        public int Id { get; protected set; }
        [JsonProperty]
        public int UserId { get; protected set; }
        [JsonProperty]
        public string FirstName { get; protected set; }
        [JsonProperty]
        public string LastName { get; protected set; }
        [JsonProperty]
        public Genders Gender { get; protected set; }
        [JsonProperty]
        public bool isNew { get; protected set; } = true;
        [JsonProperty]
        public bool isDead { get; protected set; } = true;
        [JsonProperty]
        public float[] LastPos { get; protected set; } = new float[] { 0f, 0f, 0f };
        [JsonProperty]
        public string Model { get; protected set; }
        [JsonProperty]
        public CharacterParents Parents { get; protected set; } = new CharacterParents();
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
        [JsonProperty]
        public CharacterAppearance Appearance { get; protected set; } = new CharacterAppearance();
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
        [JsonProperty]
        public Dictionary<string, CharacterSkill> Skills { get; protected set; } = new Dictionary<string, CharacterSkill>()
        {
            ["stamina"] = new CharacterSkill("Stamina", 10),
            ["crafting"] = new CharacterSkill("Crafting", 25),
        };
        [JsonProperty]
        public CharacterInventory Inventory { get; protected set; } = new CharacterInventory();

        public Character() {  }
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

        public void SetNoLongerNew()
        {
            this.isNew = false;
        }

        public void SetDeadStatus(bool _isDead)
        {
            this.isDead = _isDead;
        }

        public void SetLastPosition(float[] args)
        {
            this.LastPos[0] = args[0];
            this.LastPos[1] = args[1];
            this.LastPos[2] = args[2];
        }

        public void SetParents(int _father, int _mother, float _mix)
        {
            this.Parents.SetFather(_father);
            this.Parents.SetMother(_mother);
            this.Parents.SetMix(_mix);
        }

        public void SetFaceFeature(int _key, float _featureMix)
        {
            this.FaceFeatures[_key] = _featureMix;
        }

        public void SetClothing(int _key, int[] args)
        {
            this.Clothing[_key] = new int[] { args[0], args[1] };
        }

        public void SetProps(int _key, int[] args)
        {
            this.Props[_key] = new int[] { args[0], args[1] };
        }

        public Tuple<bool, string> AddExperience(string _skill, int _amount)
        {
            if (this.Skills[_skill] == null) { return Tuple.Create<bool, string>(false, null); }
            return this.Skills[_skill].AddExperience(_amount);
        }
    }
}
