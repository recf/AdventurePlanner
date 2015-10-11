using Newtonsoft.Json;

namespace AdventurePlanner.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Armor
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("proficiency_group", Required = Required.Always)]
        public string ProficiencyGroup { get; set; }

        [JsonProperty("ac", Required = Required.Always)]
        public int ArmorClass { get; set; }

        [JsonProperty("max_dex", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? MaximumDexterityModifier { get; set; }
    }
}
