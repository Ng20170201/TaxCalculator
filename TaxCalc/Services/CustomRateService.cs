using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Entities;
using TaxCalc.Entity;
using TaxCalc.Enums;
using TaxCalc.Exceptions;
using TaxCalc.Repositories.Interfaces;
using TaxCalc.Services.CacheServices;
using TaxCalc.Services.Interfaces;

namespace TaxCalc.Services
{
    public class CustomRateService : ICustomRateService
    {
        private readonly ICustomRateRepository _customRateRepo;
        private readonly CustomRateCacheService _customRateCacheService;
        private readonly IRateRepository _rateRepo;
        private readonly IStandardRateRepository _standardRateRepo;
        public CustomRateService(ICustomRateRepository customRateRepository, IRateRepository rateRepository, CustomRateCacheService customRateCacheService, IStandardRateRepository standardRateRepository)
        {
            _customRateRepo = customRateRepository;
            _rateRepo = rateRepository;
            _customRateCacheService = customRateCacheService;
            _standardRateRepo = standardRateRepository;
        }

        /// <summary>
        /// Check if any Custom Rate exists for commodity
        /// </summary>
        /// <param name="commodity">Commodity</param>
        public bool ExistForCommodity(Commodity commodity)
        {
            return _customRateRepo.Exist(commodity);
        }

        /// <summary>
        /// Add new custom rate
        /// </summary>
        /// <param name="commodity">Commodity</param>
        /// <param name="rateValue">Rate</param>
        /// <param name="currentTimeInterval">Current time interval</param>
        public CustomRate AddNewCustomRate(Commodity commodity, double rateValue, TimeInterval currentTimeInterval)
        {
            Rate rate = new Rate(rateValue, currentTimeInterval);

            //Add in transaction
            _rateRepo.Add(rate);
            UpdateCurrentCustomRate(commodity, rate);

            if (!ExistForCommodity(commodity))
            {
                return _customRateRepo.Add(commodity, rate);
            }

            FinishActiveInterval(commodity, currentTimeInterval.StartDate);
            return _customRateRepo.Update(commodity, rate);
            //end of trasaction 
        }

        /// <summary>
        /// Finish active Interval
        /// </summary>
        /// <param name="commodity">Commodity</param>
        /// <param name="endDate">End Date</param>
        private void FinishActiveInterval(Commodity commodity, DateTime endDate)
        {
            CustomRate customRate = _customRateRepo.GetByCommodity(commodity);
            Rate rate = customRate.Rates.Last();
            rate.TimeInterval.EndDate = endDate;
            //_rateRepo.Update(rate); if we implement db
            _customRateRepo.UpdateLastRate(commodity, rate); //null because we already updated rate list

        }

        /// <summary>
        /// Try to Get current custom rate if exist
        /// </summary>
        /// <param name="commodity">Commodity</param>
        /// <param name="rate">Rate</param>
        public bool TryGetCurrentCustomRate(Commodity commodity, ref double rate)
        {
            _customRateCacheService.TryGetRateByCommodity(commodity, ref rate); 
            return false;
        }

        /// <summary>
        /// Try to Get Custom rate by date
        /// </summary>
        /// <param name="commodity">Commodity</param>
        /// <param name="date">Date</param>
        /// <param name="rate">Rate</param>
        public bool TryGetRateByDate(Commodity commodity, DateTime date, ref double rate)
        {
            if (!ExistForCommodity(commodity))
                throw new NotFoundItemException(commodity.ToString());

            List<Rate> rates = _customRateRepo.GetByCommodity(commodity).Rates;
            Rate? rateForDate = BinarySearchTimeRateByDate(rates, date);  //List is always sorted because we add custom flow with latest date

            if (rateForDate == null)
            {
                return false;
            }
            rate = rateForDate.Value;

            return true;
        }

        #region private methods
        /// <summary>
        /// Update current custom rate
        /// </summary>
        /// <param name="commodity">Commodity</param>
        /// <param name="rate">Rate</param>
        private void UpdateCurrentCustomRate(Commodity commodity, Rate rate)
        {
            _customRateCacheService.Update(commodity, rate.Value);
        }
        /// <summary>
        /// Binary search for custom rate by date
        /// Condition is that list is sorted by start date/end date and there is no overlapping intervals. 
        /// IT IS ALWAYS TRUE BECAUSE OF LOGIC FOR ADDING NEW CUSTOM RATE
        /// </summary>
        /// <param name="rates">Rates for commodity</param>
        /// <param name="date">Date</param>
        private Rate? BinarySearchTimeRateByDate(List<Rate> rates, DateTime date)
        {
            int left = 0;
            int right = rates.Count - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                var interval = rates[mid].TimeInterval;

                if (date < interval.StartDate)
                {
                    right = mid - 1;
                }
                else if (date > interval.EndDate)
                {
                    left = mid + 1;
                }
                else
                {
                    return rates[mid];
                }
            }

            return null;
        }

        /// <summary>
        /// Get Standard Rate for Commodity
        /// </summary>
        /// <param name="commodity">Commodity</param>
        public double GetStandardRate(Commodity commodity)
        {
            StandardRate standardRate = _standardRateRepo.GetByCommodity(commodity);

            if (standardRate == null)
            {
                StandardRate defaultRate = _standardRateRepo.GetDefault();
                if (defaultRate == null)
                    throw new NotFoundItemException("Default Rate");
                return defaultRate.Rate;

            }
                
            return _standardRateRepo.GetByCommodity(commodity).Rate;
        }
        #endregion
    }
}

