using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalc.Enums
{ 
    public enum Commodity
    {
        //PLEASE NOTE: THESE ARE THE ACTUAL TAX RATES THAT SHOULD APPLY, WE JUST GOT THEM FROM THE CLIENT!
        Default,            //25%
        Alcohol,            //25%
        Food,               //12%
        FoodServices,       //12%
        Literature,         //6%
        Transport,          //6%
        CulturalServices    //6%
    }
}
