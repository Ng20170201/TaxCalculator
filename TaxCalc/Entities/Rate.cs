using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Entities;

namespace TaxCalc.Entity
{
    public class Rate
    {
        [BsonId] 
        public string Id { get; set; } = null!;

        public double Value { get; set; }
        public TimeInterval TimeInterval { get; set; }

        public Rate(double value, TimeInterval timeInterval)
        {
            Value = value;
            TimeInterval = timeInterval;
        }
    }
}
