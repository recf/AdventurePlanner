using System;
using System.Collections.Generic;
using System.Linq;
using AdventurePlanner.Domain;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AdventurePlanner.Domain.Tests
{
    public class AssertionHelpers
    {
        public static void AssertEquivalentLists<TSource, TKey>(
            IList<TSource> actualList,
            IList<TSource> expectedList,
            Func<TSource, TKey> keySelector,
            Action<TSource, TSource, string> itemAssert,
            string context)
        {
            var actual = actualList.OrderBy(keySelector).ToList();
            var expected = expectedList.OrderBy(keySelector).ToList();
            
            Assert.That(actual.Count, Is.EqualTo(expected.Count), context + ".Count");

            for (var i = 0; i < expected.Count; i++)
            {
                itemAssert(actual[i], expected[i], string.Format("{0}[{1}]", context, i));
            }
        }

        public static void AssertEqualSnapshots(PlayerCharacter actualChar, PlayerCharacter expectedChar)
        {
            Assert.That(actualChar.Name, Is.EqualTo(expectedChar.Name));
            Assert.That(actualChar.Race, Is.EqualTo(expectedChar.Race));
            Assert.That(actualChar.Alignment, Is.EqualTo(expectedChar.Alignment));
            Assert.That(actualChar.Background, Is.EqualTo(expectedChar.Background));
            Assert.That(actualChar.Age, Is.EqualTo(expectedChar.Age));
            Assert.That(actualChar.HeightFeet, Is.EqualTo(expectedChar.HeightFeet));
            Assert.That(actualChar.HeightInches, Is.EqualTo(expectedChar.HeightInches));
            Assert.That(actualChar.Weight, Is.EqualTo(expectedChar.Weight));
            Assert.That(actualChar.EyeColor, Is.EqualTo(expectedChar.EyeColor));
            Assert.That(actualChar.HairColor, Is.EqualTo(expectedChar.HairColor));
            Assert.That(actualChar.SkinColor, Is.EqualTo(expectedChar.SkinColor));

            Assert.That(actualChar.ClassName, Is.EqualTo(expectedChar.ClassName));

            foreach (var abbr in expectedChar.Abilities.Keys)
            {
                var actual = actualChar.Abilities[abbr];
                var expected = expectedChar.Abilities[abbr];

                Assert.That(actual.Score, Is.EqualTo(expected.Score), "Abilities[{0}].Score", abbr);
            }

            Assert.That(actualChar.CharacterLevel, Is.EqualTo(expectedChar.CharacterLevel), "CharacterLevel");
            Assert.That(actualChar.ProficiencyBonus, Is.EqualTo(expectedChar.ProficiencyBonus), "ProficiencyBonus");

            foreach (var skillName in expectedChar.Skills.Keys)
            {
                var actual = actualChar.Skills[skillName];
                var expected = expectedChar.Skills[skillName];

                Assert.That(actual.IsProficient, Is.EqualTo(expected.IsProficient), "Skills[{0}].IsProficient", skillName);
            }

            foreach (var savingThrowKey in expectedChar.SavingThrows.Keys)
            {
                var actual = actualChar.SavingThrows[savingThrowKey];
                var expected = expectedChar.SavingThrows[savingThrowKey];

                Assert.That(
                    actual.IsProficient,
                    Is.EqualTo(expected.IsProficient),
                    "SavingThrows[{0}].IsProficient",
                    savingThrowKey);
            }

            Assert.That(actualChar.ArmorProficiencies, Is.EquivalentTo(expectedChar.ArmorProficiencies));
            Assert.That(actualChar.WeaponProficiencies, Is.EquivalentTo(expectedChar.WeaponProficiencies));
            Assert.That(actualChar.ToolProficiencies, Is.EquivalentTo(expectedChar.ToolProficiencies));

            AssertEquivalentLists(
                actualChar.Features,
                expectedChar.Features,
                f => f.Name,
                AssertEqualFeatures,
                "Features");

            AssertEquivalentLists(actualChar.Armor, expectedChar.Armor, a => a.Armor.Name, AssertEqualArmor, "Armor");
        }

        public static void AssertEqualArmor(InventoryArmor actual, InventoryArmor expected, string context)
        {
            Assert.That(actual.Armor.Name, Is.EqualTo(expected.Armor.Name), "{0}.Name", context);
            Assert.That(actual.Armor.ArmorClass, Is.EqualTo(expected.Armor.ArmorClass), "{0}.BaseArmorClass", context);
            Assert.That(actual.Armor.ProficiencyGroup, Is.EqualTo(expected.Armor.ProficiencyGroup), "{0}.ProficiencyGroup", context);
            Assert.That(actual.Armor.MaximumDexterityModifier, Is.EqualTo(expected.Armor.MaximumDexterityModifier), "{0}.MaximumDexterityModifier", context);
        }

        public static void AssertEqualFeatures(FeatureSnapshot actual, FeatureSnapshot expected, string context)
        {
            Assert.That(actual.Name, Is.EqualTo(expected.Name), "{0}.Name", context);
            Assert.That(actual.Description, Is.EqualTo(expected.Description), "{0}.Description", context);
        }

        public static void AssertEqualAttacks(Attack actual, Attack expected, string context)
        {
            Assert.That(actual.Name, Is.EqualTo(expected.Name), "{0}.Name", context);
            Assert.That(actual.AttackModifier, Is.EqualTo(expected.AttackModifier), "{0}.AttackModifier", context);
            Assert.That(actual.DamageType, Is.EqualTo(expected.DamageType), "{0}.DamageType", context);
            Assert.That(actual.DamageDice, Is.EqualTo(expected.DamageDice), "{0}.DamageDice", context);
            Assert.That(actual.NormalRange, Is.EqualTo(expected.NormalRange), "{0}.NormalRange", context);
            Assert.That(actual.MaximumRange, Is.EqualTo(expected.MaximumRange), "{0}.MaximumRange", context);
        }
    }
}