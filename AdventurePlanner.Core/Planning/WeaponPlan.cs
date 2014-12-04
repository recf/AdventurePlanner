using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventurePlanner.Core.Conversion;
using AdventurePlanner.Core.Snapshots;
using Newtonsoft.Json;
using Polyhedral;

namespace AdventurePlanner.Core.Planning
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WeaponPlan
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("proficiency_group", Required = Required.Always)]
        public string ProficiencyGroup { get; set; }
        
        [JsonProperty("has_ammo", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(false)]
        public bool HasAmmunition { get; set; }

        [JsonProperty("damage_dice", Required = Required.Always)]
        [JsonConverter(typeof(DiceRollConverter))]
        public DiceRoll DamageDice { get; set; }

        [JsonProperty("damage_type", Required = Required.Always)]
        public string DamageType { get; set; }

        [JsonProperty("range", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? NormalRange { get; set; }

        [JsonProperty("max_range", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? MaximumRange { get; set; }

        public IList<AttackSnapshot> GetAttacks(CharacterSnapshot snapshot)
        {
            var attacks = new List<AttackSnapshot>();

            var isRanged = NormalRange.HasValue;

            var abilityKey = isRanged ? "Dex" : "Str";
            var ability = snapshot.Abilities[abilityKey];

            var mainAttack = new AttackSnapshot(snapshot)
            {
                Name = Name,
                Ability = ability,
                DamageDice = (DamageDice ?? new DiceRoll()) + ability.Modifier,
                DamageType = DamageType,
                NormalRange = NormalRange,
                MaximumRange = MaximumRange,
            };

            attacks.Add(mainAttack);

            return attacks;
        }
    }
}
