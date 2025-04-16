
# DeveloperEvaluationAmbev

This project follows the principles of **Clean Architecture**, with a clear separation between domain, application, infrastructure, and presentation layers.

## 🧱 Project Structure

```
backend/
├── Ambev.DeveloperEvaluation.sln
├── src/
│   ├── Ambev.DeveloperEvaluation.Domain           # Entities and domain interfaces
│   ├── Ambev.DeveloperEvaluation.Application      # Use cases and application logic
│   ├── Ambev.DeveloperEvaluation.Common           # Common utilities and helpers
│   ├── Ambev.DeveloperEvaluation.ORM              # Persistence and repositories (Entity Framework)
│   ├── Ambev.DeveloperEvaluation.IoC              # Dependency Injection
│   └── Ambev.DeveloperEvaluation.WebApi           # REST API
├── tests/
│   ├── Ambev.DeveloperEvaluation.Unit             # Unit tests
│   ├── Ambev.DeveloperEvaluation.Integration      # Integration tests
│   └── Ambev.DeveloperEvaluation.Functional       # Functional tests
```

## 🛠️ Running `update-database` via PowerShell

Make sure the startup project is set to `Ambev.DeveloperEvaluation.WebApi`. Then, open a PowerShell terminal, navigate to the `backend` folder and run:

```powershell
cd .\backend
dotnet ef database update --startup-project .\src\Ambev.DeveloperEvaluation.WebApi\ --project .\src\Ambev.DeveloperEvaluation.ORM\
```

> 💡 Requires the `Microsoft.EntityFrameworkCore.Tools` package to be installed.

## 🐳 Running with Docker Compose

Make sure Docker is installed and running. To start the services:

```powershell
cd .\backend
docker-compose up --build
```

> This will bring up the required infrastructure (e.g., database, application).

---

## 📄 License

This project is licensed under the terms of the [LICENSE](../LICENSE) file.
