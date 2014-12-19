using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventurePlanner.Core.Domain;
using NUnit.Framework;

namespace AdventurePlannter.Core.Tests
{
    [TestFixture]
    public class PlayerCharacterTests
    {
        [TestCase(1, 2)]
        [TestCase(2, 2)]
        [TestCase(3, 2)]
        [TestCase(4, 2)]

        [TestCase(5, 3)]
        [TestCase(6, 3)]
        [TestCase(7, 3)]
        [TestCase(8, 3)]
        
        [TestCase(9, 4)]
        [TestCase(10, 4)]
        [TestCase(11, 4)]
        [TestCase(12, 4)]
        
        [TestCase(13, 5)]
        [TestCase(14, 5)]
        [TestCase(15, 5)]
        [TestCase(16, 5)]
        
        [TestCase(17, 6)]
        [TestCase(18, 6)]
        [TestCase(19, 6)]
        [TestCase(20, 6)]
        public void TestProficiencyBonus(int characterLevel, int expectedProfBonus)
        {
            var snapshot = new PlayerCharacter { CharacterLevel = characterLevel };

            Assert.That(snapshot.ProficiencyBonus, Is.EqualTo(expectedProfBonus));
        }
    }
}
