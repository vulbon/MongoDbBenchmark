using Bogus;
using Bogus.DataSets;
using DataModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GenrateFakeData
{
    internal class Program
    {
        private static MongoClient MGClient = new MongoClient("mongodb://localhost:27017");
        private static IMongoDatabase IntraMongoDB = MGClient.GetDatabase("Benchmark");
        private static Random _random = new Random();
        static void Main(string[] args)
        {

            var collection = IntraMongoDB.GetCollection<BsonDocument>("SalesData");

            var salesData = new Faker<SalesData>()
            .RuleFor(x => x.CompanyName, f => f.Company.CompanyName())
            .RuleFor(x => x.CompanyId, f => f.Random.String2(4, "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"))
            .RuleFor(x => x.Currency, f => f.Finance.Currency().Code)
            .RuleFor(x => x.LocalCurrency, f => f.Finance.Currency().Code)

            .RuleFor(x => x.SalesCost, f => f.Random.Decimal(0, 2000))
            .RuleFor(x => x.SalesCost_Spec, f => f.Random.Decimal(0, 2000))
            .RuleFor(x => x.GrossProfit_Spec, f => f.Random.Decimal(0, 2000))
            .RuleFor(x => x.SalesCost_TMC_RMB, f => f.Random.Decimal(0, 2000))
            .RuleFor(x => x.SalesCost_TMC_Spec, f => f.Random.Decimal(0, 2000))
            .RuleFor(x => x.GrossProfit_TMC_Spec, f => f.Random.Decimal(0, 2000))
            .RuleFor(x => x.SalesCost_TMF_NTD, f => f.Random.Decimal(0, 2000))
            .RuleFor(x => x.SalesCost_TMF_Spec, f => f.Random.Decimal(0, 2000))

            .RuleFor(x => x.SalesCost_MA_TMC_RMB, f => f.Random.Decimal(0, 2000))
            .RuleFor(x => x.SalesCost_LA_TMC_RMB, f => f.Random.Decimal(0, 2000))
            .RuleFor(x => x.SalesCost_OV_TMC_RMB, f => f.Random.Decimal(0, 2000))
            .RuleFor(x => x.SalesCost_CO_TMC_RMB, f => f.Random.Decimal(0, 2000))
            .RuleFor(x => x.SalesCost_MA_TMF_NTD, f => f.Random.Decimal(0, 2000))
            .RuleFor(x => x.SalesCost_LA_TMF_NTD, f => f.Random.Decimal(0, 2000))
            .RuleFor(x => x.SalesCost_OV_TMF_NTD, f => f.Random.Decimal(0, 2000))
            .RuleFor(x => x.SalesCost_CO_TMF_NTD, f => f.Random.Decimal(0, 2000))

            .RuleFor(x => x.SalesQty, f => f.Random.Int(0, 100))
            .RuleFor(x => x.SalesAmount, f => f.Random.Double(0, 2000))
            .RuleFor(x => x.SalesAmount_Local, f => f.Random.Double(0, 2000))
            .RuleFor(x => x.SalesAmount_Spec, f => f.Random.Double(0, 2000))
            .RuleFor(x => x.SalesCostWoReturn, f => f.Random.Double(0, 2000))
            .RuleFor(x => x.SalesCostWoReturn_Spec, f => f.Random.Double(0, 2000))
            .RuleFor(x => x.GrossProfitWoReturn_Spec, f => f.Random.Double(0, 2000))
            .RuleFor(x => x.SalesCostWoReturn_TMC_RMB, f => f.Random.Double(0, 2000))
            .RuleFor(x => x.SalesCostWoReturn_TMC_Spec, f => f.Random.Double(0, 2000))
            .RuleFor(x => x.GrossProfitWoReturn_TMC_Spec, f => f.Random.Double(0, 2000));

            var faker = new Faker();

            Stopwatch theTimer = new Stopwatch();

            for (int i = 0; i < 100; i++)
            {
                theTimer.Start();
                List<BsonDocument> bsonDocs = new List<BsonDocument>();
                foreach (var s in salesData.Generate(100))
                {
                    var issueDate = GenIssueDate(faker);
                    s.IssueDate = issueDate.IssueDate;
                    s.IssueYear = issueDate.IssueYear;
                    s.IssueMonth = issueDate.IssueMonth;
                    s.IssueQuater = issueDate.IssueQuater;

                    var forecast = GenSalesCost(faker);
                    s.ForecastQty = forecast.Qty;
                    s.ForecastUnitCost = forecast.UnitCost;
                    s.ForecastAmount = forecast.Amount;
                    s.ForecastAmountLocalCurrency = forecast.AmountLocalCurrency;

                    var order = GenSalesCost(faker);
                    s.OrderQty = order.Qty;
                    s.OrderUnitCost = order.UnitCost;
                    s.OrderAmount = order.Amount;
                    s.OrderAmountLocalCurrency = order.AmountLocalCurrency;

                    var ship = GenSalesCost(faker);
                    s.ShipQty = ship.Qty;
                    s.ShipUnitCost = ship.UnitCost;
                    s.ShipAmount = ship.Amount;
                    s.ShipAmountLocalCurrency = ship.AmountLocalCurrency;

                    bsonDocs.Add(s.ToBsonDocument());
                }
                theTimer.Stop();
                Console.WriteLine($"Procesed 100 in {theTimer.ElapsedMilliseconds} ms");
                theTimer.Restart();
                collection.InsertMany(bsonDocs);
            }
        }

        private static (DateTime IssueDate, int IssueYear, string IssueMonth, string IssueQuater) GenIssueDate(Faker faker)
        {
            var issueDate = faker.Date.Between(new DateTime(2012, 1, 1), DateTime.Now);

            int issueYear = issueDate.Year;
            string issueMonth = issueDate.Month.ToString().PadLeft(2, '0');
            string issueQuater = ConvertMonthToQuater(issueDate.Month);

            return (issueDate, issueYear, issueMonth, issueQuater);
        }

        private static (int Qty, decimal UnitCost, decimal Amount, decimal AmountLocalCurrency) GenSalesCost(Faker faker)
        {
            var qty = faker.Random.Int(0, 2000);
            var unitCost = faker.Random.Decimal(0, 2000);
            var amount = qty * unitCost;
            var amountLocalCurrency = amount * faker.Random.Decimal(0.5m, 35m);

            return (qty, unitCost, amount, amountLocalCurrency);
        }

        private static (DateTime IssueDate, int IssueYear, string IssueMonth, string IssueQuater) GenIssueDate2()
        {
            int range = (DateTime.Now - new DateTime(2012, 1, 1)).Days;
            var issueDate = DateTime.Now.AddDays(_random.Next(range));

            int issueYear = issueDate.Year;
            string issueMonth = issueDate.Month.ToString().PadLeft(2, '0');
            string issueQuater = ConvertMonthToQuater(issueDate.Month);

            return (issueDate, issueYear, issueMonth, issueQuater);
        }

        private static (int Qty, decimal UnitCost, decimal Amount, decimal AmountLocalCurrency) GenSalesCost2()
        {
            var qty = _random.Next(2000);
            var unitCost = _random.Next(2000);
            var amount = qty * unitCost;
            var amountLocalCurrency = amount * (0.5m + (decimal)_random.NextDouble() * 30m);

            return (qty, unitCost, amount, amountLocalCurrency);
        }

        private static string ConvertMonthToQuater(int month)
        {
            switch (month)
            {
                case 1:
                case 2:
                case 3:
                    return "Q1";
                case 4:
                case 5:
                case 6:
                    return "Q2";
                case 7:
                case 8:
                case 9:
                    return "Q3";
                case 10:
                case 11:
                case 12:
                    return "Q4";
                default:
                    throw new ArgumentOutOfRangeException("Month must be between 1 and 12.");
            }
        }
    }
}
