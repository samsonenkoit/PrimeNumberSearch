using PrimeNumber.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeNumber
{
    /// <summary>
    /// В зависимости от размера проверяемого чила, класс может проверять его как в синхронном режиме, так и в многопоточном
    /// </summary>
    public class AdaptivePrimeChecker : PrimeChecker
    {
        #region Const

        private const long SynchronousMaxNumberDefault = 10000;

        #endregion

        /// <summary>
        /// Числа которые меньше данного, проверяются синхронно, которые больше, в многопоточном режиме
        /// </summary>
        private readonly long _synchronousMaxNumber;

        private readonly MultiThreadPrimeChecker _multiThreadChecker;

        public AdaptivePrimeChecker(int maxThreadsCount, int searchIntervalLength, long synchronousMaxNumber = SynchronousMaxNumberDefault)
        {
            _synchronousMaxNumber = synchronousMaxNumber;
            _multiThreadChecker = new MultiThreadPrimeChecker(maxThreadsCount, searchIntervalLength);
        }

        public override Task<bool> IsPrimeAsync(long num)
        {
            var baseCheck = BasePrimeCheck(num);

            if (baseCheck.IsPrime != null)
                return Task.FromResult(baseCheck.IsPrime.Value);

            if(num < _synchronousMaxNumber)
            {
                return Task.FromResult(!HaveOddDivisor(num, baseCheck.MinDivisionMinValue,
                    baseCheck.MinDivisionMaxValue, CancellationToken.None));
            }
            else
            {
                return _multiThreadChecker.IsPrimeAsync(num);
            }

        }
    }
}
