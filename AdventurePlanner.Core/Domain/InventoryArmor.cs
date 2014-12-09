using System;
using System.Linq;

namespace AdventurePlanner.Core.Domain
{
    public class InventoryArmor
    {
        private CharacterSnapshot _character;

        public Armor Armor { get; private set; }

        public InventoryArmor(CharacterSnapshot character, Armor armor)
        {
            _character = character;
            Armor = armor;
        }

        public bool IsProficient
        {
            get { return _character.ArmorProficiencies.Contains(Armor.ProficiencyGroup, StringComparer.InvariantCultureIgnoreCase); }
        }

        public int ArmorClass
        {
            get
            {
                var dexMod = _character.Abilities["Dex"].Modifier;

                if (Armor.MaximumDexterityModifier.HasValue)
                {
                    dexMod = Math.Min(dexMod, Armor.MaximumDexterityModifier.Value);
                }

                return Armor.ArmorClass + dexMod;
            }
        }
    }
}
