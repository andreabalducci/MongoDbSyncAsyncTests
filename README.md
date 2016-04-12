# MongoDbSyncAsyncTests

Test results on my dev machine (Dell T 3500) - Debug

	ProcessorCount           4
	MaxDegreeOfParallelism   8
	Mongo Pool Size          100
	---------------------------------------------------
	Iterations               100
	---------------------------------------------------
	Run #1
	Sync version - Elapsed 106ms
	Async version - Elapsed 672ms
	Async/Await version
	- Elapsed 34ms
	Run #2
	Sync version - Elapsed 77ms
	Async version - Elapsed 1842ms
	Async/Await version
	- Elapsed 16ms
	Run #3
	Sync version - Elapsed 27ms
	Async version - Elapsed 937ms
	Async/Await version
	- Elapsed 9ms
	Run #4
	Sync version - Elapsed 27ms
	Async version - Elapsed 31ms
	Async/Await version
	- Elapsed 5ms
	Run #5
	Sync version - Elapsed 31ms
	Async version - Elapsed 32ms
	Async/Await version
	- Elapsed 8ms
	---------------------------------------------------
	Iterations               1000
	---------------------------------------------------
	Run #1
	Sync version - Elapsed 129ms
	Async version - Elapsed 167ms
	Async/Await version
	  queue full at document 562, thread 5
	  queue full at document 672, thread 15
	  queue full at document 100, thread 1
	- Elapsed 137ms
	Run #2
	Sync version - Elapsed 130ms
	Async version - Elapsed 161ms
	Async/Await version
	  queue full at document 631, thread 4
	  queue full at document 639, thread 26
	  queue full at document 604, thread 31
	- Elapsed 206ms
	Run #3
	Sync version - Elapsed 132ms
	Async version - Elapsed 163ms
	Async/Await version
	  queue full at document 655, thread 26
	  queue full at document 630, thread 31
	  queue full at document 595, thread 18
	- Elapsed 177ms
	Run #4
	Sync version - Elapsed 138ms
	Async version - Elapsed 252ms
	Async/Await version
	  queue full at document 623, thread 34
	  queue full at document 651, thread 36
	  queue full at document 254, thread 23
	- Elapsed 214ms
	Run #5
	Sync version - Elapsed 152ms
	Async version - Elapsed 190ms
	Async/Await version
	  queue full at document 568, thread 1
	  queue full at document 632, thread 12
	  queue full at document 646, thread 20
	  queue full at document 611, thread 31
	- Elapsed 193ms
	---------------------------------------------------
	Iterations               10000
	---------------------------------------------------
	Run #1
	Sync version - Elapsed 1118ms
	Async version - Elapsed 1564ms
	Async/Await version
	  queue full at document 686, thread 1
	  queue full at document 271, thread 37
	  queue full at document 678, thread 17
	- Elapsed 2592ms
	Run #2
	Sync version - Elapsed 1083ms
	Async version - Elapsed 1595ms
	Async/Await version
	  queue full at document 600, thread 25
	  queue full at document 639, thread 18
	- Elapsed 2374ms
	Run #3
	Sync version - Elapsed 1153ms
	Async version - Elapsed 1925ms
	Async/Await version
	  queue full at document 643, thread 34
	  queue full at document 706, thread 27
	  queue full at document 681, thread 1
	- Elapsed 2485ms
	Run #4
	Sync version - Elapsed 1116ms
	Async version - Elapsed 1543ms
	Async/Await version
	  queue full at document 584, thread 33
	  queue full at document 654, thread 22
	  queue full at document 614, thread 1
	  queue full at document 629, thread 17
	- Elapsed 3112ms
	Run #5
	Sync version - Elapsed 1064ms
	Async version - Elapsed 1592ms
	Async/Await version
	  queue full at document 620, thread 1
	  queue full at document 673, thread 22
	  queue full at document 646, thread 24
	- Elapsed 2328ms



























