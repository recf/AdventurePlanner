using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventurePlanner.Core;
using AdventurePlanner.Core.Meta;
using AdventurePlanner.Core.Planning;
using AdventurePlanner.Core.Snapshots;
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

                ArmorPlans = new List<ArmorPlan>
                {
                    new ArmorPlan
                    {
                        ArmorName = "Padded",
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

            expectedSnapshot.Armor.Add(new ArmorSnapshot(expectedSnapshot)
            {
                ArmorName = "Padded",
                BaseArmorClass = 11,
                MaximumDexterityModifier = null,
                ProficiencyGroup = "light armor"
            });

            var actualSnapshot = plan.ToSnapshot(snapshotLevel);

            AssertEqualSnapshots(actualSnapshot, expectedSnapshot);
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

        #region Assertion Helpers

        private void AssertEqualSnapshots(CharacterSnapshot actualSnapshot, CharacterSnapshot expectedSnapshot)
        {
            Assert.That(actualSnapshot.Name, Is.EqualTo(expectedSnapshot.Name));
            Assert.That(actualSnapshot.Race, Is.EqualTo(expectedSnapshot.Race));
            Assert.That(actualSnapshot.Alignment, Is.EqualTo(expectedSnapshot.Alignment));
            Assert.That(actualSnapshot.Background, Is.EqualTo(expectedSnapshot.Background));
            Assert.That(actualSnapshot.Age, Is.EqualTo(expectedSnapshot.Age));
            Assert.That(actualSnapshot.HeightFeet, Is.EqualTo(expectedSnapshot.HeightFeet));
            Assert.That(actualSnapshot.HeightInches, Is.EqualTo(expectedSnapshot.HeightInches));
            Assert.That(actualSnapshot.Weight, Is.EqualTo(expectedSnapshot.Weight));
            Assert.That(actualSnapshot.EyeColor, Is.EqualTo(expectedSnapshot.EyeColor));
            Assert.That(actualSnapshot.HairColor, Is.EqualTo(expectedSnapshot.HairColor));
            Assert.That(actualSnapshot.SkinColor, Is.EqualTo(expectedSnapshot.SkinColor));

            Assert.That(actualSnapshot.Classes, Is.EquivalentTo(expectedSnapshot.Classes));

            foreach (var abbr in expectedSnapshot.Abilities.Keys)
            {
                var actual = actualSnapshot.Abilities[abbr];
                var expected = expectedSnapshot.Abilities[abbr];

                Assert.That(actual.Score, Is.EqualTo(expected.Score), "Abilities[{0}].Score", abbr);
            }

            Assert.That(actualSnapshot.ProficiencyBonus, Is.EqualTo(expectedSnapshot.ProficiencyBonus));

            foreach (var skillName in expectedSnapshot.Skills.Keys)
            {
                var actual = actualSnapshot.Skills[skillName];
                var expected = expectedSnapshot.Skills[skillName];

                Assert.That(actual.IsProficient, Is.EqualTo(expected.IsProficient), "Skills[{0}].IsProficient", skillName);

                AssertionHelpers.AssertEquivalentLists(
                    actual.Features,
                    expected.Features,
                    string.Format("Skills[{0}].Features", skillName));
            }

            foreach (var savingThrowKey in expectedSnapshot.SavingThrows.Keys)
            {
                var actual = actualSnapshot.SavingThrows[savingThrowKey];
                var expected = expectedSnapshot.SavingThrows[savingThrowKey];

                Assert.That(
                    actual.IsProficient,
                    Is.EqualTo(expected.IsProficient),
                    "SavingThrows[{0}].IsProficient",
                    savingThrowKey);
            }

            Assert.That(actualSnapshot.ArmorProficiencies, Is.EquivalentTo(expectedSnapshot.ArmorProficiencies));
            Assert.That(actualSnapshot.WeaponProficiencies, Is.EquivalentTo(expectedSnapshot.WeaponProficiencies));
            Assert.That(actualSnapshot.ToolProficiencies, Is.EquivalentTo(expectedSnapshot.ToolProficiencies));

            AssertionHelpers.AssertEquivalentLists(actualSnapshot.Features, expectedSnapshot.Features, "Features");

            AssertionHelpers.AssertEquivalentLists(actualSnapshot.Armor, expectedSnapshot.Armor, "Armor");
        }

        #endregion
    }
}
