using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AdventurePlanner.Core.Domain
{
    public class Ability
    {
        public static IReadOnlyCollection<Ability> All { get; private set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        static Ability()
        {
            var abilities = new List<Ability>
            {
                new Ability { Abbreviation = "Str", Name = "Strength" },
                new Ability { Abbreviation = "Dex", Name = "Dexterity" },
                new Ability { Abbreviation = "Con", Name = "Constitution" },
                new Ability { Abbreviation = "Int", Name = "Intelligence" },
                new Ability { Abbreviation = "Wis", Name = "Wisdom" },
                new Ability { Abbreviation = "Cha", Name = "Charisma" },
            };

            All = new ReadOnlyCollection<Ability>(abilities);
        }
    }
}
