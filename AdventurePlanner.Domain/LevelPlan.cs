using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdventurePlanner.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LevelPlan
    {
        [JsonProperty("level", Required = Required.Always)]
        public int Level { get; set; }

        [JsonProperty("ability_score_improvements", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IDictionary<string, int> AbilityScoreImprovements { get; set; }
        
        [JsonProperty("features", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<FeaturePlan> FeaturePlans { get; set; }
    }
}
