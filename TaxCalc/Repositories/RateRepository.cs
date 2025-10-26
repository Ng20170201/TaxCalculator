using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Entity;
using TaxCalc.Repositories.Interfaces;

namespace TaxCalc.Repositories
{
    //there should be an implementation for mongoDb collections. But since we don't use that right now, we'll just use in-memory 
    public class RateRepository : IRateRepository
    {
        private readonly List<Rate> _rates;
        public RateRepository()
        {
            _rates = new List<Rate>();
        }

        public Rate Add(Rate rate)
        {
            _rates.Add(rate);

            return rate; 
        }

        public bool Update(Rate rate)
        {
            Rate updatedRate = _rates.FirstOrDefault(r => r.Id == rate.Id);
            updatedRate.TimeInterval.EndDate = rate.TimeInterval.EndDate;
            return true;
        }
    }
}
