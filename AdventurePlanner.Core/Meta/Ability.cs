using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventurePlanner.Core.Meta
{
    public class Ability
    {
        public static IReadOnlyCollection<Ability> All { get; private set; }

        public string AbilityName { get; set; }

        public string Abbreviation { get; set; }

        static Ability()
        {
            var abilities = new List<Ability>
            {
                new Ability { Abbreviation = "Str", AbilityName = "Strength" },
                new Ability { Abbreviation = "Dex", AbilityName = "Dexterity" },
                new Ability { Abbreviation = "Con", AbilityName = "Constitution" },
                new Ability { Abbreviation = "Int", AbilityName = "Intelligence" },
                new Ability { Abbreviation = "Wis", AbilityName = "Wisdom" },
                new Ability { Abbreviation = "Cha", AbilityName = "Charisma" },
            };

            All = new ReadOnlyCollection<Ability>(abilities);
        }
    }
}
