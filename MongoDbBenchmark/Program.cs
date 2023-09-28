using DataModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDbBenchmark
{
    internal class Program
    {
        private static MongoClient MGClient = new MongoClient("mongodb://localhost:27017");
        private static IMongoDatabase IntraMongoDB = MGClient.GetDatabase("Benchmark");
        private static int[] Years = new int[] { 2021, 2022, 2023 };

        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                GetDataByRawBsonDocument();

                GetDataByModel();
            }).Wait();

            Console.ReadLine();
        }

        private static void GetDataByRawBsonDocument()
        {
            Stopwatch sw = Stopwatch.StartNew(); ;
            var collection = IntraMongoDB.GetCollection<RawBsonDocument>("SalesData");
            sw.Stop();
            WriteTraceLog("GotCollection", new { sw.Elapsed.TotalMilliseconds });
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
            WriteTraceLog("FiltersAndProjection", new { sw.Elapsed.TotalMilliseconds });

            sw.Restart();
            var rawBsonDoc = collection.Find(filter).Project<RawBsonDocument>(projection).ToList();

            sw.Stop();

            int count = rawBsonDoc.Count();
            WriteTraceLog("GotData", new { sw.Elapsed.TotalMilliseconds, count });
        }

        private static void GetDataByModel()
        {
            Stopwatch sw = Stopwatch.StartNew(); ;
            var collection = IntraMongoDB.GetCollection<SalesData>("SalesData");
            sw.Stop();
            WriteTraceLog("GotCollection", new { sw.Elapsed.TotalMilliseconds });
            sw.Restart();

            var filter = Builders<SalesData>.Filter.Where(x => Years.Contains(x.IssueYear));
            var projection = Builders<SalesData>.Projection.Exclude("_id");

            sw.Stop();
            WriteTraceLog("FiltersAndProjection", new { sw.Elapsed.TotalMilliseconds });

            sw.Restart();
            var result = collection.Find(filter).Project<SalesData>(projection).ToList();
            sw.Stop();

            WriteTraceLog("GotData", new { sw.Elapsed.TotalMilliseconds, result.Count });
        }

        private static void WriteTraceLog(string title, object obj)
        {
            string msg = DateTime.Now.ToString("HH:mm:ss.fff") + " | " + "[" + title + "]" + Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            Console.WriteLine(msg);
        }

    }
}
