using System.Collections.Generic;

namespace AdventurePlanner.Core.Domain
{
    public class SkillScore
    {
        private readonly PlayerCharacter _playerCharacter;

        public SkillScore(PlayerCharacter playerCharacter, string skillName, AbilityScore ability)
        {
            _playerCharacter = playerCharacter;

            // TODO: :question: Make this an actual Skill object?
            SkillName = skillName;
            Ability = ability;
        }

        public string SkillName { get; private set; }

        public AbilityScore Ability { get; private set; }

        public bool IsProficient { get; set; }

        public int Modifier
        {
            get { return (IsProficient ? _playerCharacter.ProficiencyBonus : 0) + Ability.Modifier; }
        }
    }
}
