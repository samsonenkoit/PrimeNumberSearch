using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeNumber
{
    /// <summary>
    /// Вспомогательный класс, используется для распределения числовых интервалов при многопоточной проверке числа на простоту
    /// </summary>
    internal class PrimeDivisorManager
    {
        
        private readonly long _end;
        private readonly long _intervalLength;

        private object _locker;

        /// <summary>
        /// Последний отданный делитель,
        /// используется так же как начальное значение для 1го интервала
        /// </summary>
        private long _lastGiverDivisor;

        internal PrimeDivisorManager(long start, long end, long intervalLength)
        {
            if (end < start) throw new ArgumentOutOfRangeException($"{nameof(end)} must be > {nameof(start)}");
            if (intervalLength < 1) throw new ArgumentOutOfRangeException($"{nameof(intervalLength)} must be >= 1");

            _lastGiverDivisor = start - 1;
            _end = end;
            _intervalLength = intervalLength;
            _locker = new object();
        }

        internal Tuple<long, long> GetSearchInterval()
        {
            lock (_locker)
            {
                long start = _lastGiverDivisor + 1;

                if (start > _end) return null;

                long end = _lastGiverDivisor + _intervalLength * 2;

                if (end > _end)
                    end = _end;

                _lastGiverDivisor = end;

                return new Tuple<long, long>(start, end);
            }
        }
    }
}
