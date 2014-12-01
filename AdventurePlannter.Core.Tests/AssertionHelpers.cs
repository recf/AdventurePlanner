using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AdventurePlannter.Core.Tests
{
    public class AssertionHelpers
    {
        public static void AssertEquivalentLists<T>(
            IList<T> actualList,
            IList<T> expectedList,
            string context)
        {
            var actual = actualList.Select(x => JsonConvert.SerializeObject(x)).ToArray();
            var expected = expectedList.Select(x => JsonConvert.SerializeObject(x)).ToArray();

            Assert.That(actual, Is.EquivalentTo(expected), context);
        }
    }
}