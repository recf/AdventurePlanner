namespace AdventurePlanner.Core.Domain
{
    public class SavingThrowSnapshot
    {
        private readonly CharacterSnapshot _character;

        public SavingThrowSnapshot(CharacterSnapshot character, AbilitySnapshot ability)
        {
            _character = character;

            Ability = ability;
        }

        public AbilitySnapshot Ability { get; private set; }
        
        public bool IsProficient { get; set; }

        public int Modifier
        {
            get { return (IsProficient ? _character.ProficiencyBonus : 0) + Ability.Modifier; }
        }
    }
}