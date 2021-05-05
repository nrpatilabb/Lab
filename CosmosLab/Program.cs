using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace CosmosLab
{
    class Program
    {
        static void Main(string[] args)
        {
            DB.Insert().Wait();
        }
    }

    class DB
    {
        public static async Task Insert()
        {
            var db = MongoConnect.GetDB();
            var collections = db.GetCollection<LabBsonObject>("SceTypes");
            var data = new LabBsonObject
            {
                _id = Guid.NewGuid().ToString(),
                Name = "Nikhil",
                Address = "Kolhapur"
            };
            await collections.InsertOneAsync(data);
        }
    }

    class MongoConnect
    {
        private static readonly string ConnectionString =  @"mongodb://my-lab-db:Zo3Jr3FopBCTPde1bYN9uKls666PwDBrPp1UephJnhOUFgadinNR1RNa4TQ7ZeZucqh6FfeKY17xi51RnbK1NQ==@my-lab-db.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@my-lab-db@";
        private static readonly string DBName = "SceLab";

        public static IMongoDatabase GetDB()
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(ConnectionString)
            );
            settings.SslSettings =
                  new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);
            return mongoClient.GetDatabase(DBName);
        }
    }
}
