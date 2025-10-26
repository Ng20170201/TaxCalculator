using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using TaxCalc.Enums;
using TaxCalc.Services.Interfaces;
using TaxCalc.Services;
using TaxCalc.Services.CacheServices;
using TaxCalc.TaxCalculatorClient;
using TaxCalc.Repositories.Interfaces;
using TaxCalc.Repositories;

var services = new ServiceCollection();

services.AddTransient<ITaxCalculator, TaxCalculator>();
services.AddTransient<ICustomRateService, CustomRateService>(); 
services.AddTransient<CustomRateCacheService>(); 

services.AddTransient<IRateRepository, RateRepository>();
services.AddTransient<IStandardRateRepository, StandardRateRepository>();
services.AddTransient<ICustomRateRepository, CustomRateRepository>();

var serviceProvider = services.BuildServiceProvider();

//my code for my manual tests -- delete it
var taxCalculator = serviceProvider.GetRequiredService<ITaxCalculator>();
var taxCalculator2 = serviceProvider.GetRequiredService<ITaxCalculator>();

taxCalculator.SetCustomTaxRate(Commodity.Transport, 0.11);
taxCalculator.SetCustomTaxRate(Commodity.Alcohol, 0.12);
taxCalculator.SetCustomTaxRate(Commodity.Transport, 0.13);
Thread.Sleep(5000);
taxCalculator.SetCustomTaxRate(Commodity.Transport, 0.14);

taxCalculator2.SetCustomTaxRate(Commodity.Transport, 0.21);
taxCalculator2.SetCustomTaxRate(Commodity.Alcohol, 0.22);
taxCalculator2.SetCustomTaxRate(Commodity.Transport, 0.23);
Thread.Sleep(5000);
taxCalculator2.SetCustomTaxRate(Commodity.Transport, 0.24);

Console.WriteLine("Transport standard rate " + taxCalculator.GetStandardTaxRate(Commodity.Transport));
Console.WriteLine("Transport Current rate " + taxCalculator.GetCurrentTaxRate(Commodity.Transport));
Console.WriteLine("Transport Before 5 sec rate " + taxCalculator.GetTaxRateForDateTime(Commodity.Transport, DateTime.UtcNow.AddSeconds(-5)));

Console.WriteLine("Transport standard rate " + taxCalculator2.GetStandardTaxRate(Commodity.Transport));
Console.WriteLine("Transport Current rate " + taxCalculator2.GetCurrentTaxRate(Commodity.Transport));
Console.WriteLine("Transport Before 5 sec rate " + taxCalculator2.GetTaxRateForDateTime(Commodity.Transport, DateTime.UtcNow.AddSeconds(-5)));



/*
 * NOTE:
 *
 *   All logic has been moved to CustomRateService to improve code readability and maintainability.
 *   
 *   A CustomRate object has been created.
 *   Each object contains the following data:
 *   Rate — the value of the rate
 *   TimeInterval — the time interval (start and end date/time), e.g.:  Rate | Time Interval
 *                                                                      --------------------
 *   																	0.01 | {12:00–13:00}
 *   																	0.02 | {14:00–15:00}
 *   																	0.01 | {15:00–16:00}	
 *   
 *   The list of CustomRate objects is associated with a specific Commodity.
 *   The list is always sorted by time, as new elements are added to the end.
 *   Using a SortedList<CustomRate> was considered, but it would slow down insert operations, while we already know the list is always sorted.
 *   (This logic can be changed in the future if a different ordering becomes necessary.)
 *   
 *   If the data were to be stored in a database, each CustomRate object could also include a reference to the Commodity it belongs to, but this is currently unnecessary.
 *   It was also possible to make the logic more complex by using a Dictionary structure, where only time interval set would be stored for each identical rate.
 *   However, this approach would not provide significant benefits because:
 *   The main advantage would be reduced memory usage, which matters only for in-memory storage.
 *   In the database, each time interval would still need to be stored along with its corresponding rate.
 *   Searching by rate would be faster, but in this case, iterating through all rates is still necessary, so no real optimization would be achieved.
 *   It would be good if we had a function to return all the intervals from the given rate
 *   
 *   Most common use cases
 *   
 *   * GET the current rate -- Optimized by storing all current values per commodity.
 *   					     (Recommendation: add caching for additional performance boost.)
 *   * GET the rate for a specific date/time -- Optimized using binary search, since the list is always sorted.
 *
 *   All exception messages can be moved to .resx files and read from there to ensure consistency and avoid differences between the messages.
 */

