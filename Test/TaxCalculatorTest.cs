using TaxCalc.Entities;
using TaxCalc.Enums;
using TaxCalc.Repositories.Interfaces;
using TaxCalc.Repositories;
using TaxCalc.Services.CacheServices;
using TaxCalc.Services.Interfaces;
using TaxCalc.Services;
using TaxCalc.TaxCalculatorClient;
using Microsoft.Extensions.DependencyInjection;
using TaxCalc.Repositories.TestRepositories;

namespace Test
{
    //Test class for test our function in Calculator Test. I only added tests to verify our individual functions
    //and a few important edge cases. It might be a good idea to add a test that covers the entire flow as well.
    [TestClass]
    public class TaxCalculatorTest
    {
        private ITaxCalculator _taxCalculator;

        [TestInitialize]
        public void Setup()
        {
            var services = new ServiceCollection();

            services.AddTransient<ITaxCalculator, TaxCalculator>();
            services.AddTransient<ICustomRateService, CustomRateService>();
            services.AddTransient<CustomRateCacheService>();

            services.AddTransient<IRateRepository, RateRepository>();
            services.AddTransient<IStandardRateRepository, TestStandardRateRepository>();
            services.AddTransient<ICustomRateRepository, CustomRateRepository>();

            var serviceProvider = services.BuildServiceProvider();

            _taxCalculator = serviceProvider.GetRequiredService<ITaxCalculator>();
        }


        [TestMethod]
        public void GetStandardTaxRateForExistingItem()
        {
            Commodity testCommodity = Commodity.Food;
            double rate = _taxCalculator.GetStandardTaxRate(testCommodity);

            Assert.AreEqual(0.12, rate);
        }
        [TestMethod]
        public void GetStandardTaxRateForNonExistingItem()
        {
            Commodity testCommodity = Commodity.Transport;
            double rate = _taxCalculator.GetStandardTaxRate(testCommodity);

            Assert.AreEqual(0.25, rate);
        }
        [TestMethod]
        public void GetCurrentTaxRate()
        {
            Commodity testCommodity = Commodity.Transport;
            double rate = 0.1;

            _taxCalculator.SetCustomTaxRate(testCommodity, rate);
            var currentRate = _taxCalculator.GetCurrentTaxRate(testCommodity); //we should have get information from db, but in this case i will get current

            Assert.AreEqual(0.1, currentRate);
        }

        [TestMethod]
        public void SetSameRateLikeCurrent()
        {
            Commodity testCommodity = Commodity.Transport;
            double rate = 0.5;

            try
            {
                _taxCalculator.SetCustomTaxRate(testCommodity, rate);
                _taxCalculator.SetCustomTaxRate(testCommodity, rate);
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual($"Rate {rate} is already set for this commodity.", ex.Message);
            }
        }

        [TestMethod]
        public void SetCustomTaxRateIfRateNotExistForCommodity()
        {
            Commodity testCommodity = Commodity.Transport;
            double rate = 0.5;

            _taxCalculator.SetCustomTaxRate(testCommodity, rate);
            var currentRate = _taxCalculator.GetCurrentTaxRate(testCommodity); //we should have get information from db, but in this case i will get current

            Assert.AreEqual(rate, currentRate);
        }

        [TestMethod]
        public void SetCustomTaxRateIfRateExistForCommodity()
        {
            Commodity testCommodity = Commodity.Transport;
            double rate = 0.5;
            double rate2 = 0.3;

            _taxCalculator.SetCustomTaxRate(testCommodity, rate);
            _taxCalculator.SetCustomTaxRate(testCommodity, rate2);
            var currentRate = _taxCalculator.GetCurrentTaxRate(testCommodity);     //we should have get information from db, but in this case i will get current

            Assert.AreEqual(rate2, currentRate);
        }

        [TestMethod]
        public void GetTaxRateForDateTimeWhenRateIsNotExist()
        {
            DateTime date = DateTime.UtcNow.AddDays(-1);
            Commodity testCommodity = Commodity.Transport;

            double timeRate = _taxCalculator.GetTaxRateForDateTime(testCommodity, date);
            double standardRate = _taxCalculator.GetStandardTaxRate(testCommodity);

            Assert.AreEqual(timeRate, standardRate);
        }

        [TestMethod]
        public void GetTaxRateForDateTimeWhenRateIsRateExist()
        {
            Commodity testCommodity = Commodity.Transport;
            double rate1 = 0.1;
            double rate2 = 0.2;
            double rate3 = 0.3;

            _taxCalculator.SetCustomTaxRate(testCommodity, rate1);
            Thread.Sleep(2000);
            DateTime date1 = DateTime.UtcNow.AddSeconds(-1);
            double timeRate1 = _taxCalculator.GetTaxRateForDateTime(testCommodity, date1);

            Assert.AreEqual(0.1, timeRate1);

            _taxCalculator.SetCustomTaxRate(testCommodity, rate2);
            Thread.Sleep(2000);
            _taxCalculator.SetCustomTaxRate(testCommodity, rate3);
            Thread.Sleep(2000);
            DateTime date2 = DateTime.UtcNow.AddSeconds(-3);
            double timeRate2 = _taxCalculator.GetTaxRateForDateTime(testCommodity, date2);

            Assert.AreEqual(0.2, timeRate2);
        }
    }
}
