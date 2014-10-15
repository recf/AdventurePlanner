using Newtonsoft.Json;

namespace AdventurePlanner.Core.Planning
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CharacterLevelPlan
    {
        [JsonProperty("level", Required = Required.Always)]
        public int Level { get; set; }

        [JsonProperty("class", Required = Required.Always)]
        public string ClassName { get; set; }

        // TODO: Convert Ability Score increases to to a dictionary (to map to snapshot)
        #region Ability Scores

        [JsonProperty("increase_str", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int IncreaseStr { get; set; }

        [JsonProperty("increase_dex", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int IncreaseDex { get; set; }

        [JsonProperty("increase_con", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int IncreaseCon { get; set; }

        [JsonProperty("increase_int", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int IncreaseInt { get; set; }

        [JsonProperty("increase_wis", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int IncreaseWis { get; set; }

        [JsonProperty("increase_cha", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int IncreaseCha { get; set; }

        #endregion

        [JsonProperty("set_prof_bonus", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int SetProficiencyBonus { get; set; }

        [JsonProperty("new_skill_proficiencies", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string[] NewSkillProficiencies { get; set; }
    }
}
