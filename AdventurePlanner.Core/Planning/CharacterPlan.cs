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
        public IList<CharacterLevelPlan> LevelPlans { get; set; }

        public CharacterSnapshot ToSnapshot(int level)
        {
            var applicable = LevelPlans.Where(l => l.Level <= level);

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
                snapshot.StrScore.Score += plan.IncreaseStr;
                snapshot.DexScore.Score += plan.IncreaseDex;
                snapshot.ConScore.Score += plan.IncreaseCon;
                snapshot.IntScore.Score += plan.IncreaseInt;
                snapshot.WisScore.Score += plan.IncreaseWis;
                snapshot.ChaScore.Score += plan.IncreaseCha;

                if (plan.SetProficiencyBonus > 0)
                {
                    snapshot.ProficiencyBonus = plan.SetProficiencyBonus;
                }
            }

            return snapshot;
        }
    }
}
