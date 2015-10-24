using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AdventurePlanner.Domain
{
    public class Ability
    {
        public Ability(string name, string abbreviation)
        {
            Name = name;
            Abbreviation = abbreviation;
        }

        public string Name { get; private set;}

        public string Abbreviation { get; private set; }
    }
}
