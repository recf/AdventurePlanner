using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventurePlanner.Core;
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
                        IncreaseCha = 11
                    },
                    new CharacterLevelPlan { Level = 2, ClassName = "Cleric" },
                    new CharacterLevelPlan { Level = 3, ClassName = "Cleric" },
                    new CharacterLevelPlan { Level = 4, ClassName = "Cleric", IncreaseWis = 1 }
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

                Classes = new Dictionary<string, int> { { "Cleric", 3 }, { "Fighter", 1 } },

                StrScore = { Score = 10 },
                DexScore = { Score = 12 },
                ConScore = { Score = 14 },
                IntScore = { Score = 8 },
                WisScore = { Score = 16 },
                ChaScore = { Score = 11 },
            };
            
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

            Assert.That(actualSnapshot.StrScore.Score, Is.EqualTo(expectedSnapshot.StrScore.Score));
            Assert.That(actualSnapshot.DexScore.Score, Is.EqualTo(expectedSnapshot.DexScore.Score));
            Assert.That(actualSnapshot.ConScore.Score, Is.EqualTo(expectedSnapshot.ConScore.Score));
            Assert.That(actualSnapshot.IntScore.Score, Is.EqualTo(expectedSnapshot.IntScore.Score));
            Assert.That(actualSnapshot.WisScore.Score, Is.EqualTo(expectedSnapshot.WisScore.Score));
            Assert.That(actualSnapshot.ChaScore.Score, Is.EqualTo(expectedSnapshot.ChaScore.Score));
        }
    }
}
