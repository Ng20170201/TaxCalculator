using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Entities;
using TaxCalc.Entity;
using TaxCalc.Enums;
using TaxCalc.Repositories.Interfaces;

namespace TaxCalc.Repositories
{
    public class StandardRateRepository : IStandardRateRepository
    {
        //better because we will mostly be pulling that data from some tables or from APIs
        //I assume Commodity.Default is default standard rate if we don't have specific standard rate for commodity
        List<StandardRate> _commodityItems = new List<StandardRate>()
        {
         new StandardRate(Commodity.Default,0.25),
         new StandardRate(Commodity.Alcohol,0.25),
         new StandardRate(Commodity.Food,0.12),
         new StandardRate(Commodity.FoodServices,0.12),
         new StandardRate(Commodity.Literature,0.06),
         new StandardRate(Commodity.Transport,0.06),
         new StandardRate(Commodity.CulturalServices,0.06)
        };

        public StandardRateRepository()
        {
        }

        public StandardRate GetByCommodity(Commodity commodity)
        {
            StandardRate standardRate = _commodityItems.FirstOrDefault(x => x.Commodity == commodity);
            if(standardRate == null) return null;

            return standardRate;
        }

        public StandardRate GetDefault()
        {
            StandardRate standardRate = _commodityItems.FirstOrDefault(x => x.Commodity == Commodity.Default);
            if (standardRate == null) return null;

            return standardRate;
        }
    }
}
