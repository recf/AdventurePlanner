using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventurePlanner.Core;
using AdventurePlanner.Core.Domain;
using AdventurePlanner.Core.Planning;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using Polyhedral;

namespace AdventurePlannter.Core.Tests
{
    [TestFixture]
    public class CharacterPlanTests
    {
        // TODO: Figure out how I want to test multiple cases (snapshot at different levels, etc).
        [Test]
        public void TestToSnapshot()
        {
            var fighter = Fighter();

            var cleric = new ClassPlan
            {
                ClassName = "Cleric",

                ArmorProficiencies = new[] { "Light Armor", "Medium Armor", "Shields" },
                WeaponProficiencies = new[] { "Simple Weapons", "Martial Weapons" },
                ToolProficiencies = new[] { "Cleric kit" },

                SaveProficiencies = new[] { "Wis", "Cha" },
                SkillProficiencies = new[] { "Perception", "Insight" },
            };

            var plan = new CharacterPlan
            {
                Name = "Balin Thundershield",
                Race = "Dwarf",
                Speed = 25,
                Alignment = "TN",
                Background = "Artisan",
                Age = 78,
                HeightFeet = 5,
                HeightInches = 0,
                Weight = 150,
                EyeColor = "Brown",
                HairColor = "Rust",
                SkinColor = "Tan",

                ClassPlans = new List<ClassPlan>
                {
                    fighter,
                    cleric
                },

                LevelPlans = new List<LevelPlan>
                {
                    new LevelPlan
                    {
                        Level = 1,
                        ClassPlan = fighter,

                        AbilityScoreImprovements = AbilityScores(10, 12, 14, 8, 15, 11),

                        SetProficiencyBonus = 2,

                        FeaturePlans = new List<FeaturePlan>()
                        {
                            new FeaturePlan() { Name = "Quick Wits" },
                            new FeaturePlan()
                            {
                                Name = "Nimble",
                                Description = "Half penalty on rough terrain",
                                SkillName = "Acrobatics"
                            }
                        },
                    },
                    new LevelPlan
                    {
                        Level = 2,
                        ClassPlan = cleric,
                    },
                    new LevelPlan
                    {
                        Level = 3,
                        ClassPlan = cleric,
                    },
                    new LevelPlan
                    {
                        Level = 4,
                        ClassPlan = cleric,

                        AbilityScoreImprovements = new Dictionary<string, int>()
                        {
                            { "Wis", 1 },
                        },
                    },
                    new LevelPlan
                    {
                        Level = 4,
                        ClassPlan = cleric,
                        SetProficiencyBonus = 3
                    }
                },

                ArmorPlans = new List<Armor>
                {
                    new Armor
                    {
                        Name = "Padded",
                        ArmorClass = 11,
                        MaximumDexterityModifier = null,
                        ProficiencyGroup = "light armor"
                    }
                }
            };

            var snapshotLevel = 20;

            var expectedSnapshot = new CharacterSnapshot
            {
                Name = "Balin Thundershield",
                Race = "Dwarf",
                Speed = 25,
                Alignment = "TN",
                Background = "Artisan",
                Age = 78,
                HeightFeet = 5,
                HeightInches = 0,
                Weight = 150,
                EyeColor = "Brown",
                HairColor = "Rust",
                SkinColor = "Tan",

                Classes = new Dictionary<string, int> { { "Cleric", 4 }, { "Fighter", 1 } },

                ProficiencyBonus = 3,
            };
            expectedSnapshot.Abilities["Str"].Score = 10;
            expectedSnapshot.Abilities["Dex"].Score = 12;
            expectedSnapshot.Abilities["Con"].Score = 14;
            expectedSnapshot.Abilities["Int"].Score = 8;
            expectedSnapshot.Abilities["Wis"].Score = 16;
            expectedSnapshot.Abilities["Cha"].Score = 11;

            expectedSnapshot.SavingThrows["Str"].IsProficient = true;
            expectedSnapshot.SavingThrows["Con"].IsProficient = true;
            expectedSnapshot.SavingThrows["Wis"].IsProficient = true;
            expectedSnapshot.SavingThrows["Cha"].IsProficient = true;

            expectedSnapshot.Skills["Perception"].IsProficient = true;
            expectedSnapshot.Skills["Insight"].IsProficient = true;
            expectedSnapshot.Skills["Athletics"].IsProficient = true;

            expectedSnapshot.Skills["Acrobatics"].Features.Add(new FeatureSnapshot()
            {
                Name = "Nimble",
                Description = "Half penalty on rough terrain"
            });

            foreach (var prof in new[] { "All Armor", "Light Armor", "Medium Armor", "Shields" })
            {
                expectedSnapshot.ArmorProficiencies.Add(prof);
            }
            foreach (var prof in new[] { "Simple Weapons", "Martial Weapons" })
            {
                expectedSnapshot.WeaponProficiencies.Add(prof);
            }
            foreach (var prof in new[] { "Fighter kit", "Cleric kit" })
            {
                expectedSnapshot.ToolProficiencies.Add(prof);
            }

            expectedSnapshot.Features.Add(
                new FeatureSnapshot { Name = "Quick Wits" });

            var armor = new Armor
            {
                Name = "Padded",
                ArmorClass = 11,
                MaximumDexterityModifier = null,
                ProficiencyGroup = "light armor"
            };
            expectedSnapshot.Armor.Add(new InventoryArmor(expectedSnapshot, armor));

            var actualSnapshot = plan.ToSnapshot(snapshotLevel);

            AssertionHelpers.AssertEqualSnapshots(actualSnapshot, expectedSnapshot);
        }

        #region Construction Helpers

        public ClassPlan Fighter()
        {
            return new ClassPlan
            {
                ClassName = "Fighter",

                ArmorProficiencies = new[] { "All Armor", "Shields" },
                WeaponProficiencies = new[] { "Simple Weapons", "Martial Weapons" },
                ToolProficiencies = new[] { "Fighter kit" },

                SaveProficiencies = new[] { "Str", "Con" },
                SkillProficiencies = new[] { "Athletics" },
            };
        }

        public IDictionary<string, int> AbilityScores(
            int strScore = 10,
            int dexScore = 10,
            int conScore = 10,
            int intScore = 10,
            int wisScore = 10,
            int chaScore = 10)
        {
            return new Dictionary<string, int>()
            {
                { "Str", strScore },
                { "Dex", dexScore },
                { "Con", conScore },
                { "Int", intScore },
                { "Wis", wisScore },
                { "Cha", chaScore },
            };
        }

        #endregion
        
    }
}
