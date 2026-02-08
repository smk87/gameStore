<!-- Context: project-intelligence/technical | Priority: critical | Version: 1.1 | Updated: 2026-02-08 -->

# Technical Domain

**Purpose**: Tech stack, architecture, and development patterns for the gameStore project.
**Last Updated**: 2026-02-08

## Quick Reference
**Update Triggers**: Tech stack changes | New patterns | Architecture decisions
**Audience**: Developers, AI agents

## Primary Stack
| Layer | Technology | Version | Rationale |
|-------|-----------|---------|-----------|
| Framework | .NET Minimal APIs | 8.0 | Lightweight, high-performance web APIs |
| Language | C# | 12 | Modern features (Primary Ctors, Collection Expressions) |
| ORM | EF Core | 8.0 | Standard .NET ORM for data access |
| Database | SQLite | Local | Simple, file-based persistence for catalog |
| Validation | MinimalApis.Extensions | 0.11.0 | Automatic parameter validation |

## Code Patterns
### API Endpoint (Minimal API)
```csharp
public static RouteGroupBuilder MapGameEndpoints(this WebApplication app)
{
    var group = app.MapGroup("games").WithParameterValidation();
    
    group.MapGet("/", async (GameStoreContext dbContext) => 
        await dbContext.Games
                       .Select(game => game.ToDto())
                       .AsNoTracking()
                       .ToListAsync());

    return group;
}
```

### Data Models (Entity vs DTO)
```csharp
// Entity (Internal)
public class Game {
    public int Id { get; set; }
    public required string Name { get; set; }
    public int GenreId { get; set; }
    public Genre? Genre { get; set; }
}

// DTO (External Contract)
public record GameDto(int Id, string Name, string Genre);
```

## Naming Conventions
| Type | Convention | Example |
|------|-----------|---------|
| Files | PascalCase | `GameEndpoints.cs` |
| Classes/Methods | PascalCase | `MapGameEndpoints` |
| Parameters | camelCase | `dbContext` |
| Interfaces | IPrefix | `IGameService` |
| Private Fields | `_` prefix | `_logger` (if not in primary ctor) |

## Code Standards
- **Primary Constructors**: Use for DI in classes and for record types.
- **Collection Expressions**: Use `[]` for all array and list initializations.
- **DTO Separation**: Never expose Entity models directly in API endpoints.
- **Persistence**: Refactor static lists to use `GameStoreContext` exclusively.
- **Formatting**: Allman style braces, 4-space indentation.

## Security Requirements
- **Secrets**: Never commit connection strings; use `appsettings.json` or user secrets.
- **Validation**: Decorate DTOs with `DataAnnotations` and use `.WithParameterValidation()`.
- **Safety**: Verify entity existence before updating or deleting.

## 📂 Codebase References
**Implementation**: `Endpoints/`, `Entities/`, `Dtos/`, `Data/`
**Config**: `gameStore.csproj`, `appsettings.json`

## Related Files
- [Business Domain](business-domain.md)
- [Decisions Log](decisions-log.md)
