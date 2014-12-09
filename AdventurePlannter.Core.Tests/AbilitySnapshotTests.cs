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
    public class AbilitySnapshotTests
    {
        [TestCase(8, -1)]
        [TestCase(9, -1)]
        [TestCase(10, 0)]
        [TestCase(11, 0)]
        [TestCase(12, 1)]
        public void TestModifier(int score, int expectedModifier)
        {
            var a = new AbilityScore("Tes", "Test Ability") { Score = score };
            Assert.That(a.Modifier, Is.EqualTo(expectedModifier));
        }
    }
}
