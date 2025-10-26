using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Enums;

namespace TaxCalc.Entities
{
    //Better if there is some object for representing Standard rate, because we get this information from db or from api. So it is more clear for next steps.
    public class StandardRate
    {
        [BsonId]
        public string Id { get; set; } = null!;

        public Commodity Commodity { get; set; }
        public double Rate { get; set; }
        public StandardRate(Commodity commodity, double rate)
        {
            Commodity = commodity;
            Rate = rate;
        }
    }

}
