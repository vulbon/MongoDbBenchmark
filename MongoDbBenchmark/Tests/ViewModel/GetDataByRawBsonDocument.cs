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
    internal class GetDataByRawBsonDocument : iTest
    {
        private static int[] Years = new int[] { 2021, 2022, 2023 };
        public void runTest(IMongoDatabase IntraMongoDB, int limit)
        {
            Stopwatch sw = Stopwatch.StartNew(); ;
            var collection = IntraMongoDB.GetCollection<RawBsonDocument>("SalesData");
            sw.Stop();
            //WriteTraceLog(" GotCollection", new { sw.Elapsed.TotalMilliseconds });
            sw.Restart();

            BsonDocument filter = new BsonDocument
            {
                {
                    "IssueYear",
                    new BsonDocument{{"$in", new BsonArray(Years)} }
                }
            };
            var projection = Builders<RawBsonDocument>.Projection.Exclude("_id");

            sw.Stop();
            //WriteTraceLog(" FiltersAndProjection", new { sw.Elapsed.TotalMilliseconds });

            sw.Restart();
            var rawBsonDoc = collection.Find(filter).Project<RawBsonDocument>(projection).Limit(limit).ToList();

            sw.Stop();

            int count = rawBsonDoc.Count();
            Console.WriteLine($" {limit}: {sw.Elapsed.TotalMilliseconds}");
        }
    }
}
