using PrimeNumber.Interface;
using PrimeNumber.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeNumber
{
    public class MultiThreadPrimeChecker : PrimeChecker
    {

        /// <summary>
        /// Максимальное кол-во потоков, которое может быть использовано при проверке на простоту
        /// </summary>
        private readonly int _maxThreadsCount;
        /// <summary>
        /// Длинна числового интервала, на котором за раз ищется делитель
        /// </summary>
        private readonly int _searchIntervalLength;

        public MultiThreadPrimeChecker(int maxThreadsCount, int searchIntervalLength)
        {
            if (maxThreadsCount < 1) throw new ArgumentOutOfRangeException($"{nameof(maxThreadsCount)} must be >= 1");
            if (searchIntervalLength < 1) throw new ArgumentOutOfRangeException($"{nameof(searchIntervalLength)} must be >= 1");

            _maxThreadsCount = maxThreadsCount;
            _searchIntervalLength = searchIntervalLength;

        }

        public override Task<bool> IsPrimeAsync(long num)
        {
            var baseCheck = BasePrimeCheck(num);

            if (baseCheck.IsPrime != null)
                return Task.FromResult(baseCheck.IsPrime.Value);

            return UnsafeIsPrimeAsync(num,baseCheck);
        }

        internal async Task<bool> UnsafeIsPrimeAsync(long checkedValue, BasePrimeCheckResult baseCheck)
        {
            CancellationTokenSource cancellSource = new CancellationTokenSource();
            PrimeDivisorManager divisorManager = new PrimeDivisorManager(baseCheck.MinDivisionMinValue,
                baseCheck.MinDivisionMaxValue, _searchIntervalLength);
            var searchTasks = GetSearchTasks(_maxThreadsCount, cancellSource, checkedValue, divisorManager);

            while(searchTasks.Count != 0)
            {
                var task = await Task.WhenAny(searchTasks).ConfigureAwait(false);

                if (!task.Result)
                {
                    cancellSource.Cancel();
                    return false;
                }
                else
                {
                    searchTasks.Remove(task);
                }

            }

            return true;
        }


        private Task<bool> GetCheckPrimeTaskAsync(long checkerNumber, PrimeDivisorManager divisorManager, CancellationToken token)
        {
            return Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();

                    var interval = divisorManager.GetSearchInterval();

                    if (interval == null) return true;

                    if (HaveOddDivisor(checkerNumber,interval.Item1, interval.Item2, token))
                        return false;

                }
            });
        }

        private List<Task<bool>> GetSearchTasks(int maxTasksCount, CancellationTokenSource cancellationSource,
            long checkedNumber, PrimeDivisorManager divisorManager)
        {
            var tasks = new List<Task<bool>>();

            for (int i = 0; i < maxTasksCount; i++)
                tasks.Add(GetCheckPrimeTaskAsync(checkedNumber, divisorManager, cancellationSource.Token));

            return tasks;
        }
    }
}
