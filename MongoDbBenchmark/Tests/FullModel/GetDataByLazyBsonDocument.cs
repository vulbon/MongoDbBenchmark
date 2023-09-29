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
    internal class GetDataByLazyBsonDocument : iTest
    {
        private static int[] Years = new int[] { 2021, 2022, 2023 };

        public void runTest(IMongoDatabase IntraMongoDB, int limit)
        {
            Stopwatch sw = Stopwatch.StartNew(); ;
            var collection = IntraMongoDB.GetCollection<LazyBsonDocument>("SalesData");
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
            var projection = Builders<LazyBsonDocument>.Projection.Exclude("_id");

            sw.Stop();
            //WriteTraceLog(" FiltersAndProjection", new { sw.Elapsed.TotalMilliseconds });

            sw.Restart();
            var bsonDoc = collection.Find(filter).Project<LazyBsonDocument>(projection).Limit(limit).ToList();

            sw.Stop();

            int count = bsonDoc.Count();
            Console.WriteLine($" {limit}: {sw.Elapsed.TotalMilliseconds}");
        }
    }
}
