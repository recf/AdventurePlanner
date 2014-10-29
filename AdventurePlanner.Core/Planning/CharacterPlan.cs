using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AdventurePlanner.Core.Snapshots;
using Newtonsoft.Json;

namespace AdventurePlanner.Core.Planning
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CharacterPlan
    {
        [JsonProperty("snapshot_level", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(20)]
        public int SnapshotLevel { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("race", Required = Required.Always)]
        public string Race { get; set; }

        [JsonProperty("speed", Required = Required.Always)]
        public int Speed { get; set; }

        [JsonProperty("alignment", Required = Required.Always)]
        public string Alignment { get; set; }

        #region Appearance

        [JsonProperty("age", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Age { get; set; }

        [JsonProperty("height_feet", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int HeightFeet { get; set; }

        [JsonProperty("height_inches", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int HeightInches { get; set; }

        [JsonProperty("weight", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Weight { get; set; }

        [JsonProperty("eyes", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string EyeColor { get; set; }

        [JsonProperty("hair", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string HairColor { get; set; }

        [JsonProperty("skin", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string SkinColor { get; set; }

        #endregion

        #region Background info

        [JsonProperty("background", Required = Required.Always)]
        public string Background { get; set; }

        #endregion

        [JsonProperty("level_plans", Required = Required.Always)]
        public IList<LevelPlan> LevelPlans { get; set; }

        // TODO: :question: Consider moving ToSnapshot into an extension method.
        public CharacterSnapshot ToSnapshot(int level)
        {
            var applicable = LevelPlans.Where(l => l.Level <= level).ToList();

            var snapshot = new CharacterSnapshot
            {
                Name = Name,
                Race = Race,
                Speed = Speed,
                Alignment = Alignment,
                Background = Background,
                Age = Age,
                HeightFeet = HeightFeet,
                HeightInches = HeightInches,
                Weight = Weight,
                EyeColor = EyeColor,
                HairColor = HairColor,
                SkinColor = SkinColor,

                Classes = (from l in applicable
                          group l by l.ClassName ?? "<not set>" into c
                          select new { c.Key, Value = c.Count() }).ToDictionary(l => l.Key, l => l.Value)
            };

            foreach (var plan in applicable)
            {
                foreach (var kvp in plan.AbilityScoreImprovements ?? new Dictionary<string, int>())
                {
                    snapshot.Abilities[kvp.Key].Score += kvp.Value;
                }

                if (plan.SetProficiencyBonus > 0)
                {
                    snapshot.ProficiencyBonus = plan.SetProficiencyBonus;
                }

                foreach (var skillName in plan.NewSkillProficiencies ?? new string[0])
                {
                    snapshot.Skills[skillName].IsProficient = true;
                }

                foreach (var savingThrowKey in plan.NewSaveProficiencies ?? new string[0])
                {
                    snapshot.SavingThrows[savingThrowKey].IsProficient = true;
                }

                foreach (var feature in plan.FeaturePlans ?? new FeaturePlan[0])
                {
                    snapshot.Features.Add(new FeatureSnapshot()
                    {
                        Name = feature.Name,
                        Description = feature.Description
                    });
                }
            }

            return snapshot;
        }
    }
}
