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

        public override async Task<bool> IsPrimeAsync(long num)
        {
            var baseCheck = BasePrimeCheck(num);

            if (baseCheck.IsPrime != null)
                return baseCheck.IsPrime.Value;

            var haveAdvisor = await HaveOddDivisorAsync(num, baseCheck.MinDivisionMinValue, baseCheck.MinDivisionMaxValue);
            return !haveAdvisor;
        }

        internal override async Task<bool> HaveOddDivisorAsync(long checkedValue, long start, long end)
        {
            CancellationTokenSource cancellSource = new CancellationTokenSource();
            PrimeDivisorManager divisorManager = new PrimeDivisorManager(start,
                end, _searchIntervalLength);
            var searchTasks = GetSearchTasks(_maxThreadsCount, cancellSource, checkedValue, divisorManager);

            while(searchTasks.Count != 0)
            {
                var task = await Task.WhenAny(searchTasks).ConfigureAwait(false);

                if (task.Result)
                {
                    cancellSource.Cancel();
                    return true;
                }
                else
                {
                    searchTasks.Remove(task);
                }

            }

            return false;
        }


        private Task<bool> GetHaveAdvisorTaskAsync(long checkerNumber, PrimeDivisorManager divisorManager, CancellationToken token)
        {
            return Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();

                    var interval = divisorManager.GetSearchInterval();

                    if (interval == null) return false;

                    if (HaveOddDivisor(checkerNumber,interval.Item1, interval.Item2, token))
                        return true;

                }
            });
        }

        private List<Task<bool>> GetSearchTasks(int maxTasksCount, CancellationTokenSource cancellationSource,
            long checkedNumber, PrimeDivisorManager divisorManager)
        {
            var tasks = new List<Task<bool>>();

            for (int i = 0; i < maxTasksCount; i++)
                tasks.Add(GetHaveAdvisorTaskAsync(checkedNumber, divisorManager, cancellationSource.Token));

            return tasks;
        }
    }
}
