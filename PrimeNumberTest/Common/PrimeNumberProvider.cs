using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeNumberTest.Common
{
    public class PrimeNumberProvider
    {
        public static long[] GetPrimeNumbers()
        {
            return new long[] { 2, 3, 5, 7, 11,13, 17, 19, 23, 29, 73, 79, 83, 1861, 2003, 3463 };
        }

        public static long[] GetNotPrimeNumbers()
        {
            return new long[] { 0, 1, 4, 6, 8, 9, 10, 12, 14, 15, 2000 };
        }

        public static Dictionary<long,long> GetValueFirstPrimeDicionary()
        {
            var d = new Dictionary<long, long>();
            d.Add(1, 2);
            d.Add(2, 3);
            d.Add(3, 5);
            d.Add(4, 5);
            d.Add(5, 7);
            d.Add(6, 7);
            d.Add(11, 13);
            d.Add(12, 13);
            d.Add(823, 827);
            d.Add(825, 827);
            d.Add(3100, 3109);
            d.Add(3120, 3121);

            return d;
        }
    }
}
