# Career Grid API

Career Grid is a JobStreet-inspired recruitment platform for applicants, employers, and administrators. Applicants discover jobs and track applications; employers manage company memberships, job postings, and hiring pipelines; administrators review companies and job publication.

This repository contains the ASP.NET Core backend. The companion React application is in `../frontend-career-grid`.

## Project status

As reviewed on October 6, 2026:

- Implemented: 14 entity models, EF Core SQL Server configuration, an initial migration, and 13 repository interfaces and implementations registered in dependency injection.
- Missing: business API controllers, controller route mapping, authentication, authorization, application services, resume file storage, and automated test cases.
- The frontend renders the Vite starter page and has no API integration.

The solution builds, but it does not yet provide a usable recruitment API. `CG/CG.http` contains a leftover `/weatherforecast` request; that endpoint is not implemented.

## Documentation

Start with the [documentation index](docs/README.md).

| Document | Purpose |
| --- | --- |
| [Product requirements](docs/product-requirements.md) | Scope, roles, workflows, permissions, and acceptance criteria |
| [Architecture](docs/architecture.md) | Current structure and proposed application design |
| [Data model](docs/data-model.md) | Entities, relationships, enums, constraints, and integrity gaps |
| [API specification](docs/api-specification.md) | Proposed HTTP contracts and access rules |
| [Frontend specification](docs/frontend-specification.md) | Pages, navigation, components, and interaction behavior |
| [Development guide](docs/development-guide.md) | Installation, configuration, commands, and troubleshooting |
| [Quality and operations](docs/quality-and-operations.md) | Security, testing, deployment, and operational requirements |
| [Roadmap and decisions](docs/roadmap-and-decisions.md) | Implementation sequence, known gaps, and unresolved decisions |

## Quick start

Install a .NET SDK capable of targeting .NET 8 and a reachable SQL Server instance. From this repository:

```powershell
dotnet restore CG.sln
dotnet build CG.sln
```

Create `CG/appsettings.Local.json` with your own database connection string. This filename is ignored by Git. Follow the [development guide](docs/development-guide.md) for a sample and migration commands.

```powershell
dotnet run --project CG/CG.csproj --launch-profile https
```

The configured HTTPS URL is `https://localhost:7265`; development Swagger is available at `/swagger`. Swagger has no business operations until controllers are implemented and mapped.

## Verification

```powershell
dotnet build CG.sln --no-restore
dotnet test CG.tests/CG.Tests.csproj --no-restore
```

The test project currently has no test classes and is not included in `CG.sln`. A successful test command must not be treated as evidence of passing tests when discovery reports zero tests.

## Documentation conventions

**Current** means verified in the workspace. **Proposed** means an implementation specification, not functionality already delivered. **Decision needed** means the product or technical policy is unresolved. See [roadmap and decisions](docs/roadmap-and-decisions.md) before treating proposed rules as final.
