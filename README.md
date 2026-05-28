**High-Performance Stock Trading Simulation API** 

This project is a high-performance stock trading simulation API built using ASP.NET Core Web API. It simulates a real-world trading system with an order matching engine, real-time price updates, and concurrency-safe order processing.
Focus on: 
* Low latency order matching
* Thread safety under high load
* Real-time event streaming
* Performance benchmarking

**⚙️ Tech Stack**

ASP.NET Core Web API
In-Memory Data Structures
SortedDictionary
Priority Queue (Heap-based order book)
Multithreading (Task Parallel Library)
Concurrency Control (locks, SemaphoreSlim)
WebSockets (SignalR / custom implementation)
BenchmarkDotNet (Performance testing)
TDD (Test Driven Development)

##  System Architecture

Client (Trader) > Web API (Order Controller) > Order Service Layer > OrderBook (In-Memory Engine) >  Matching Engine  > Trade Execution + Event Bus
