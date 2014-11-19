using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventurePlanner.Core.Snapshots
{
    public class ArmorSnapshot
    {
        private CharacterSnapshot _character;

        public ArmorSnapshot(CharacterSnapshot character)
        {
            _character = character;
        }

        public string ArmorName { get; set; }

        public string ProficiencyGroup { get; set; }

        public int BaseArmorClass { get; set; }

        public int? MaximumDexterityModifier { get; set; }

        public bool IsProficient
        {
            get { return _character.ArmorProficiencies.Contains(ProficiencyGroup, StringComparer.InvariantCultureIgnoreCase); }
        }

        public int TotalArmorClass
        {
            get
            {
                var dexMod = _character.Abilities["Dex"].Modifier;

                if (MaximumDexterityModifier.HasValue)
                {
                    dexMod = Math.Min(dexMod, MaximumDexterityModifier.Value);
                }

                return BaseArmorClass + dexMod;
            }
        }
    }
}
