using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Enums;

namespace TaxCalc.Entity
{
    public class CustomRate
    {
        [BsonId]
        public string Id { get; set; } = null!;
        public Commodity Commodity { get; set; }
        public List<Rate> Rates { get; set; } = new();
        public CustomRate() { }

        public CustomRate(Commodity commodity)
        {
            Commodity = commodity;
        }
        public CustomRate(Commodity commodity, List<Rate> rates)
        {
            Commodity = commodity;
            Rates = rates;
        }
    }
}
