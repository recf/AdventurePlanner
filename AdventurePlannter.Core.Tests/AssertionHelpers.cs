using System;
using System.Collections.Generic;
using System.Linq;
using AdventurePlanner.Core.Domain;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AdventurePlannter.Core.Tests
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

        public static void AssertEqualSnapshots(CharacterSnapshot actualSnapshot, CharacterSnapshot expectedSnapshot)
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

                AssertEquivalentLists(
                    actual.Features,
                    expected.Features,
                    f => f.Name,
                    AssertEqualFeatures,
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

            AssertEquivalentLists(
                actualSnapshot.Features,
                expectedSnapshot.Features,
                f => f.Name,
                AssertEqualFeatures,
                "Features");

            AssertEquivalentLists(actualSnapshot.Armor, expectedSnapshot.Armor, a => a.Armor.Name, AssertEqualArmor, "Armor");
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

        public static void AssertEqualAttacks(AttackSnapshot actual, AttackSnapshot expected, string context)
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