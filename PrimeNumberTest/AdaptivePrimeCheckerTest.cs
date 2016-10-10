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
    public class AdaptivePrimeCheckerTest
    {
        [Test]
        public void PrimeCheckTest()
        {
            var primeNumbers = PrimeNumberProvider.GetPrimeNumbers();

            AdaptivePrimeChecker checker = new AdaptivePrimeChecker(4, 100, 100);

            foreach (var prime in primeNumbers)
            {
                Assert.AreEqual(checker.IsPrimeAsync(prime, CancellationToken.None).Result, true);
            }

        }

        [Test]
        public void NotPrimeCheckTest()
        {
            var primeNumbers = PrimeNumberProvider.GetNotPrimeNumbers();

            AdaptivePrimeChecker checker = new AdaptivePrimeChecker(4, 100, 100);

            foreach (var prime in primeNumbers)
            {
                Assert.AreEqual(checker.IsPrimeAsync(prime, CancellationToken.None).Result, false);
            }

        }
    }
}
