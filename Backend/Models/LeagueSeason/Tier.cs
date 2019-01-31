using System.Collections.Generic;

namespace Backend.Models.LeagueSeason
{
    public class Tier
    {
        public List<Division> Divisions { get; set; }
        public int Level { get; set; }
    }

    public class Division
    {
        public string Name { get; set; }
        public int ActiveFrom { get; set; }
        public int ActiveTo { get; set; }
    }
}