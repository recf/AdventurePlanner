using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AdventurePlanner.Core.Domain;
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

        [JsonProperty("class_plan", Required = Required.Always)]
        public ClassPlan ClassPlan { get; set; }

        [JsonProperty("level_plans", Required = Required.Always)]
        public IList<LevelPlan> LevelPlans { get; set; }

        [JsonProperty("armor", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<Armor> Armor { get; set; }

        [JsonProperty("weapons", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<Weapon> Weapons { get; set; }

        // TODO: :question: Consider moving ToSnapshot into an extension method.
        public PlayerCharacter ToSnapshot(int level)
        {
            var applicableLevels = LevelPlans.Where(l => l.Level <= level).ToList();

            var snapshot = new PlayerCharacter
            {
                CharacterLevel = level,

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

                ClassName = ClassPlan.ClassName
            };

            foreach (var prof in ClassPlan.ArmorProficiencies ?? new string[0])
            {
                snapshot.ArmorProficiencies.Add(prof);
            }

            foreach (var prof in ClassPlan.WeaponProficiencies ?? new string[0])
            {
                snapshot.WeaponProficiencies.Add(prof);
            }

            foreach (var prof in ClassPlan.ToolProficiencies ?? new string[0])
            {
                snapshot.ToolProficiencies.Add(prof);
            }

            foreach (var savingThrowKey in ClassPlan.SaveProficiencies ?? new string[0])
            {
                snapshot.SavingThrows[savingThrowKey].IsProficient = true;
            }

            foreach (var skillName in ClassPlan.SkillProficiencies ?? new string[0])
            {
                snapshot.Skills[skillName].IsProficient = true;
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
                    snapshot.Features.Add(new FeatureSnapshot()
                    {
                        Name = feature.Name,
                        Description = feature.Description
                    });
                }
            }

            foreach (var armor in Armor ?? new Armor[0])
            {
                snapshot.Armor.Add(new InventoryArmor(snapshot, armor));
            }

            foreach (var weapon in Weapons ?? new Weapon[0])
            {
                snapshot.Weapons.Add(new InventoryWeapon(snapshot, weapon));
            }

            return snapshot;
        }
    }
}
