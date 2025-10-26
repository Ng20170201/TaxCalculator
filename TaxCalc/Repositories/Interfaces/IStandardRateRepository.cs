using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Entities;
using TaxCalc.Enums;

namespace TaxCalc.Repositories.Interfaces
{
    public interface IStandardRateRepository
    {
        StandardRate GetByCommodity(Commodity commodity);
        StandardRate GetDefault();
    }
}
