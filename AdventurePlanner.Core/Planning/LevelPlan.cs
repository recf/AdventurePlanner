using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdventurePlanner.Core.Planning
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LevelPlan
    {
        [JsonProperty("level", Required = Required.Always)]
        public int Level { get; set; }

        [JsonProperty("class", Required = Required.Always)]
        public string ClassName { get; set; }

        [JsonProperty("ability_score_increases", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IDictionary<string, int> AbilityScoreIncreases { get; set; }

        [JsonProperty("set_prof_bonus", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int SetProficiencyBonus { get; set; }

        [JsonProperty("new_skill_proficiencies", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string[] NewSkillProficiencies { get; set; }
    }
}
