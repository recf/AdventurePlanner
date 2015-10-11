namespace AdventurePlanner.Domain
{
    public class SavingThrow
    {
        private readonly PlayerCharacter _playerCharacter;

        public SavingThrow(PlayerCharacter playerCharacter, AbilityScore ability)
        {
            _playerCharacter = playerCharacter;

            Ability = ability;
        }

        public AbilityScore Ability { get; private set; }
        
        public bool IsProficient { get; set; }

        public int Modifier
        {
            get { return (IsProficient ? _playerCharacter.ProficiencyBonus : 0) + Ability.Modifier; }
        }
    }
}