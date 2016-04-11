using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace testMongo
{
    public class JournalEntry
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("PersistenceId")]
        public string PersistenceId { get; set; }

        [BsonElement("SequenceNr")]
        public long SequenceNr { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var db = new MongoDB.Driver
                .MongoClient("mongodb://localhost/?maxPoolSize=1000")
                .GetDatabase("mongo-async-tests");

            db.DropCollection("journal-sync");
            db.DropCollection("journal-async");
            db.DropCollection("journal-async-await");

            TestSync(db, 1000);
            TestAsync(db, 1000);
            TestAsyncAwait(db, 1000);

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

        private static void TestSync(IMongoDatabase db, int documents)
        {
            var collection = db.GetCollection<JournalEntry>("journal-sync");
            Console.Write("Inserting {0} documents, Sync version ", documents);

            var sw = new Stopwatch();
            sw.Start();
            Parallel.ForEach(Enumerable.Range(1, documents), i =>
            {
                var commit = new JournalEntry()
                {
                    PersistenceId = (i%10).ToString(),
                    SequenceNr = i
                };

                collection.InsertOne(commit);
            });
            sw.Stop();

            Console.WriteLine("- Elapsed {0}ms", sw.ElapsedMilliseconds);
        }

        private static void TestAsync(IMongoDatabase db, int documents)
        {
            var collection = db.GetCollection<JournalEntry>("journal-async");
            Console.Write("Inserting {0} documents, Async version ", documents);

            var sw = new Stopwatch();
            sw.Start();
            Parallel.ForEach(Enumerable.Range(1, documents), i =>
            {
                var commit = new JournalEntry()
                {
                    PersistenceId = (i % 10).ToString(),
                    SequenceNr = i
                };

                collection.InsertOneAsync(commit).Wait();
            });
            sw.Stop();

            Console.WriteLine("- Elapsed {0}ms", sw.ElapsedMilliseconds);
        }

        private static void TestAsyncAwait(IMongoDatabase db, int documents)
        {
            var collection = db.GetCollection<JournalEntry>("journal-async-await");
            Console.Write("Inserting {0} documents, Async/Await version ", documents);

            var sw = new Stopwatch();
            sw.Start();
            Parallel.ForEach(Enumerable.Range(1, documents), async i =>
            {
                var commit = new JournalEntry()
                {
                    PersistenceId = (i % 10).ToString(),
                    SequenceNr = i
                };

                while(true)
                {
                    try
                    {
                        // http://haacked.com/archive/2014/11/11/async-void-methods/
                        await collection.InsertOneAsync(commit);
                        break;
                    }
                    catch (MongoWaitQueueFullException ex)
                    {
                        Console.WriteLine("queue full at document {0}", i);
                        Thread.Sleep(2000);
                    }
                }
            });
            sw.Stop();

            Console.WriteLine("- Elapsed {0}ms", sw.ElapsedMilliseconds);
        }
    }
}
