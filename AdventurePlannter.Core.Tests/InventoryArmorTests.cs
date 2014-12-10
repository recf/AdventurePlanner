using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventurePlanner.Core.Domain;
using AdventurePlanner.Core.Planning;
using NUnit.Framework;

namespace AdventurePlannter.Core.Tests
{
    [TestFixture]
    public class InventoryArmorTests
    {
        [Test]
        public void TestIsProficient()
        {
            var prof = "light armor";

            var character = new PlayerCharacter();

            var armor = new Armor
            {
                ProficiencyGroup = prof
            };

            var inventoryArmor = new InventoryArmor(character, armor);

            Assert.That(inventoryArmor.IsProficient, Is.False);

            character.ArmorProficiencies.Add(prof);

            Assert.That(inventoryArmor.IsProficient, Is.True);
        }

        [TestCase(11, null, 14)]
        [TestCase(13, 2, 15)]
        [TestCase(18, 0, 18)]
        public void TestArmorClass(int baseAc, int? maxDexMod, int expectedTotalAc)
        {
            var character = new PlayerCharacter();
            var dex = character.Abilities["Dex"];
            dex.Score = 16;

            Assert.That(dex.Modifier, Is.EqualTo(3));

            var armor = new Armor
            {
                ArmorClass = baseAc,
                MaximumDexterityModifier = maxDexMod
            };

            var inventoryArmor = new InventoryArmor(character, armor);

            Assert.That(inventoryArmor.ArmorClass, Is.EqualTo(expectedTotalAc));
        }
    }
}
