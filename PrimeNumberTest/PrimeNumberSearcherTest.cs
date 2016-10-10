using NUnit.Framework;
using PrimeNumber;
using PrimeNumberTest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeNumberTest
{
    [TestFixture]
    public class PrimeNumberSearcherTest
    {
        [Test]
        public void SearchTest()
        {
            var primeNumbersDict = PrimeNumberProvider.GetValueFirstPrimeDicionary();

            PrimeNumberSearcher searcher = new PrimeNumberSearcher(new AdaptivePrimeChecker(4, 100, 1000));

            foreach(var item in primeNumbersDict)
            {
                Assert.AreEqual(item.Value, searcher.SearchNearestPrimeAsync(item.Key, CancellationToken.None).Result);
            }
        }
    }
}
