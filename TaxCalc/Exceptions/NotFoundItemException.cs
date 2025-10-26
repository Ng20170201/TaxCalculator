using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalc.Exceptions
{
    //Exception for not found item
    public class NotFoundItemException : Exception
    {
        public NotFoundItemException(string item,string message = "")
            : base($"{item} was not found" + ", " +message)
        {
        }
    }
}
