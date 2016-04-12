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
        private static ParallelOptions options = new ParallelOptions()
        {
            MaxDegreeOfParallelism = 8
        };
        static void Main(string[] args)
        {
            var client = new MongoClient("mongodb://localhost/");

            var db = client.GetDatabase("mongo-async-tests");

            Console.WriteLine("ProcessorCount           {0}", Environment.ProcessorCount);
            Console.WriteLine("MaxDegreeOfParallelism   {0}", options.MaxDegreeOfParallelism);
            Console.WriteLine("Mongo Pool Size          {0}", client.Settings.MaxConnectionPoolSize);

            foreach (var iterations in new[] { 100,1000, 10000})
            {
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine("Iterations               {0}", iterations);
                Console.WriteLine("---------------------------------------------------");

                for (int c = 1; c <= 5; c++)
                {
                    Console.WriteLine("Run #{0}", c);
                    db.DropCollection("journal-sync");
                    db.DropCollection("journal-async");
                    db.DropCollection("journal-async-await");

                    TestSync(db, iterations);
                    TestAsync(db, iterations);
                    TestAsyncAwait(db, iterations);
                }
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

        private static void TestSync(IMongoDatabase db, int documents)
        {
            var collection = db.GetCollection<JournalEntry>("journal-sync");
            Console.Write("Sync version ");

            var sw = new Stopwatch();
            sw.Start();
            Parallel.ForEach(Enumerable.Range(1, documents), options, i =>
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
            Console.Write("Async version ");

            var sw = new Stopwatch();
            sw.Start();
            Parallel.ForEach(Enumerable.Range(1, documents), options, i =>
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
            Console.WriteLine("Async/Await version");

            bool showException = true;
            var sw = new Stopwatch();
            sw.Start();
            Parallel.ForEach(Enumerable.Range(1, documents), options, async i =>
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
                        if (showException)
                        {
                            Console.WriteLine("  queue full at document {0}, thread {1}", i, Thread.CurrentThread.ManagedThreadId);
                            showException = false;
                        }
                        Thread.Sleep(100);
                    }
                }
            });
            sw.Stop();

            Console.WriteLine("- Elapsed {0}ms", sw.ElapsedMilliseconds);
        }
    }
}
