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
            if (startValue < 2) return 2;
            if (startValue == 2) return 3;
            
            startValue += (startValue & 0x1) == 0 ? 1 : 2;
            
            while (true)
            {
                bool isPrime = await _checker.IsPrimeAsync(startValue).ConfigureAwait(false);

                if (isPrime)
                    return startValue;
                else
                {
                    startValue += 2;
                }
            }
        }
    }
}
