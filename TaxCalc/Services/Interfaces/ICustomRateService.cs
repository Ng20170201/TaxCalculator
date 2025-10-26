using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Entities;
using TaxCalc.Entity;
using TaxCalc.Enums;
using TaxCalc.Exceptions;

namespace TaxCalc.Services.Interfaces
{
    public interface ICustomRateService
    {
        public bool ExistForCommodity(Commodity commodity);
        public CustomRate AddNewCustomRate(Commodity commodity, double rateValue, TimeInterval currentTimeInterval);
        public bool TryGetCurrentCustomRate(Commodity commodity, ref double rate);
        public bool TryGetRateByDate(Commodity commodity, DateTime date, ref double rate);
        double GetStandardRate(Commodity commodity);
    }
}
