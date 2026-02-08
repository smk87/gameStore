# Agent Guidelines: gameStore (C# .NET 8)

This document provides essential context, commands, and standards for AI agents (like yourself) operating within this repository. Adhering to these guidelines ensures consistency, safety, and high-quality contributions.

## 1. Project Overview & Stack

The `gameStore` project is a modern, lightweight catalog management system for video games, built using the latest .NET technologies.

- **Framework:** .NET 8 Minimal APIs.
- **Language:** C# 12 (utilizing modern features like Primary Constructors and Collection Expressions).
- **ORM:** Entity Framework Core (EF Core) 8.0.
- **Database:** SQLite (local file: `GameStore.db`).
- **Validation:** `MinimalApis.Extensions` for automatic parameter validation.

### Architecture
- **Entities:** Located in `Entities/`. POCOs representing database tables (e.g., `Game`, `Genre`).
- **DTOs:** Located in `Dtos/`. Records used for API request/response contracts to avoid leaking domain models.
- **Endpoints:** Located in `Endpoints/`. Extension methods (e.g., `MapGameEndpoints`) to organize route groups.
- **Data:** Located in `Data/`. Contains `GameStoreContext`, migrations, and seeding logic.

---

## 2. Essential Commands

### Build, Run, and Lint
| Action | Command | Notes |
| :--- | :--- | :--- |
| **Build Project** | `dotnet build` | Run before modifying code. |
| **Run Application** | `dotnet run` | Runs on localhost. |
| **Watch Mode** | `dotnet watch run` | Hot reload enabled. |
| **Lint/Format** | `dotnet format` | Enforces C# style. |
| **Clean Build** | `dotnet clean` | Resolves cache issues. |

### Database Management (EF Core)
Requires `dotnet-ef` tool globally installed.
- **Add Migration:** `dotnet ef migrations add <MigrationName>` (e.g., `AddProductTable`)
- **Update Database:** `dotnet ef database update` (Applies pending migrations)
- **Drop Database:** `dotnet ef database drop` (Use with extreme caution)
- **Remove Last Migration:** `dotnet ef migrations remove` (Undo unapplied migration)

### Testing
*Note: No test project exists yet. If creating one, use xUnit/NUnit conventions.*
- **Run All Tests:** `dotnet test`
- **Run Specific Class:** `dotnet test --filter "ClassName"`
- **Run Single Method:** `dotnet test --filter "FullyQualifiedName~TestMethodName"`

---

## 3. Code Style & Standards

### C# 12 & Modern Syntax (Mandatory)
1. **Primary Constructors:** Use for DI in classes and for record types.
   ```csharp
   public class GameService(GameStoreContext dbContext) { ... } // Service
   public record GameDto(int Id, string Name); // DTO
   ```
2. **Collection Expressions:** Use `[]` for all array and list initializations.
   ```csharp
   List<GameDto> games = []; // Empty
   int[] numbers = [1, 2, 3]; // Populated
   ```
3. **File-Scoped Namespaces:** Reduce nesting.
   ```csharp
   namespace gameStore.Endpoints; // Correct
   // namespace gameStore.Endpoints { ... } // Avoid
   ```
4. **Target-Typed New:** Use `new()` when type is inferred.
   ```csharp
   Game game = new() { Name = "..." };
   ```

### Naming Conventions
- **PascalCase:** Classes, Methods, Properties, Records, Public Fields.
- **camelCase:** Local variables, parameters, private fields (if not in primary ctor).
- **Interfaces:** Prefix with `I` (e.g., `IGameService`).
- **Private Fields:** Prefix with `_` (e.g., `_logger`) ONLY if not injected via primary constructor.

### Formatting
- **Indentation:** 4 spaces.
- **Braces:** Allman style (new line).
- **Structure:** Organize by: Fields -> Constructors -> Properties -> Methods.

---

## 4. Architectural Patterns & Rules

### 1. DTO Separation (Strict Rule)
**Never** expose Entity models (e.g., `Game`, `Genre`) directly in API endpoints.
- **Input:** Use `CreateGameDto`, `UpdateGameDto`.
- **Output:** Use `GameDto`.
- **Mapping:** Manual mapping is currently used; keep it simple inside the endpoint or a mapper class.

### 2. Minimal API Organization
Keep `Program.cs` lean. Map endpoints using extension methods in `Endpoints/`.
```csharp
public static RouteGroupBuilder MapGameEndpoints(this WebApplication app)
{
    var group = app.MapGroup("games").WithParameterValidation();
    group.MapGet("/", ...);
    return group;
}
```

### 3. Dependency Injection
Prefer primary constructor injection.
- **DbContext:** Inject `GameStoreContext` directly into endpoint delegates or services.
- **Scope:** EF Core Contexts are scoped; Minimal API handles this automatically.

### 4. Persistence Mandate (Refactoring Priority)
The project is in a **hybrid state**.
- **Current Status:** `GameEndpoints.cs` mixes a static `List<GameDto>` with `GameStoreContext`.
- **Action Required:** When modifying endpoints, **REFACTOR** them to use `GameStoreContext` exclusively.
- **Seeding:** `Genre` data is seeded in `GameStoreContext.OnModelCreating`. Do not hardcode genres.
- **DbSets:** Use expression-bodied properties: `public DbSet<Game> Games => Set<Game>();`.
- **Transactions:** Always call `dbContext.SaveChanges()` after `Add`, `Update`, or `Remove`.

---

## 5. Error Handling & Validation

### HTTP Responses
- **GET (Found):** `Results.Ok(dto)`
- **GET (Not Found):** `Results.NotFound()`
- **POST (Success):** `Results.CreatedAtRoute(EndpointName, new { id }, dto)`
- **PUT/DELETE:** `Results.NoContent()`
- **Validation:** Handled by `.WithParameterValidation()`.

### Data Validation
Decorate DTOs with `System.ComponentModel.DataAnnotations`:
- `[Required]`, `[StringLength(N)]`, `[Range(Min, Max)]`.
- `MinimalApis.Extensions` middleware will automatically return `400 Bad Request` if validation fails.

---

## 6. Security & Safety

- **Secrets:** Never commit connection strings. Use `appsettings.json` or user secrets.
- **Safe Operations:**
  - Use `FindAsync` or `SingleOrDefaultAsync` (or synchronous equivalents if simple) for retrieval.
  - Verify entity existence before updating/deleting.
- **Migrations:** Always verify `Migrations/` directory content after adding a migration.

---

## 7. Context Awareness for Agents

1. **Pre-Change Verification:**
   - Run `dotnet build`.
   - Check `GameEndpoints.cs` to see if the target endpoint is using the static list or DB.

2. **Pattern Discovery:**
   - Use `grep` to see how `Genre` is retrieved (`dbContext.Genres.Find(...)`).
   - Mimic existing DTO records in `Dtos/`.

3. **Implementation Steps:**
   - Create/Update DTO.
   - Update Entity (if needed) & create Migration.
   - Update Endpoint logic.
   - Run `dotnet format`.
   - Verify with `dotnet run`.

4. **Common Pitfalls:**
   - Forgetting `WithParameterValidation()`.
   - Returning `Game` entity instead of `GameDto`.
   - Forgetting `SaveChanges()`.
   - Mixing static list logic with DB logic (Refactor to DB!).

---
*End of Guidelines*
