using NUnit.Framework;
using PrimeNumber;
using PrimeNumberTest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeNumberTest
{
    [TestFixture]
    public class MultiThreadPrimeCheckerTest
    {
        [Test]
        public void PrimeCheckTest()
        {
            var primeNumbers = PrimeNumberProvider.GetPrimeNumbers();

            MultiThreadPrimeChecker checker = new MultiThreadPrimeChecker(5, 7);

            foreach (var prime in primeNumbers)
            {
                Assert.AreEqual(checker.IsPrimeAsync(prime).Result, true);
            }
            
        }

        [Test]
        public void NotPrimeCheckTest()
        {
            var primeNumbers = PrimeNumberProvider.GetNotPrimeNumbers();

            MultiThreadPrimeChecker checker = new MultiThreadPrimeChecker(5, 7);

            foreach (var prime in primeNumbers)
            {
                Assert.AreEqual(checker.IsPrimeAsync(prime).Result, false);
            }

        }
    }
}
