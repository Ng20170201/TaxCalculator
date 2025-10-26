using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Entity;

namespace TaxCalc.Repositories.Interfaces
{
    public interface IRateRepository
    {
        Rate Add(Rate rate);
        bool Update(Rate rate);
    }
}
