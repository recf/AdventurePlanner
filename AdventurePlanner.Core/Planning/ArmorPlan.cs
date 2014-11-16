using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AdventurePlanner.Core.Planning
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ArmorPlan
    {
        [JsonProperty("armor_name", Required = Required.Always)]
        public string ArmorName { get; set; }

        [JsonProperty("proficiency_group", Required = Required.Always)]
        public string ProficiencyGroup { get; set; }

        [JsonProperty("ac", Required = Required.Always)]
        public int ArmorClass { get; set; }

        [JsonProperty("max_dex", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? MaximumDexterityModifier { get; set; }
    }
}
