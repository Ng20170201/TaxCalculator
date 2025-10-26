using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Entity;
using TaxCalc.Enums;
using TaxCalc.Repositories.Interfaces;

namespace TaxCalc.Repositories
{    
    //there should be an implementation for mongoDb collections. But since we don't use that right now, we'll just use in-memory 
    public class CustomRateRepository : ICustomRateRepository
    {
        private readonly Dictionary<Commodity,List<Rate>> _customRates = new Dictionary<Commodity, List<Rate>>();

        public CustomRate Add(Commodity commodity, Rate rate)
        {
            _customRates[commodity] = new List<Rate>() { rate };
            return new CustomRate(commodity, _customRates[commodity]);
        }

        public bool Exist(Commodity commodity)
        {
            if (!_customRates.ContainsKey(commodity)) return false;
            return true;
        }

        public Dictionary<Commodity, List<Rate>> GetAll()
        {
            return _customRates;
        }

        public CustomRate GetByCommodity(Commodity commodity)
        {
            return new CustomRate(commodity, _customRates[commodity]);
        }

        public CustomRate Update(Commodity commodity, Rate rate)
        {
            _customRates[commodity].Add(rate);
            return new CustomRate(commodity, _customRates[commodity]);
        }
    }
}
