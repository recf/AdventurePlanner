using Polyhedral;

namespace AdventurePlanner.Core.Domain
{
    public class AttackSnapshot
    {
        private CharacterSnapshot _character;

        public AttackSnapshot(CharacterSnapshot character)
        {
            _character = character;
        }

        public string Name { get; set; }

        public DiceRoll DamageDice { get; set; }

        public string DamageType { get; set; }

        public int? NormalRange { get; set; }

        public int? MaximumRange { get; set; }

        public int AttackModifier { get; set; }
    }
}
