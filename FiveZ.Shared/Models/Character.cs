using System.Collections.Generic;
using FiveZ.Shared.Enums;

namespace FiveZ.Shared.Models
{
    public class Character
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Genders Gender { get; set; }
        public bool isNew { get; set; }
        public float[] LastPos { get; set; }
        // Add Character Looks
        // Add Clothing
        // Add Props
        //public Skills Skills { get; set; }
        //public List<Item> Inventory { get; set; }
    }
}
