using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using AdventurePlanner.Core.Meta;

namespace AdventurePlanner.Core.Snapshots
{
    public class CharacterSnapshot
    {
        public string Name { get; set; }

        public string Race { get; set; }

        public int Speed { get; set; }

        public string Alignment { get; set; }

        public string Background { get; set; }

        #region Appearance

        public int Age { get; set; }

        public int? HeightFeet { get; set; }

        public int HeightInches { get; set; }

        public int Weight { get; set; }

        public string EyeColor { get; set; }

        public string HairColor { get; set; }

        public string SkinColor { get; set; }

        #endregion

        public int CharacterLevel
        {
            get { return Classes.Sum(kvp => kvp.Value); }
        }

        public IDictionary<string, int> Classes { get; set; }

        public IReadOnlyDictionary<string, AbilitySnapshot> Abilities { get; private set; }

        public IReadOnlyDictionary<string, SavingThrowSnapshot> SavingThrows { get; private set; }

        public int ProficiencyBonus { get; set; }

        public IReadOnlyDictionary<string, SkillSnapshot> Skills { get; private set; }

        public IList<FeatureSnapshot> Features { get; private set; }

        public ISet<string> ArmorProficiencies { get; private set; }

        public IList<ArmorSnapshot> Armor { get; private set; } 

        public ISet<string> WeaponProficiencies { get; private set; }

        public ISet<string> ToolProficiencies { get; private set; }

        public CharacterSnapshot()
        {
            var abilities =
                Ability.All.Select(conf => new AbilitySnapshot(conf.Abbreviation, conf.AbilityName))
                    .ToDictionary(a => a.Abbreviation);

            Abilities = new ReadOnlyDictionary<string, AbilitySnapshot>(abilities);

            var skills =
                Skill.All.Select(conf => new SkillSnapshot(this, conf.SkillName, Abilities[conf.Ability]))
                    .ToDictionary(s => s.SkillName);

            Skills = new ReadOnlyDictionary<string, SkillSnapshot>(skills);

            var savingThrows =
                Abilities.Values.Select(a => new SavingThrowSnapshot(this, a))
                    .ToDictionary(s => s.Ability.Abbreviation);

            SavingThrows = new ReadOnlyDictionary<string, SavingThrowSnapshot>(savingThrows);

            Features = new List<FeatureSnapshot>();

            ArmorProficiencies = new SortedSet<string>();
            WeaponProficiencies = new SortedSet<string>();
            ToolProficiencies = new SortedSet<string>();

            Armor = new List<ArmorSnapshot>();
        }
    }
}
