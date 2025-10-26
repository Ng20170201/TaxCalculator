using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalc.Entities
{
    //This class defines time interval from start to end
    public class TimeInterval
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeInterval(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
                throw new ArgumentException("Start Date must be before End Date");
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
