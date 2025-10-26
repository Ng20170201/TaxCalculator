using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Entity;
using TaxCalc.Enums;
using TaxCalc.Repositories.Interfaces;

namespace TaxCalc.Services.CacheServices
{
    public class CustomRateCacheService
    {
        //we can add it, but this is not tranisent, we will use Dictionary as cache
        //private readonly IMemoryCache _cache;
        Dictionary<Commodity, double> currentCustomRates = new Dictionary<Commodity, double>();

        private readonly ICustomRateRepository _customRateRepository;

        public CustomRateCacheService(ICustomRateRepository customRateRepository)
        {
            _customRateRepository = customRateRepository;
            LoadCurrentCustomRates();
        }

        /// <summary>
        /// Try Get Current Rate For Commodity from cache
        /// </summary>
        /// <param name="commodity">Commodity</param>
        /// <param name="rate">Rate</param>
        internal void TryGetRateByCommodity(Commodity commodity, ref double rate)
        {
            if (currentCustomRates == null || !currentCustomRates.ContainsKey(commodity)) return;

            rate = currentCustomRates[commodity];
        }

        /// <summary>
        /// Update Current Rate in cache
        /// </summary>
        /// <param name="commodity">Commodity</param>
        /// <param name="value">Rate</param>
        internal void Update(Commodity commodity, double value)
        {
            if (currentCustomRates == null) return;

            if (currentCustomRates.ContainsKey(commodity)) currentCustomRates[commodity] = value;
            else currentCustomRates.Add(commodity, value);
        }

        /// <summary>
        /// Load current custom rates
        /// </summary>
        public void LoadCurrentCustomRates()
        {
            Dictionary<Commodity, List<Rate>> currentCustomRate = _customRateRepository.GetAll();
            if (currentCustomRate == null) return;

            foreach (var item in currentCustomRate)
            {
                Commodity commodity = item.Key;
                List<Rate> rates = item.Value;
                Rate currentRate = rates.Last();
                Update(commodity, currentRate.Value);
            }
        }

    }
}
