using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeNumber.Model
{
    public class BasePrimeCheckResult
    {
        public bool? IsPrime { get; set; } = false;
        public long MinDivisionMaxValue { get; set; } = -1;
        public long MinDivisionMinValue { get; set; } = -1;
    }
}
