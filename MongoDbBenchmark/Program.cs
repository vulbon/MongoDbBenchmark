using DataModel;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbBenchmark.Tests;
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
            //iTest getDataByRawBsonDocument = new MongoDbBenchmark.Tests.ViewModel.GetDataByRawBsonDocument();
            //iTest getDataByBsonDocument = new MongoDbBenchmark.Tests.ViewModel.GetDataByBsonDocument();
            iTest getDataByLazyBsonDocument = new MongoDbBenchmark.Tests.FullModel.GetDataByLazyBsonDocument();
            //iTest getDataByModel = new MongoDbBenchmark.Tests.ViewModel.GetDataByModel();

            runTest(getDataByLazyBsonDocument);
            //runTest(getDataByRawBsonDocument);
            //runTest(getDataByBsonDocument);
            //runTest(getDataByModel);

            Console.ReadLine();

        }

        private static void runTest (iTest test)
        {
            Console.WriteLine(test.GetType().FullName);
            int dataSize = 1000;

            for (int i = 0; i < 100; i++)
            {
                test.runTest(IntraMongoDB, dataSize);
                dataSize += 1000;
            }
            Console.WriteLine("**************************************************");
        }


        private static void WriteTraceLog(string title, object obj)
        {
            string msg = DateTime.Now.ToString("HH:mm:ss.fff") + " | " + "[" + title + "]" + Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            Console.WriteLine(msg);
        }

    }
}
