using DataModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbBenchmark.Tests.FullModel
{
    internal class GetDataByModel : iTest
    {
        private static int[] Years = new int[] { 2021, 2022, 2023 };
        public void runTest(IMongoDatabase IntraMongoDB, int limit)
        {
            var theType = typeof(SalesData);

            Stopwatch sw = Stopwatch.StartNew(); ;
            var collection = IntraMongoDB.GetCollection<SalesData>("SalesData");
            sw.Stop();
            //WriteTraceLog(" GotCollection", new { sw.Elapsed.TotalMilliseconds });
            sw.Restart();

            var filter = Builders<SalesData>.Filter.Where(x => Years.Contains(x.IssueYear));
            var projection = Builders<SalesData>.Projection.Exclude("_id");

            sw.Stop();
            //WriteTraceLog(" FiltersAndProjection", new { sw.Elapsed.TotalMilliseconds });

            sw.Restart();
            var result = collection.Find(filter).Project<SalesData>(projection).Limit(limit).ToList();
            sw.Stop();

            Console.WriteLine($" {limit}: {sw.Elapsed.TotalMilliseconds}");
        }
    }
}
