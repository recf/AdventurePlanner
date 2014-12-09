using Polyhedral;

namespace AdventurePlanner.Core.Domain
{
    public class Attack
    {
        private PlayerCharacter _playerCharacter;

        public Attack(PlayerCharacter playerCharacter)
        {
            _playerCharacter = playerCharacter;
        }

        public string Name { get; set; }

        public DiceRoll DamageDice { get; set; }

        public string DamageType { get; set; }

        public int? NormalRange { get; set; }

        public int? MaximumRange { get; set; }

        public int AttackModifier { get; set; }
    }
}
