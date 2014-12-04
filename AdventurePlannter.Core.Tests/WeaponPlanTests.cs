using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AdventurePlanner.Core.Planning;
using AdventurePlanner.Core.Snapshots;
using NUnit.Framework;
using Polyhedral;

namespace AdventurePlannter.Core.Tests
{
    [TestFixture]
    public class WeaponPlanTests
    {
        [Test]
        public void TestAttackModifier([Random(0, 18, 5)] int abilityScore)
        {
            var snapshot = new CharacterSnapshot();
            var strength = snapshot.Abilities["Str"];
            strength.Score = abilityScore;

            var attack = new AttackSnapshot(snapshot)
            {
                Name = "Flail",
                Ability = strength,
            };

            Assert.That(attack.AttackModifier, Is.EqualTo(attack.Ability.Modifier));
        }

        [Test]
        public void TestBasicMeleeAttack()
        {
            var weapon = new WeaponPlan
            {
                Name = "Flail",
                ProficiencyGroup = "Flails",
                DamageDice = new DiceRoll(d8: 1),
                DamageType = "blugeoning"
            };
            
            var snapshot = new CharacterSnapshot();
            var strength = snapshot.Abilities["Str"];
            strength.Score = 12;

            var expectedAttacks = new List<AttackSnapshot>()
            {
                new AttackSnapshot(snapshot)
                {
                    Name = "Flail",
                    Ability = strength,
                    DamageDice = new DiceRoll(d8: 1, modifier: 1),
                    DamageType = "blugeoning"
                }
            };

            var actualAttacks = weapon.GetAttacks(snapshot);

            AssertionHelpers.AssertEquivalentLists(actualAttacks, expectedAttacks, a => a.Name, AssertionHelpers.AssertEqualAttacks, string.Empty);
        }
        
        [Test]
        public void TestRangeAndAmmo()
        {
            var weapon = new WeaponPlan
            {
                Name = "Crossbow, light",
                ProficiencyGroup = "Crossbows",
                NormalRange = 80,
                MaximumRange = 320,
                HasAmmunition = true,
                DamageDice = new DiceRoll(d8: 1),
                DamageType = "piercing"
            };

            var snapshot = new CharacterSnapshot();
            var dex = snapshot.Abilities["Dex"];
            dex.Score = 14;

            var expectedAttacks = new List<AttackSnapshot>()
            {
                new AttackSnapshot(snapshot)
                {
                    Name = "Crossbow, light",
                    DamageDice = new DiceRoll(d8: 1, modifier: 2),
                    DamageType = "piercing",
                    Ability = dex,
                    NormalRange = 80,
                    MaximumRange = 320
                }
            };

            var actualAttacks = weapon.GetAttacks(snapshot);

            AssertionHelpers.AssertEquivalentLists(actualAttacks, expectedAttacks, a => a.Name, AssertionHelpers.AssertEqualAttacks, string.Empty);
        }
    }
}
