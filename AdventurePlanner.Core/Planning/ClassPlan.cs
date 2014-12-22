using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AdventurePlanner.Core.Planning
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ClassPlan
    {
        [JsonProperty("class_name", Required = Required.Always)]
        public string ClassName { get; set; }
        
        [JsonProperty("armor_proficiencies", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string[] ArmorProficiencies { get; set; }

        [JsonProperty("weapon_proficiencies", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string[] WeaponProficiencies { get; set; }

        [JsonProperty("tool_proficiencies", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string[] ToolProficiencies { get; set; }

        [JsonProperty("save_proficiencies", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string[] SaveProficiencies { get; set; }

        [JsonProperty("skill_proficiencies", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string[] SkillProficiencies { get; set; }
    }
}
