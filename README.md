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

- Visual Studio 2019 (or newer)
- .NET Framework 4.8 Developer Pack
- Tested with SQL Server LocalDB (MSSQLLocalDB).
Can also run on SQL Server Express by updating the connection string.

---

## 🚀 Setup & Run

1. Open the solution in Visual Studio.
2. Ensure connection string is set in `Web.config`.
3. Open **Package Manager Console**.
4. Set Default project to `QuizMaker.Infrastructure`.
5. Run: **Update-Database**
6. Press F5 to run the project.
7. Swagger UI is available at: https://localhost:{port}/swagger


---

## 📌 Main Endpoints

- `GET /api/quizzes`
- `POST /api/quizzes`
- `GET /api/quizzes/{id}`
- `PUT /api/quizzes/{id}`
- `DELETE /api/quizzes/{id}`
- `GET /api/questions`
- `GET /api/exporters`
- `GET /api/quizzes/{id}/export?exporter=csv`

---

## 📤 Export System (MEF)

Exporters are dynamically loaded using MEF.

Supported formats:
- CSV
- TXT
- JSON
- TXT

New exporters can be added without recompiling the API by implementing `IQuizExporter` and placing the compiled DLL in the exporters folder.

---

## 🧪 Testing

Includes unit tests for:
- Quiz creation logic
- Export service validation
- Question search with pagination

---

## 📚 Design Notes

- Soft delete is implemented for quizzes.
- Questions are reusable across multiple quizzes.
- Join entity (`QuizQuestion`) preserves question order.
- Pagination and indexing are implemented for scalability.
- Global exception handling ensures consistent API responses.

---

## 🔧 Trade-offs

- Search is implemented using SQL `LIKE`. Full-text search would be recommended for large-scale systems.
- Repository + Unit of Work pattern is used for clean separation of concerns.
