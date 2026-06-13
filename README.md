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

##  System Architecture

Client (Trader) > Web API (Order Controller) > Order Service Layer > OrderBook (In-Memory Engine) >  Matching Engine  > Trade Execution + Event Bus

**PlaceOrder API**
<img width="1775" height="793" alt="Screenshot 2026-06-13 191334" src="https://github.com/user-attachments/assets/79893c57-487b-49e4-8413-d3cdb4d84eb0" />

**Response**
<img width="1751" height="863" alt="Screenshot 2026-06-13 191343" src="https://github.com/user-attachments/assets/cf97cce9-94d0-4217-b464-1338800de2a5" />
<img width="1477" height="878" alt="Get Stats" src="https://github.com/user-attachments/assets/996558c5-10ad-4f27-8813-baed3fdeb1c5" />

