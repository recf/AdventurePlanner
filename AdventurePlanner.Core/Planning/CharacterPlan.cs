using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using AdventurePlanner.Core.Snapshots;
using Newtonsoft.Json;

namespace AdventurePlanner.Core.Planning
{
    [JsonObject(MemberSerialization.OptIn, Title = "Character Plan", Description = "Adventure Planner: Levelling plan for a D&D 5e character.")]
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

        [JsonProperty("class_plans", Required = Required.Always)]
        public IList<ClassPlan> ClassPlans { get; set; }

        [JsonProperty("level_plans", Required = Required.Always)]
        public IList<LevelPlan> LevelPlans { get; set; }

        // TODO: :question: Consider moving ToSnapshot into an extension method.
        public CharacterSnapshot ToSnapshot(int level)
        {
            var applicableLevels = LevelPlans.Where(l => l.Level <= level).ToList();

            var applicableClasses = (from l in applicableLevels
                                     where l.ClassPlan != null
                                     group l by l.ClassPlan
                                         into c
                                         select new { c.Key, Value = c.Count() }).ToList();

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

                // TODO: :poop: This is really clunky
                Classes = applicableClasses.ToDictionary(l => l.Key != null ? l.Key.ClassName ?? "<not set>" : "<not set>", l => l.Value)
            };

            foreach (var plan in applicableClasses.Select(ac => ac.Key))
            {
                foreach (var prof in plan.ArmorProficiencies ?? new string[0])
                {
                    snapshot.ArmorProficiencies.Add(prof);
                }

                foreach (var prof in plan.WeaponProficiencies ?? new string[0])
                {
                    snapshot.WeaponProficiencies.Add(prof);
                }

                foreach (var prof in plan.ToolProficiencies ?? new string[0])
                {
                    snapshot.ToolProficiencies.Add(prof);
                }

                foreach (var savingThrowKey in plan.SaveProficiencies ?? new string[0])
                {
                    snapshot.SavingThrows[savingThrowKey].IsProficient = true;
                }

                foreach (var skillName in plan.SkillProficiencies ?? new string[0])
                {
                    snapshot.Skills[skillName].IsProficient = true;
                }
            }

            foreach (var plan in applicableLevels)
            {
                foreach (var kvp in plan.AbilityScoreImprovements ?? new Dictionary<string, int>())
                {
                    snapshot.Abilities[kvp.Key].Score += kvp.Value;
                }

                if (plan.SetProficiencyBonus > 0)
                {
                    snapshot.ProficiencyBonus = plan.SetProficiencyBonus;
                }

                foreach (var feature in plan.FeaturePlans ?? new FeaturePlan[0])
                {
                    var target = snapshot.Features;
                    if (!string.IsNullOrWhiteSpace(feature.SkillName))
                    {
                        target = snapshot.Skills[feature.SkillName].Features;
                    }

                    target.Add(new FeatureSnapshot()
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
