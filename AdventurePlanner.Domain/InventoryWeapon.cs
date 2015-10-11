using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polyhedral;

namespace AdventurePlanner.Domain
{
    public class InventoryWeapon
    {
        private PlayerCharacter _playerCharacter;

        public Weapon Weapon { get; private set; }

        public InventoryWeapon(PlayerCharacter playerCharacter, Weapon weapon)
        {
            _playerCharacter = playerCharacter;
            Weapon = weapon;
        }

        public IList<Attack> GetAttacks()
        {
            var attacks = new List<Attack>();

            var isRanged = Weapon.NormalRange.HasValue;

            var abilityKey = isRanged ? "Dex" : "Str";
            var ability = _playerCharacter.Abilities[abilityKey];

            var attackType = isRanged ? "Ranged" : "Melee";
            var attackName = attackType + " Attack";

            attacks.Add(new Attack(_playerCharacter)
            {
                Name = attackName,
                AttackModifier = ability.Modifier + _playerCharacter.ProficiencyBonus,
                DamageDice = (Weapon.DamageDice ?? new DiceRoll()) + ability.Modifier,
                DamageType = Weapon.DamageType,
                NormalRange = Weapon.NormalRange,
                MaximumRange = Weapon.MaximumRange,
            });

            if (Weapon.IsLight)
            {
                attacks.Add(new Attack(_playerCharacter)
                {
                    Name = attackName + " (Bonus Action)",
                    AttackModifier = ability.Modifier + _playerCharacter.ProficiencyBonus,
                    DamageDice = (Weapon.DamageDice ?? new DiceRoll()),
                    DamageType = Weapon.DamageType,
                    NormalRange = Weapon.NormalRange,
                    MaximumRange = Weapon.MaximumRange,
                });
            }

            return attacks;
        }
    }
}
