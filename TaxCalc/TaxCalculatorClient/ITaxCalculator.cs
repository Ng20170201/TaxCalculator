using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Enums;

namespace TaxCalc.TaxCalculatorClient
{
    public interface ITaxCalculator
    {
        double GetStandardTaxRate(Commodity commodity);
        void SetCustomTaxRate(Commodity commodity, double rate);
        double GetTaxRateForDateTime(Commodity commodity, DateTime date);
        double GetCurrentTaxRate(Commodity commodity);
    }
}
