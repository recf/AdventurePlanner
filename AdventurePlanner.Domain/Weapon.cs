using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Polyhedral;

namespace AdventurePlanner.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Weapon
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("proficiency_group", Required = Required.Always)]
        public string ProficiencyGroup { get; set; }
        
        [JsonProperty("has_ammo", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(false)]
        public bool HasAmmunition { get; set; }

        [JsonProperty("damage_dice", Required = Required.Always)]
        public DiceRoll DamageDice { get; set; }

        [JsonProperty("damage_type", Required = Required.Always)]
        public string DamageType { get; set; }

        [JsonProperty("range", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? NormalRange { get; set; }

        [JsonProperty("max_range", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? MaximumRange { get; set; }

        [JsonProperty("light")]
        public bool IsLight { get; set; }
    }
}
