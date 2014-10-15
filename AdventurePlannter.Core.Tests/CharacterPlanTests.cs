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

namespace AdventurePlannter.Core.Tests
{
    [TestFixture]
    public class CharacterPlanTests
    {
        // TODO: Figure out how I want to test multiple cases (snapshot at different levels, etc).
        [Test]
        public void TestToSnapshot()
        {
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

                LevelPlans = new List<CharacterLevelPlan>
                {
                    new CharacterLevelPlan
                    {
                        Level = 1,
                        ClassName = "Fighter",

                        IncreaseStr = 10,
                        IncreaseDex = 12,
                        IncreaseCon = 14,
                        IncreaseInt = 8,
                        IncreaseWis = 15,
                        IncreaseCha = 11,

                        SetProficiencyBonus = 2,

                        NewSkillProficiencies = new[] { "Perception", "Insight" }
                    },
                    new CharacterLevelPlan { Level = 2, ClassName = "Cleric" },
                    new CharacterLevelPlan { Level = 3, ClassName = "Cleric" },
                    new CharacterLevelPlan
                    {
                        Level = 4,
                        ClassName = "Cleric",
                        IncreaseWis = 1,
                        NewSkillProficiencies = new[] { "Athletics" }
                    },
                    new CharacterLevelPlan { Level = 4, ClassName = "Cleric", SetProficiencyBonus = 3 }
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
                
                ProficiencyBonus = 3
            };
            expectedSnapshot.Abilities["Str"].Score = 10;
            expectedSnapshot.Abilities["Dex"].Score = 12;
            expectedSnapshot.Abilities["Con"].Score = 14;
            expectedSnapshot.Abilities["Int"].Score = 8;
            expectedSnapshot.Abilities["Wis"].Score = 16;
            expectedSnapshot.Abilities["Cha"].Score = 11;

            expectedSnapshot.Skills["Perception"].IsProficient = true;
            expectedSnapshot.Skills["Insight"].IsProficient = true;
            expectedSnapshot.Skills["Athletics"].IsProficient = true;
            
            var actualSnapshot = plan.ToSnapshot(snapshotLevel);

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

            foreach (var skillName in expectedSnapshot.Skills.Keys)
            {
                var actual = actualSnapshot.Skills[skillName];
                var expected = expectedSnapshot.Skills[skillName];

                Assert.That(actual.IsProficient, Is.EqualTo(expected.IsProficient), "Skills[{0}].IsProficient", skillName);
            }

            Assert.That(actualSnapshot.ProficiencyBonus, Is.EqualTo(expectedSnapshot.ProficiencyBonus));
        }
    }
}
