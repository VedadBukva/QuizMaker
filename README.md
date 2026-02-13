# QuizMaker API

QuizMaker API is a .NET Framework 4.8 Web API application for creating, managing and exporting quizzes.  
It supports question reuse, pagination, soft delete, dynamic exporters via MEF, and Swagger documentation.

---

## 🧱 Architecture

The solution follows a layered architecture:

- **Domain** – Entities and core business models
- **Application** – Services, DTOs and business logic
- **Infrastructure** – EF6, repositories, migrations, MEF exporter loading
- **API** – Web API controllers, Swagger, global exception handling

---

## ⚙️ Requirements

- Visual Studio 2022 (or 2019)
- .NET Framework 4.8 Developer Pack
- SQL Server LocalDB or SQL Server Express

---

## 🚀 Setup & Run

1. Open the solution in Visual Studio.
2. Ensure connection string is set in `Web.config`.
3. Open **Package Manager Console**.
4. Set Default project to `QuizMaker.Infrastructure`.
5. Run:

