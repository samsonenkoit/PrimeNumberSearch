using PrimeNumber.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public async Task<long> SearchNearestPrimeAsync(long startValue, CancellationToken token)
        {
            if (startValue < 2) return 2;
            if (startValue < 3) return 3;
            if (startValue < 5) return 5;
            if (startValue < 7) return 7;

            startValue += (startValue & 0x1) == 0 ? 1 : 2;

            while (true)
            {
                var minDivisorMaxValue = PrimeChecker.GetMinDivisorMaxValue(startValue);

                bool isPrime = !(await _checker.HaveOddDivisorAsync(startValue, 3, minDivisorMaxValue, token).ConfigureAwait(false));

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
