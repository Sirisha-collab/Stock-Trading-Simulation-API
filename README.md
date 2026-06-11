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

<img width="1461" height="888" alt="Placeorder API" src="https://github.com/user-attachments/assets/4948dcda-c83e-4d0a-9e09-10b3ba3372e0" />
<img width="1459" height="686" alt="Api&#39;s" src="https://github.com/user-attachments/assets/ad7c90d9-60fb-4057-a10d-8cd1116884d8" />
<img width="1477" height="878" alt="Get Stats" src="https://github.com/user-attachments/assets/996558c5-10ad-4f27-8813-baed3fdeb1c5" />

