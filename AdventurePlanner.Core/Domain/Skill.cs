using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AdventurePlanner.Core.Domain
{
    public class Skill
    {
        public static IReadOnlyCollection<Skill> All { get; private set; }

        public string SkillName { get; set; }

        public string Ability { get; set; }

        static Skill()
        {
            var skills = new List<Skill>()
            {
                new Skill() { SkillName = "Acrobatics", Ability = "Dex" },
                new Skill() { SkillName = "Animal Handling", Ability = "Wis" },
                new Skill() { SkillName = "Arcana", Ability = "Int" },
                new Skill() { SkillName = "Athletics", Ability = "Str" },
                new Skill() { SkillName = "Deception", Ability = "Cha" },
                new Skill() { SkillName = "History", Ability = "Int" },
                new Skill() { SkillName = "Insight", Ability = "Wis" },
                new Skill() { SkillName = "Intimidation", Ability = "Cha" },
                new Skill() { SkillName = "Investigation", Ability = "Int" },
                new Skill() { SkillName = "Medicine", Ability = "Wis" },
                new Skill() { SkillName = "Nature", Ability = "Int" },
                new Skill() { SkillName = "Perception", Ability = "Wis" },
                new Skill() { SkillName = "Performance", Ability = "Dex" },
                new Skill() { SkillName = "Persuasion", Ability = "Cha" },
                new Skill() { SkillName = "Religion", Ability = "Int" },
                new Skill() { SkillName = "Sleight of Hand", Ability = "Dex" },
                new Skill() { SkillName = "Stealth", Ability = "Dex" },
                new Skill() { SkillName = "Survival", Ability = "Wis" },
            };

            All = new ReadOnlyCollection<Skill>(skills);
        }
    }
}
