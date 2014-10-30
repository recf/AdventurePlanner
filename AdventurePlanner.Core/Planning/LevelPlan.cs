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

        [JsonProperty("ability_score_improvements", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IDictionary<string, int> AbilityScoreImprovements { get; set; }

        [JsonProperty("set_prof_bonus", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int SetProficiencyBonus { get; set; }
        
        [JsonProperty("new_save_proficiencies", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string[] NewSaveProficiencies { get; set; }

        [JsonProperty("new_skill_proficiencies", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string[] NewSkillProficiencies { get; set; }
        
        [JsonProperty("features", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<FeaturePlan> FeaturePlans { get; set; }
    }
}
