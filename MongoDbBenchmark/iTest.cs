using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbBenchmark
{
    internal interface iTest
    {
        void runTest(IMongoDatabase IntraMongoDB, int limit);
    }
}
