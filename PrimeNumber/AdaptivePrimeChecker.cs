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

        private const long SynchronousMaxDivisorsCountDefault = 100;

        #endregion

        /// <summary>
        /// Если кол-во делителей, которые необходимо проверить для определения простоты числа меньше, то проверка осуществляется синхнонно,
        /// если больше, то в несколько потоков
        /// </summary>
        private readonly long _synchronousMaxDivisorsCount;

        private readonly MultiThreadPrimeChecker _multiThreadChecker;

        public AdaptivePrimeChecker(int maxThreadsCount, int searchIntervalLength,
            long synchronousMaxDivisorsCount = SynchronousMaxDivisorsCountDefault)
        {
            _synchronousMaxDivisorsCount = synchronousMaxDivisorsCount;
            _multiThreadChecker = new MultiThreadPrimeChecker(maxThreadsCount, searchIntervalLength);
        }

        public override async Task<bool> IsPrimeAsync(long num, CancellationToken token)
        {
            var baseCheck = BasePrimeCheck(num);

            if (baseCheck.IsPrime != null)
                return baseCheck.IsPrime.Value;

            var haveAdvisor = await HaveOddDivisorAsync(num, baseCheck.MinDivisionMinValue,
                baseCheck.MinDivisionMaxValue, token).ConfigureAwait(false);
            return !haveAdvisor;
        }

        internal override Task<bool> HaveOddDivisorAsync(long checkedValue, long start, long end, CancellationToken token)
        {
            if(end - start < _synchronousMaxDivisorsCount)
            {
                return Task.FromResult(HaveOddDivisor(checkedValue, start,
                    end, CancellationToken.None));
            }
            else
            {
                return _multiThreadChecker.HaveOddDivisorAsync(checkedValue, start, end, token);
            }
        }
    }
}
