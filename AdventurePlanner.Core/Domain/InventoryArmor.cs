using System;
using System.Linq;

namespace AdventurePlanner.Core.Domain
{
    public class InventoryArmor
    {
        private PlayerCharacter _playerCharacter;

        public Armor Armor { get; private set; }

        public InventoryArmor(PlayerCharacter playerCharacter, Armor armor)
        {
            _playerCharacter = playerCharacter;
            Armor = armor;
        }

        public bool IsProficient
        {
            get { return _playerCharacter.ArmorProficiencies.Contains(Armor.ProficiencyGroup, StringComparer.InvariantCultureIgnoreCase); }
        }

        public int ArmorClass
        {
            get
            {
                var dexMod = _playerCharacter.Abilities["Dex"].Modifier;

                if (Armor.MaximumDexterityModifier.HasValue)
                {
                    dexMod = Math.Min(dexMod, Armor.MaximumDexterityModifier.Value);
                }

                return Armor.ArmorClass + dexMod;
            }
        }
    }
}
