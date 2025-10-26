using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Entities;
using TaxCalc.Enums;
using TaxCalc.Exceptions;
using TaxCalc.Services;
using TaxCalc.Services.CacheServices;
using TaxCalc.Services.Interfaces;

namespace TaxCalc.TaxCalculatorClient
{
    //Tax Calculator class
    public class TaxCalculator : ITaxCalculator
    {
        ICustomRateService _customRateService;
        public TaxCalculator(ICustomRateService customRateService)
        {
            _customRateService = customRateService;
        }

        /// <summary>
        /// Get standard tax rate
        /// </summary>
        /// <param name="commodity">Commodity</param>
        public double GetStandardTaxRate(Commodity commodity)
        {
            var standardRate = _customRateService.GetStandardRate(commodity);
            return standardRate;
        }

        /// <summary>
        /// Set custom tax rate
        /// </summary>
        /// <param name="commodity">Commodity</param>
        /// <param name="rate">Rate</param>
        public void SetCustomTaxRate(Commodity commodity, double rate)
        {
            ValidateCurrentRateCommodity(commodity, rate);
            TimeInterval currTimeInterval = new TimeInterval(DateTime.UtcNow, DateTime.MaxValue);
            _customRateService.AddNewCustomRate(commodity, rate, currTimeInterval);
        }

        /// <summary>
        /// Get tax rate for date
        /// </summary>
        /// <param name="commodity">Commodity</param>
        /// <param name="date">Date</param>
        public double GetTaxRateForDateTime(Commodity commodity, DateTime date)
        {
            double rate = GetStandardTaxRate(commodity);

            if (!_customRateService.ExistForCommodity(commodity))
                return rate;

            _customRateService.TryGetRateByDate(commodity, date, ref rate);
            return rate;
        }


        /// <summary>
        /// Get current tax rate
        /// </summary>
        /// <param name="commodity">Commodity</param>
        public double GetCurrentTaxRate(Commodity commodity)
        {
            double rate = GetStandardTaxRate(commodity);
            _customRateService.TryGetCurrentCustomRate(commodity, ref rate);

            return rate;
        }

        /// <summary>
        /// Validate if we try to set same rate for commodity if is rate active
        /// </summary>
        /// <param name="commodity">Commodity</param>
        /// <param name="rate">Rate</param>
        private void ValidateCurrentRateCommodity(Commodity commodity, double rate)
        {
            if (!_customRateService.ExistForCommodity(commodity))
                return;

            const double tolerance = 0.000001; //tolerance for double comparison, maybe Equals can be used or logic for saving on same number of decimal places
            var currentRate = GetCurrentTaxRate(commodity);

            if (Math.Abs(currentRate - rate) < tolerance)
                throw new ArgumentException($"Rate {rate} is already set for this commodity.");
        }
    }
}
