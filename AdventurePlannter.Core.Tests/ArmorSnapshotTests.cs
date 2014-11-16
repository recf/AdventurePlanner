using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventurePlanner.Core.Snapshots;
using NUnit.Framework;

namespace AdventurePlannter.Core.Tests
{
    [TestFixture]
    public class ArmorSnapshotTests
    {
        [Test]
        public void TestIsProficient()
        {
            var prof = "light armor";

            var character = new CharacterSnapshot();
            var armor = new ArmorSnapshot(character)
            {
                ProficiencyGroup = prof
            };

            Assert.That(armor.IsProficient, Is.False);

            character.ArmorProficiencies.Add(prof);

            Assert.That(armor.IsProficient, Is.True);
        }

        [TestCase(11, null, 14)]
        [TestCase(13, 2, 15)]
        [TestCase(18, 0, 18)]
        public void TestArmorClass(int baseAc, int? maxDexMod, int expectedTotalAc)
        {
            var character = new CharacterSnapshot();
            var dex = character.Abilities["Dex"];
            dex.Score = 16;

            Assert.That(dex.Modifier, Is.EqualTo(3));

            var armor = new ArmorSnapshot(character)
            {
                BaseArmorClass = baseAc,
                MaximumDexterityModifier = maxDexMod
            };

            Assert.That(armor.TotalArmorClass, Is.EqualTo(expectedTotalAc));
        }
    }
}
