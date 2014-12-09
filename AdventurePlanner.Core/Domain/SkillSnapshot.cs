using System.Collections.Generic;

namespace AdventurePlanner.Core.Domain
{
    public class SkillSnapshot
    {
        private readonly CharacterSnapshot _character;

        public SkillSnapshot(CharacterSnapshot character, string skillName, AbilitySnapshot ability)
        {
            _character = character;

            SkillName = skillName;
            Ability = ability;

            Features = new List<FeatureSnapshot>();
        }

        public string SkillName { get; private set; }

        public AbilitySnapshot Ability { get; private set; }

        public IList<FeatureSnapshot> Features { get; private set; }

        public bool IsProficient { get; set; }

        public int Modifier
        {
            get { return (IsProficient ? _character.ProficiencyBonus : 0) + Ability.Modifier; }
        }
    }
}
