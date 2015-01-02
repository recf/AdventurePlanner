using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AdventurePlanner.Core.Planning;

namespace AdventurePlanner.Core.Domain
{
    public class PlayerCharacter
    {
        public string Name { get; set; }

        public string Race { get; set; }

        public int Speed { get; set; }

        public string Alignment { get; set; }

        public string Background { get; set; }

        #region Appearance

        public int Age { get; set; }

        public int HeightFeet { get; set; }

        public int HeightInches { get; set; }

        public int Weight { get; set; }

        public string EyeColor { get; set; }

        public string HairColor { get; set; }

        public string SkinColor { get; set; }

        #endregion

        public int CharacterLevel { get; set; }

        public string ClassName { get; set; }

        public IReadOnlyDictionary<string, AbilityScore> Abilities { get; private set; }

        public IReadOnlyDictionary<string, SavingThrow> SavingThrows { get; private set; }

        public int ProficiencyBonus
        {
            get { return ((CharacterLevel - 1)/4) + 2; }
        }

        public IReadOnlyDictionary<string, SkillScore> Skills { get; private set; }

        public IList<FeatureSnapshot> Features { get; private set; }

        public ISet<string> ArmorProficiencies { get; private set; }

        public IList<InventoryArmor> Armor { get; private set; }
        
        public ISet<string> WeaponProficiencies { get; private set; }

        public IList<InventoryWeapon> Weapons { get; private set; }

        public ISet<string> ToolProficiencies { get; private set; }

        public PlayerCharacter()
        {
            var abilities =
                Ability.All.Select(conf => new AbilityScore(conf.Abbreviation, conf.Name))
                    .ToDictionary(a => a.Abbreviation);

            Abilities = new ReadOnlyDictionary<string, AbilityScore>(abilities);

            var skills =
                Skill.All.Select(conf => new SkillScore(this, conf.SkillName, Abilities[conf.Ability]))
                    .ToDictionary(s => s.SkillName);

            Skills = new ReadOnlyDictionary<string, SkillScore>(skills);

            var savingThrows =
                Abilities.Values.Select(a => new SavingThrow(this, a))
                    .ToDictionary(s => s.Ability.Abbreviation);

            SavingThrows = new ReadOnlyDictionary<string, SavingThrow>(savingThrows);

            Features = new List<FeatureSnapshot>();

            ArmorProficiencies = new SortedSet<string>();
            WeaponProficiencies = new SortedSet<string>();
            ToolProficiencies = new SortedSet<string>();

            Armor = new List<InventoryArmor>();
            Weapons = new List<InventoryWeapon>();
        }
    }
}
