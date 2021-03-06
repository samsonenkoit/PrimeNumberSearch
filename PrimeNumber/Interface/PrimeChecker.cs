﻿using PrimeNumber.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeNumber.Interface
{
    /// <summary>
    /// Базовый абстрактный класс. Инкапсулирующий ряд общийх ф-й для проверки на простоту. 
    /// Предоставляет интерфейс ф-и проверки.
    /// </summary>
    public abstract class PrimeChecker
    {

        #region Static

        /// <summary>
        /// Возвращает максимальное значение минимального делителя числа. Используется св-во что 
        /// число, на которое делится натуральное число n, не превышает целой части квадратного корня из числа n
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        internal static long GetMinDivisorMaxValue(long number)
        {
            long maxDivisor = (long)Math.Truncate(Math.Sqrt(number));
            return maxDivisor;
        }

        #endregion


        abstract public Task<bool> IsPrimeAsync(long num, CancellationToken token);
        abstract internal Task<bool> HaveOddDivisorAsync(long checkedValue, long start, long end, CancellationToken token);

        /// <summary>
        /// Осуществляет базовую проверку на простоту.
        /// True - число простое
        /// False - не четное
        /// Null - необходимо проверять дальше
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected virtual BasePrimeCheckResult BasePrimeCheck(long number)
        {
            var result = new BasePrimeCheckResult();

            if(number < 2)
            {
                result.IsPrime = false;
            }
            else if(number == 2 || number == 3)
            {
                result.IsPrime = true;
            }
            else if((number & 0x1) == 0)
            {
                result.IsPrime = false;
            }
            else
            {
                var minDivisionMaxValue = GetMinDivisorMaxValue(number);

                if (minDivisionMaxValue < 3)
                    result.IsPrime = true;
                else
                {
                    result.IsPrime = null;
                    result.MinDivisionMaxValue = minDivisionMaxValue;
                    result.MinDivisionMinValue = 3; //все числа с делителями меньше 3 уже проверены
                }

            }


            return result;

        }


        /// <summary>
        /// Проверяет есть ли у числа number нечетный делитель в интервале [start,end]
        /// ВАЖНО: start - должен быть нечетным числом.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        protected virtual bool HaveOddDivisor(long number, long start, long end, CancellationToken token)
        {
            for (long divisor = start; divisor <= end; divisor += 2)
            {
                token.ThrowIfCancellationRequested();

                if (number % divisor == 0)
                    return true;
            }

            return false;
        }

    }
}
