using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polyhedral;

namespace AdventurePlanner.Core.Snapshots
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
