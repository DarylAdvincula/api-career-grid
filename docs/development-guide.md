# Development guide

## Workspace and prerequisites

```text
Downloads/
  api-career-grid/
    CG.sln
    CG/                 API, models, data context, repositories, migrations
    CG.tests/           Test project scaffold
    docs/               Shared product/technical documentation
  frontend-career-grid/
    src/                React starter application
    public/             Static assets
    package.json
    package-lock.json
```

Requirements: .NET SDK capable of targeting `net8.0`, .NET 8 runtime, reachable SQL Server, Node.js/npm, and a code editor. Current local checks used .NET SDK 10.0.401 with runtime 8.0.31 and Node 24.21.0/npm 11.19.0. These are observed versions, not a promise that all newer versions are compatible.

The installed Vite and React plugin declare Node `^20.19.0 || >=22.12.0`; installed ESLint declares `^20.19.0 || ^22.13.0 || >=24`. Node 24 satisfies the current installed frontend packages. Recheck engines when dependencies change. No `global.json` pins the .NET SDK.

Commands below assume the current directory is the indicated repository. On Windows PowerShell, use `npm.cmd` if `npm.ps1` is blocked by execution policy; no policy change is needed.

## Backend installation and configuration

From `api-career-grid`:

```powershell
dotnet restore CG.sln
dotnet restore CG.tests/CG.Tests.csproj
dotnet build CG.sln
```

Restore the test project separately because it is currently absent from the solution. EF package declarations use `8.*`; resolved versions can change on restore. Reproducible backend dependency pinning/locking is a pending task.

Create `CG/appsettings.Local.json` using a connection string for a development-only database. Example for Windows integrated authentication:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CareerGrid_Dev;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"
  }
}
```

Change `Server` for your actual SQL Server instance. LocalDB, SQL Express, a container, or a remote development SQL Server require their own valid connection strings. The example does not install SQL Server or create an instance. `TrustServerCertificate=True` is a local certificate workaround, not a production configuration.

`appsettings.Local.json` is ignored by Git and loaded explicitly in `Program.cs` after default configuration providers. Consequently its values override matching environment variables/command-line values in the present implementation. Do not ship the local file to production; the project also marks it for output copying when present. Standard environment-variable configuration uses `ConnectionStrings__DefaultConnection` if no local override supersedes it.

Development settings currently contain a DefaultConnection; base `appsettings.json` does not. Supply a real environment-specific connection without copying existing machine-specific settings into documentation.

## Database migrations

Check the EF CLI:

```powershell
dotnet ef --version
```

If unavailable, install a compatible .NET 8 EF CLI under your team's tooling policy. The following commands require the tool and a working configured connection:

```powershell
dotnet ef migrations list --project CG/CG.csproj --startup-project CG/CG.csproj
dotnet ef database update --project CG/CG.csproj --startup-project CG/CG.csproj
```

Only run `database update` against your intended development database. To add an agreed schema change:

```powershell
dotnet ef migrations add DescriptiveChangeName --project CG/CG.csproj --startup-project CG/CG.csproj
dotnet ef migrations script --idempotent --project CG/CG.csproj --startup-project CG/CG.csproj --output migration.sql
```

Review generated migration code/SQL and schema effects before applying it. No seed data or seed command exists. Do not manually create production users or invent an admin password as part of startup. A controlled admin/bootstrap and taxonomy seed mechanism remains to be implemented.

## Run backend

```powershell
dotnet dev-certs https --trust
dotnet run --project CG/CG.csproj --launch-profile https
```

Certificate trust is a local developer action. Launch profiles:

| Profile | URLs |
| --- | --- |
| http | `http://localhost:5094` |
| https | `https://localhost:7265` and `http://localhost:5094` |
| IIS Express | `http://localhost:21599` and SSL port 44334 |

Development Swagger: `https://localhost:7265/swagger`. The HTTPS profile is preferred for the planned browser/session integration. Swagger middleware is development-only. Empty Swagger operations are expected today: controllers and route mapping are absent. The HTTP-only profile may emit HTTPS redirection configuration warnings if no HTTPS port can be resolved.

## Frontend installation and run

From `frontend-career-grid`:

```powershell
npm.cmd ci
npm.cmd run dev
```

Use the URL printed by Vite; its usual default port is 5173, but occupied ports may change it. The current configuration does not specify a fixed port or API proxy.

```powershell
npm.cmd run build
npm.cmd run lint
npm.cmd run preview
```

Build performs `tsc -b` and creates static output in `dist`. Preview serves that build locally and is not the production deployment service.

Proposed `.env.local` once an API client is implemented:

```dotenv
VITE_API_BASE_URL=https://localhost:7265/api/v1
```

This variable has no effect in the current starter application. `.env.local` is ignored by the frontend's `*.local` rule. Never place secrets in Vite variables. Local browser integration will require a dev proxy or a narrowly configured API CORS policy; neither exists yet. Credentialed cross-origin requests also require the selected authentication/CSRF configuration.

## Tests and checks

From `api-career-grid`:

```powershell
dotnet build CG.sln --no-restore
dotnet test CG.tests/CG.Tests.csproj --no-restore
```

Current test outcome: no test cases discovered. `dotnet test CG.sln` omits the test project entirely. After integration tests exist, use a dedicated test database and ensure cleanup cannot target development/production data. `appsettings.Test.Local.json` is ignored and marked for output copying, but no test bootstrap currently loads it.

From `frontend-career-grid`, build and lint are available; no test script or browser test harness is configured.

## Troubleshooting

| Symptom | Check/action |
| --- | --- |
| npm PowerShell script blocked | Invoke `npm.cmd` |
| Database connection failure | Check actual instance, authentication, connectivity, database permissions, and which config provider wins |
| EF command missing | Install/restore an EF CLI compatible with the project's EF major version |
| Swagger has no operations | Expected baseline; implement controllers and `MapControllers` |
| `/weatherforecast` returns 404 | `CG.http` is a leftover sample; no such endpoint exists |
| Browser API request blocked | Check trusted HTTPS certificate and implement proxy/CORS for the exact frontend origin |
| Test command succeeds without tests | Inspect discovery; add test cases and include test project in solution |
| Frontend still shows Get started | Expected starter; implement specified screens/providers/router |

## Contribution workflow

Make focused changes with the corresponding documentation updates. Add migrations for schema changes, explicit DTO contracts for new endpoints, and meaningful tests for permissions/workflow invariants. Run the checks relevant to the change and record actual test counts. Do not commit credentials, local config, generated build output, or personal resume files.

The backend is a Git repository in this workspace. The frontend currently has no Git repository metadata; decide where its version history will live before establishing CI across both projects.
