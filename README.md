# MongoDbSyncAsyncTests

Test results on my dev machine (Dell T 3500) - Debug

    Inserting 1000 documents, Sync version - Elapsed 200ms
    Inserting 1000 documents, Async version - Elapsed 10597ms
    Inserting 1000 documents, Async/Await version - Elapsed 351ms
    
Note: first round is subject to 50ms delay (due to first connection?).
