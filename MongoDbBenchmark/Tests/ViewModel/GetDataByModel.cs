using DataModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbBenchmark.Tests.ViewModel
{
    internal class GetDataByModel : iTest
    {
        private static int[] Years = new int[] { 2021, 2022, 2023 };
        public void runTest(IMongoDatabase IntraMongoDB, int limit)
        {

            Stopwatch sw = Stopwatch.StartNew(); ;
            var collection = IntraMongoDB.GetCollection<SalesDataViewModel>("SalesData");
            sw.Stop();
            //WriteTraceLog(" GotCollection", new { sw.Elapsed.TotalMilliseconds });
            sw.Restart();

            var filter = Builders<SalesDataViewModel>.Filter.Where(x => Years.Contains(x.IssueYear));
            var projection = Builders<SalesDataViewModel>.Projection.Include("CompanyName")
                .Include("CompanyId")
                .Include("Currency")
                .Include("LocalCurrency")
                .Include("IssueDate")
                .Include("IssueYear")
                .Include("IssueMonth")
                .Include("IssueQuater")
                .Include("ForecastQty")
                .Include("ForecastUnitCost")
                .Include("ForecastAmount")
                .Include("ForecastAmountLocalCurrency")
                .Include("OrderQty")
                .Include("OrderUnitCost")
                .Include("OrderAmount")
                .Include("OrderAmountLocalCurrency").Exclude("_id");
            sw.Stop();
            //WriteTraceLog(" FiltersAndProjection", new { sw.Elapsed.TotalMilliseconds });

            sw.Restart();
            var result = collection.Find(filter).Project<SalesDataViewModel>(projection).Limit(limit).ToList();
            sw.Stop();

            Console.WriteLine($" {limit}: {sw.Elapsed.TotalMilliseconds}");
        }
    }
}
