# Golden Raspberry Awards – API

ASP.NET Web API that loads the Golden Raspberry Awards movies CSV file into an in-memory database on application startup and exposes REST endpoints to compute producer award intervals.

This project was developed as a technical challenge following the requirements defined in the provided Back-end Assessment document.

---

## Requirements

- .NET 10 SDK
- Visual Studio 2026 (recommended) or any IDE/editor that supports .NET 10
- Git
- (Optional) `dotnet` CLI

---

## About the application

- The application reads a CSV file containing Golden Raspberry Awards data during startup.
- All data is stored in an **in-memory database**, with no external dependencies required.
- The API follows **REST maturity level 2 (Richardson)**.
- **Only integration tests** are implemented.

---

## Main endpoint

- GET /api/producers/intervals

Returns:
- The producer with the **smallest interval** between two consecutive awards.
- The producer with the **largest interval** between two consecutive awards.

---

## Clone the repository

```bash
git clone https://github.com/diogohonorio/GoldenRaspberryAwards.git
```

---

## Running from Visual Studio 2026

- Open the solution.
- Set `GoldenRaspberryAwards.Api` as the startup project.
- Press F5 (or Ctrl+F5) to run.
- Use the Test Explorer to run the tests.

---

## Running the integration tests via CLI

- Open a terminal in the project's root directory.
- Run the command
```bash
dotnet test
```

