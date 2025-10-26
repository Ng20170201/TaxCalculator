using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Entity;
using TaxCalc.Enums;

namespace TaxCalc.Repositories.Interfaces
{
    public interface ICustomRateRepository
    {
        CustomRate Add(Commodity commodity, Rate rate);
        bool Exist(Commodity commodity);
        Dictionary<Commodity, List<Rate>> GetAll();
        CustomRate GetByCommodity(Commodity commodity);
        CustomRate Update(Commodity commodity, Rate rate);
        bool UpdateLastRate(Commodity commodity, Rate value);
    }
}
