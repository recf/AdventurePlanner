using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AdventurePlanner.Domain;
using NUnit.Framework;
using Polyhedral;

namespace AdventurePlanner.Domain.Tests
{
    [TestFixture]
    public class InventoryWeaponTests
    {
        public class AttackTestCaseData
        {
            public string TestName { get; set; }

            public PlayerCharacter Snapshot { get; set; }

            public Weapon Weapon { get; set; }

            public IList<Attack> ExpectedAttacks { get; set; }

            #region Overrides of Object

            public override string ToString()
            {
                return TestName;
            }

            #endregion
        }

        public IEnumerable<AttackTestCaseData> AttackCases
        {
            get
            {
                var snapshot = new PlayerCharacter();
                snapshot.CharacterLevel = 1;
                snapshot.Abilities["Str"].Score = 16;
                snapshot.Abilities["Dex"].Score = 12;

                snapshot.WeaponProficiencies.Add("Flails");
                snapshot.WeaponProficiencies.Add("Crossbows");
                snapshot.WeaponProficiencies.Add("Shortswords");

                yield return new AttackTestCaseData()
                {
                    TestName = "Proficient Melee Weapon",
                    Snapshot = snapshot,
                    Weapon = new Weapon
                    {
                        Name = "Flail",
                        ProficiencyGroup = "Flails",
                        DamageDice = new DiceRoll(d8: 1),
                        DamageType = "blugeoning"
                    },
                    ExpectedAttacks = new List<Attack>()
                    {
                        new Attack(snapshot)
                        {
                            Name = "Melee Attack",
                            AttackModifier = 5,
                            DamageDice = new DiceRoll(d8: 1, modifier: 3),
                            DamageType = "blugeoning"
                        }
                    }
                };

                yield return new AttackTestCaseData()
                {
                    TestName = "Proficient Ranged Weapon",
                    Snapshot = snapshot,
                    Weapon = new Weapon
                    {
                        Name = "Crossbow, light",
                        ProficiencyGroup = "Crossbows",
                        NormalRange = 80,
                        MaximumRange = 320,
                        HasAmmunition = true,
                        DamageDice = new DiceRoll(d8: 1),
                        DamageType = "piercing"
                    },
                    ExpectedAttacks = new List<Attack>()
                    {
                        new Attack(snapshot)
                        {
                            Name = "Ranged Attack",
                            AttackModifier = 3,
                            DamageDice = new DiceRoll(d8: 1, modifier: 1),
                            DamageType = "piercing",
                            NormalRange = 80,
                            MaximumRange = 320
                        }
                    }
                };

                yield return new AttackTestCaseData()
                {
                    TestName = "Proficient Light Melee Weapon",
                    Snapshot = snapshot,
                    Weapon = new Weapon
                    {
                        Name = "Shortsword",
                        ProficiencyGroup = "Shortswords",
                        IsLight= true,
                        DamageDice = new DiceRoll(d6: 1),
                        DamageType = "piercing"
                    },
                    ExpectedAttacks = new List<Attack>()
                    {
                        new Attack(snapshot)
                        {
                            Name = "Melee Attack",
                            AttackModifier = 5,
                            DamageDice = new DiceRoll(d6: 1, modifier: 3),
                            DamageType = "piercing"
                        },
                        
                        new Attack(snapshot)
                        {
                            Name = "Melee Attack (Bonus Action)",
                            AttackModifier = 5,
                            DamageDice = new DiceRoll(d6: 1),
                            DamageType = "piercing"
                        }
                    }
                };
            }
        }

        [Test]
        [TestCaseSource("AttackCases")]
        public void TestGetAttacks(AttackTestCaseData caseData)
        {
            var invWeapon = new InventoryWeapon(caseData.Snapshot, caseData.Weapon);
            var actualAttacks = invWeapon.GetAttacks();

            AssertionHelpers.AssertEquivalentLists(actualAttacks, caseData.ExpectedAttacks, a => a.Name, AssertionHelpers.AssertEqualAttacks, string.Empty);
        }
    }
}
