using System.Collections.Generic;
using FiveZ.Shared.Enums;

namespace FiveZ.Shared.Models
{
    public class CharacterParents
    {
        public int Father { get; set; } = 0;
        public int Mother { get; set; } = 0;
        public float Mix { get; set; } = 0f;
    }

    public class CharacterAppearance
    {
        public int HairStyle { get; set; } = 0;
        public int HairColor { get; set; } = 0;
        public int HairHighlightColor { get; set; } = 0;
    }

    public class Character
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Genders Gender { get; set; }
        public bool isNew { get; set; } = true;
        public float[] LastPos { get; set; } = new float[] { 0f, 0f, 0f };
        public string Model { get; set; }
        public CharacterParents Parents { get; set; }
        public Dictionary<int, float> FaceFeatures { get; set; } = new Dictionary<int, float>()
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
        public Dictionary<int, int[]> Clothing { get; set; } = new Dictionary<int, int[]>()
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
        public Dictionary<int, int[]> Props { get; set; } = new Dictionary<int, int[]>()
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
        public Dictionary<string, int[]> Skills { get; set; } = new Dictionary<string, int[]>()
        {
            ["stamina"] = new int[] { 1, 0 },
            ["crafting"] = new int[] { 1, 0 }
        };

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
    }
}
