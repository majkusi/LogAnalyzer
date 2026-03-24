# LogAnalyzer – Log Parsing & Grouping API

The backend project is built using C# and .NET with a strong focus on Clean Architecture and Domain-Driven Design (DDD) principles.

## ⚠️ IMPORTANT NOTICE

**Currently, this application is under active development.  
The core Regex parsing engine and Multi-line Stack Trace support are still being implemented.**

##

**This is a backend engine and API designed to automatically parse, analyze, and group raw application logs (e.g., Serilog output). It transforms massive amounts of unstructured text into clean, grouped incidents to help developers debug faster.**
  
[![.NET](https://img.shields.io/badge/.NET-10-white?logo=dotnet&logoColor=white&color=512BD4&style=flat)](https://dotnet.microsoft.com/)
[![xUnit](https://img.shields.io/badge/Testing-xUnit-white?logo=dotnet&logoColor=black&color=512BD4&style=flat)](https://xunit.net/)
[![MediatR](https://img.shields.io/badge/MediatR-12.0-white?logo=dotnet&logoColor=white&color=512BD4&style=flat)](https://github.com/jbogard/MediatR)

* **Backend:** ASP.NET Core 10 API using Clean Architecture and Domain-Driven Design
* **Parsing Engine:** Custom Regex-based parser with Multi-line (Stack Trace) support
* **Testing:** xUnit for robust logic verification
* **Error Handling:** Functional Result Pattern

---

## Table of Contents

1. Prerequisites
2. Getting Started
3. Project Structure
4. Core Features
5. Development Workflow ( ⚠️ WIP )
6. API URLs ( ⚠️ WIP )
7. Additional Notes ( ⚠️ WIP )
8. License

---

## Prerequisites

* [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (or newer)
* Git
* An IDE (Visual Studio, Rider, or VS Code)

---

## Getting Started

Clone the repository to your local machine:

```bash
git clone https://github.com/majkusi/LogAnalyzer.git
cd LogAnalyzer
```

## Project Structure
```
LogAnalyzer/
├── src/
│   ├── LogAnalyzer.Core/           # Domain entities (LogEntry, LogGroup) & Interfaces
│   ├── LogAnalyzer.Infrastructure/ # Technical details (RegexLogParser)
│   └── LogAnalyzer.Api/            # ASP.NET Core Presentation Layer (Controllers)
├── tests/
│   └── LogAnalyzer.Tests/          # Unit tests for Parsing and Grouping logic
└── README.md
```

## Core features 

| Feature | Description | Status |
| :--- | :--- | :--- |
| **Smart Parsing** | Extracts Timestamp, Level, and Message from raw text using Regex. | ⏳ WIP |
| **Multi-line Support** | Stateful parsing that attaches broken Stack Traces to the correct log entry. | ⏳ WIP |
| **Log Grouping** | Aggregates thousands of duplicate errors into single `LogGroup` objects. | ⏳ WIP |
| **Hybrid Architecture** | Can operate as a standalone **Microservice** (API) or as a reusable **NuGet package** (Engine). | ⏳ WIP |

## Development Workflow ( ⚠️ WIP )

## API URLs ( ⚠️ WIP )

## Additional Notes ( ⚠️ WIP )

## License
MIT License
