# CShells Workbench — Multi-Tenant Blog Platform

This sample application demonstrates CShells' multi-tenancy capabilities through a blog platform where different tenants receive different features depending on their plan.

## Scenario

Three tenants share a single application, each with an escalating set of features:

### Tenants

| Shell | Path | Plan | Features |
|-------|------|------|----------|
| **Default** | `/` | Free | Core, Posts |
| **Acme** | `/acme` | Pro | Core, Posts, Comments |
| **Contoso** | `/contoso` | Enterprise | Core, Posts, Comments, Analytics |

## Key Concepts Demonstrated

### 1. Feature-Based Architecture

Each feature is a self-contained module with its own services and endpoints:

- **Core** — Tenant identity + `GET /` info endpoint (always enabled)
- **Posts** — Blog post CRUD (`GET /posts`, `GET /posts/{id}`, `POST /posts`)
- **Comments** — Reader comments (`GET /posts/{id}/comments`, `POST /posts/{id}/comments`)
- **Analytics** — Post view-count analytics (`GET /analytics`)

### 2. Per-Shell Feature Isolation

Every shell gets its own DI container, so in-memory data stores are completely isolated between tenants. Posts created in the Default shell are invisible to Acme.

### 3. Feature Dependencies

Features declare dependencies via `[ShellFeature(DependsOn = [...])]`:

```
Core ← Posts ← Comments
              ← Analytics
```

### 4. Per-Shell Feature Configuration

The Analytics feature demonstrates `IConfigurableFeature<AnalyticsOptions>` with per-shell settings defined inline in `appsettings.json`:

```json
{ "Name": "Analytics", "TopPostsCount": 10 }
```

### 5. Background Work with Shell Scopes

`ShellDemoWorker` demonstrates running background tasks within each shell's service scope using `IShellHost` and `IShellContextScopeFactory`.

## Running

```bash
cd samples/CShells.Workbench
dotnet run
```

## Example Requests

```bash
# Default shell (Free plan) — info
curl http://localhost:5031/

# Acme shell (Pro plan) — info
curl http://localhost:5031/acme/

# Contoso shell (Enterprise plan) — info
curl http://localhost:5031/contoso/

# List posts (Default shell)
curl http://localhost:5031/posts

# List posts (Acme shell — isolated data)
curl http://localhost:5031/acme/posts

# Create a post (Contoso shell)
curl -X POST http://localhost:5031/contoso/posts \
  -H "Content-Type: application/json" \
  -d '{"title":"New Post","body":"Hello from Contoso","author":"Admin"}'

# Comments — only available on Pro / Enterprise
curl http://localhost:5031/acme/posts/1/comments

# Analytics — only available on Enterprise
curl http://localhost:5031/contoso/analytics
```

## Project Structure

```
CShells.Workbench/
├── Program.cs                 ← Host setup
├── appsettings.json           ← Shell definitions (3 tenants)
├── Background/
│   └── ShellDemoWorker.cs     ← Background service per shell
└── Shells/                    ← (Optional) per-shell JSON files

CShells.Workbench.Features/
├── Core/                      ← CoreFeature (ITenantInfo, info endpoint)
├── Posts/                     ← PostsFeature (IPostRepository, CRUD endpoints)
├── Comments/                  ← CommentsFeature (ICommentRepository, endpoints)
└── Analytics/                 ← AnalyticsFeature (IAnalyticsService, endpoint)
```
