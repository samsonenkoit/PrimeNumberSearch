using PrimeNumber.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeNumber
{
    public class PrimeNumberSearcher
    {
        private PrimeChecker _checker;

        public PrimeNumberSearcher(PrimeChecker checker)
        {
            if (checker == null) throw new ArgumentNullException(nameof(checker));

            _checker = checker;
        }

        public async Task<long> SearchNearestPrimeAsync(long startValue)
        {
            long firstPrime = startValue + 1;

            while (true)
            {
                bool isPrime = await _checker.IsPrimeAsync(firstPrime);

                if (isPrime)
                    return firstPrime;
                else
                {
                    firstPrime++;
                }
            }
        }
    }
}
