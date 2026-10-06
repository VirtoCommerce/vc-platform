# Sign-in Audit Log Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Record every sign-in attempt — successful, failed, and login-on-behalf — in a dedicated, queryable table, surfaced in the admin UI with statistics.

**Architecture:** A new `UserSignInAttemptEvent` domain event is published at each authentication entry point. A handler enriches it and hands it to a buffered writer that batches inserts into a new `UserSignInLog` table in `SecurityDbContext`. No authentication logic changes — this is purely additive. Search, statistics, an account-blade widget and a list blade sit on top; a cron-scheduled job trims old rows.

**Tech Stack:** .NET 10, EF Core 10, OpenIddict 7, xUnit v3 + Moq + FluentAssertions, AngularJS 1.8 admin SPA.

**Spec:** [specs/2026-09-15-signin-audit-log-design.md](2026-09-15-signin-audit-log-design.md)

---

## File Structure

### Phase 1 — Storage and capture

| File | Responsibility |
| --- | --- |
| `src/VirtoCommerce.Platform.Core/Security/UserSignInLog.cs` | Domain model |
| `src/VirtoCommerce.Platform.Core/Security/SignInType.cs` | Sign-in type constants |
| `src/VirtoCommerce.Platform.Core/Security/SignInFailureReason.cs` | Failure reason constants |
| `src/VirtoCommerce.Platform.Core/Security/IUserSignInLogService.cs` | Persistence contract |
| `src/VirtoCommerce.Platform.Core/Security/IUserSignInLogWriter.cs` | Non-blocking enqueue contract |
| `src/VirtoCommerce.Platform.Core/Security/Events/UserSignInAttemptEvent.cs` | Domain event |
| `src/VirtoCommerce.Platform.Security/Model/UserSignInLogEntity.cs` | EF entity + mapping methods |
| `src/VirtoCommerce.Platform.Security/Repositories/ISecurityRepository.cs` | Add `UserSignInLogs` queryable |
| `src/VirtoCommerce.Platform.Security/Repositories/SecurityRepository.cs` | Implement it |
| `src/VirtoCommerce.Platform.Security/Repositories/SecurityDbContext.cs` | Table + index mapping |
| `src/VirtoCommerce.Platform.Security/Services/UserSignInLogService.cs` | Insert + delete-by-age |
| `src/VirtoCommerce.Platform.Security/Services/BufferedUserSignInLogWriter.cs` | Bounded channel + background flush |
| `src/VirtoCommerce.Platform.Security/Handlers/LogUserSignInEventHandler.cs` | Event → record |
| `src/VirtoCommerce.Platform.Data.{SqlServer,PostgreSql,MySql}/Migrations/Security/` | Generated migrations |

### Phase 2 — Enrichment and retention

| File | Responsibility |
| --- | --- |
| `src/VirtoCommerce.Platform.Core/Security/IUserSignInLogEnricher.cs` | Module extension point |
| `src/VirtoCommerce.Platform.Web/Security/BackgroundJobs/SignInLogCleanupJob.cs` | Retention job |
| `src/VirtoCommerce.Platform.Web/Security/BackgroundJobs/SignInLogCleanupJobPayload.cs` | Job payload |

### Phase 3 — Surfacing

| File | Responsibility |
| --- | --- |
| `src/VirtoCommerce.Platform.Core/Security/Search/UserSignInLogSearchCriteria.cs` | Search filter |
| `src/VirtoCommerce.Platform.Core/Security/Search/UserSignInLogSearchResult.cs` | Search result |
| `src/VirtoCommerce.Platform.Core/Security/Search/IUserSignInLogSearchService.cs` | Search contract |
| `src/VirtoCommerce.Platform.Core/Security/Search/UserSignInLogStats.cs` | Statistics DTO |
| `src/VirtoCommerce.Platform.Security/Services/UserSignInLogSearchService.cs` | Search + stats aggregates |
| `src/VirtoCommerce.Platform.Web/Controllers/Api/SecurityController.cs` | Two endpoints |
| `src/VirtoCommerce.Platform.Web/wwwroot/js/app/security/widgets/accountSignInLogWidget.{js,html}` | Account widget |
| `src/VirtoCommerce.Platform.Web/wwwroot/js/app/security/blades/sign-in-log.{js,html}` | List blade + stats |

---

## Phase 1 — Storage and capture

### Task 1: Domain model and constants

**Files:**
- Create: `src/VirtoCommerce.Platform.Core/Security/UserSignInLog.cs`
- Create: `src/VirtoCommerce.Platform.Core/Security/SignInType.cs`
- Create: `src/VirtoCommerce.Platform.Core/Security/SignInFailureReason.cs`

- [ ] **Step 1: Create the constants**

`SignInType.cs`:

```csharp
namespace VirtoCommerce.Platform.Core.Security;

/// <summary>
/// Kind of authentication that produced a <see cref="UserSignInLog"/> row.
/// String constants rather than an enum so modules can add their own types.
/// </summary>
public static class SignInType
{
    public const string Password = "Password";
    public const string External = "External";
    public const string Impersonation = "Impersonation";
    public const string ImpersonationRevert = "ImpersonationRevert";
    public const string ClientCredentials = "ClientCredentials";
    public const string Logout = "Logout";
}
```

`SignInFailureReason.cs`:

```csharp
namespace VirtoCommerce.Platform.Core.Security;

/// <summary>
/// Why a sign-in attempt failed. Only values reachable from an actual call site are defined —
/// do not add a reason that cannot occur.
/// </summary>
public static class SignInFailureReason
{
    public const string UserNotFound = "UserNotFound";
    public const string InvalidPassword = "InvalidPassword";
    public const string LockedOut = "LockedOut";
    public const string NotAllowed = "NotAllowed";
    public const string RequiresTwoFactor = "RequiresTwoFactor";
    public const string PasswordLoginDisabled = "PasswordLoginDisabled";
    public const string DuplicateEmail = "DuplicateEmail";
    public const string Forbidden = "Forbidden";
}
```

- [ ] **Step 2: Create the model**

`UserSignInLog.cs`:

```csharp
using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security;

/// <summary>
/// One sign-in attempt — successful or failed, including login-on-behalf.
/// Display names are denormalized on purpose: an audit row records what was true at the time,
/// so a later rename must not silently re-attribute history.
/// </summary>
public class UserSignInLog : Entity
{
    public DateTime CreatedDate { get; set; }

    /// <summary>User name exactly as submitted. The only identity field on unknown-user attempts.</summary>
    public string UserName { get; set; }

    /// <summary>Null when the account does not exist.</summary>
    public string UserId { get; set; }

    public bool Succeeded { get; set; }

    /// <summary>One of <see cref="SignInFailureReason"/>. Null when <see cref="Succeeded"/>.</summary>
    public string FailureReason { get; set; }

    /// <summary>One of <see cref="SignInType"/>.</summary>
    public string SignInType { get; set; }

    /// <summary>External identity provider name.</summary>
    public string Provider { get; set; }

    public string OperatorUserId { get; set; }
    public string OperatorUserName { get; set; }

    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public string ClientId { get; set; }

    /// <summary>OpenIddict authorization id — joins this row to a live row in Active Sessions.</summary>
    public string SessionId { get; set; }

    public string StoreId { get; set; }
    public string StoreName { get; set; }
    public string MemberId { get; set; }
    public string OrganizationId { get; set; }
    public string OrganizationName { get; set; }
}
```

- [ ] **Step 3: Build**

Run: `dotnet build src/VirtoCommerce.Platform.Core/VirtoCommerce.Platform.Core.csproj`
Expected: Build succeeded.

- [ ] **Step 4: Commit**

```bash
git add src/VirtoCommerce.Platform.Core/Security/UserSignInLog.cs src/VirtoCommerce.Platform.Core/Security/SignInType.cs src/VirtoCommerce.Platform.Core/Security/SignInFailureReason.cs
git commit -m "feat(security): add UserSignInLog domain model and sign-in constants"
```

---

### Task 2: EF entity and DbContext mapping

**Files:**
- Create: `src/VirtoCommerce.Platform.Security/Model/UserSignInLogEntity.cs`
- Modify: `src/VirtoCommerce.Platform.Security/Repositories/SecurityDbContext.cs`
- Modify: `src/VirtoCommerce.Platform.Security/Repositories/ISecurityRepository.cs`
- Modify: `src/VirtoCommerce.Platform.Security/Repositories/SecurityRepository.cs`
- Test: `tests/VirtoCommerce.Platform.Tests/Security/UserSignInLogEntityTests.cs`

- [ ] **Step 1: Write the failing test**

```csharp
using System;
using FluentAssertions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security;

public class UserSignInLogEntityTests
{
    [Fact]
    public void FromModel_ThenToModel_RoundTripsEveryField()
    {
        var source = new UserSignInLog
        {
            Id = "row-1",
            CreatedDate = new DateTime(2026, 9, 15, 10, 30, 0, DateTimeKind.Utc),
            UserName = "b2badmin@test.com",
            UserId = "user-1",
            Succeeded = true,
            FailureReason = null,
            SignInType = SignInType.Impersonation,
            Provider = null,
            OperatorUserId = "op-1",
            OperatorUserName = "support@virtocommerce.com",
            IpAddress = "203.0.113.7",
            UserAgent = "Mozilla/5.0",
            ClientId = "frontend",
            SessionId = "auth-1",
            StoreId = "B2B-store",
            StoreName = "B2B Store",
            MemberId = "member-1",
            OrganizationId = "org-1",
            OrganizationName = "Acme Inc",
        };

        var roundTripped = new UserSignInLogEntity()
            .FromModel(source, new PrimaryKeyResolvingMap())
            .ToModel(new UserSignInLog());

        roundTripped.Should().BeEquivalentTo(source);
    }

    [Fact]
    public void FromModel_NullModel_Throws()
    {
        var act = () => new UserSignInLogEntity().FromModel(null, new PrimaryKeyResolvingMap());

        act.Should().Throw<ArgumentNullException>();
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests --filter "FullyQualifiedName~UserSignInLogEntityTests"`
Expected: FAIL — `UserSignInLogEntity` does not exist (compile error).

- [ ] **Step 3: Create the entity**

`src/VirtoCommerce.Platform.Security/Model/UserSignInLogEntity.cs`:

```csharp
using System;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.Model;

/// <summary>
/// Append-only audit row. Derives from <see cref="Entity"/> rather than AuditableEntity:
/// rows are never modified, so ModifiedBy/ModifiedDate would be dead columns on the
/// highest-write table in the system.
/// </summary>
public class UserSignInLogEntity : Entity
{
    public DateTime CreatedDate { get; set; }
    public string UserName { get; set; }
    public string UserId { get; set; }
    public bool Succeeded { get; set; }
    public string FailureReason { get; set; }
    public string SignInType { get; set; }
    public string Provider { get; set; }
    public string OperatorUserId { get; set; }
    public string OperatorUserName { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public string ClientId { get; set; }
    public string SessionId { get; set; }
    public string StoreId { get; set; }
    public string StoreName { get; set; }
    public string MemberId { get; set; }
    public string OrganizationId { get; set; }
    public string OrganizationName { get; set; }

    public virtual UserSignInLogEntity FromModel(UserSignInLog record, PrimaryKeyResolvingMap pkMap)
    {
        ArgumentNullException.ThrowIfNull(record);

        pkMap.AddPair(record, this);

        Id = record.Id;
        CreatedDate = record.CreatedDate;
        UserName = record.UserName;
        UserId = record.UserId;
        Succeeded = record.Succeeded;
        FailureReason = record.FailureReason;
        SignInType = record.SignInType;
        Provider = record.Provider;
        OperatorUserId = record.OperatorUserId;
        OperatorUserName = record.OperatorUserName;
        IpAddress = record.IpAddress;
        UserAgent = record.UserAgent;
        ClientId = record.ClientId;
        SessionId = record.SessionId;
        StoreId = record.StoreId;
        StoreName = record.StoreName;
        MemberId = record.MemberId;
        OrganizationId = record.OrganizationId;
        OrganizationName = record.OrganizationName;

        return this;
    }

    public virtual UserSignInLog ToModel(UserSignInLog record)
    {
        ArgumentNullException.ThrowIfNull(record);

        record.Id = Id;
        record.CreatedDate = CreatedDate;
        record.UserName = UserName;
        record.UserId = UserId;
        record.Succeeded = Succeeded;
        record.FailureReason = FailureReason;
        record.SignInType = SignInType;
        record.Provider = Provider;
        record.OperatorUserId = OperatorUserId;
        record.OperatorUserName = OperatorUserName;
        record.IpAddress = IpAddress;
        record.UserAgent = UserAgent;
        record.ClientId = ClientId;
        record.SessionId = SessionId;
        record.StoreId = StoreId;
        record.StoreName = StoreName;
        record.MemberId = MemberId;
        record.OrganizationId = OrganizationId;
        record.OrganizationName = OrganizationName;

        return record;
    }
}
```

- [ ] **Step 4: Map the table in `SecurityDbContext`**

In `OnModelCreating`, immediately after the `UserPasswordHistoryEntity` block (around line 50), add:

```csharp
            builder.Entity<UserSignInLogEntity>().ToEntityTable("UserSignInLog");
            builder.Entity<UserSignInLogEntity>().Property(x => x.UserName).HasMaxLength(Length256);
            builder.Entity<UserSignInLogEntity>().Property(x => x.UserId).HasMaxLength(IdLength);
            builder.Entity<UserSignInLogEntity>().Property(x => x.FailureReason).HasMaxLength(Length64);
            builder.Entity<UserSignInLogEntity>().Property(x => x.SignInType).HasMaxLength(Length32).IsRequired();
            builder.Entity<UserSignInLogEntity>().Property(x => x.Provider).HasMaxLength(Length128);
            builder.Entity<UserSignInLogEntity>().Property(x => x.OperatorUserId).HasMaxLength(IdLength);
            builder.Entity<UserSignInLogEntity>().Property(x => x.OperatorUserName).HasMaxLength(Length256);
            builder.Entity<UserSignInLogEntity>().Property(x => x.IpAddress).HasMaxLength(Length64);
            builder.Entity<UserSignInLogEntity>().Property(x => x.UserAgent).HasMaxLength(Length512);
            builder.Entity<UserSignInLogEntity>().Property(x => x.ClientId).HasMaxLength(IdLength);
            builder.Entity<UserSignInLogEntity>().Property(x => x.SessionId).HasMaxLength(IdLength);
            builder.Entity<UserSignInLogEntity>().Property(x => x.StoreId).HasMaxLength(IdLength);
            builder.Entity<UserSignInLogEntity>().Property(x => x.StoreName).HasMaxLength(Length256);
            builder.Entity<UserSignInLogEntity>().Property(x => x.MemberId).HasMaxLength(IdLength);
            builder.Entity<UserSignInLogEntity>().Property(x => x.OrganizationId).HasMaxLength(IdLength);
            builder.Entity<UserSignInLogEntity>().Property(x => x.OrganizationName).HasMaxLength(Length256);

            // Three indexes only. Each one is write-amplification on the highest-insert table in the
            // system; the deferred ones (Succeeded, StoreId, OrganizationId) are added once real query
            // patterns exist. See spec §4.1.
            builder.Entity<UserSignInLogEntity>().HasIndex(x => x.CreatedDate);
            builder.Entity<UserSignInLogEntity>().HasIndex(x => new { x.UserId, x.CreatedDate });
            builder.Entity<UserSignInLogEntity>().HasIndex(x => new { x.IpAddress, x.CreatedDate });
```

Note: there is deliberately **no** foreign key to `ApplicationUser`. Rows must survive user deletion — an audit trail that disappears when the account does is not an audit trail.

- [ ] **Step 5: Expose it on the repository**

In `ISecurityRepository.cs`, add to the interface:

```csharp
        IQueryable<UserSignInLogEntity> UserSignInLogs { get; }
```

In `SecurityRepository.cs`, add:

```csharp
        public virtual IQueryable<UserSignInLogEntity> UserSignInLogs => DbContext.Set<UserSignInLogEntity>();
```

- [ ] **Step 6: Run test to verify it passes**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests --filter "FullyQualifiedName~UserSignInLogEntityTests"`
Expected: PASS — 2 tests.

- [ ] **Step 7: Commit**

```bash
git add src/VirtoCommerce.Platform.Security/Model/UserSignInLogEntity.cs src/VirtoCommerce.Platform.Security/Repositories/ tests/VirtoCommerce.Platform.Tests/Security/UserSignInLogEntityTests.cs
git commit -m "feat(security): add UserSignInLogEntity with SecurityDbContext mapping"
```

---

### Task 3: Provider migrations

**Files:**
- Create: `src/VirtoCommerce.Platform.Data.SqlServer/Migrations/Security/*_AddUserSignInLog.cs`
- Create: `src/VirtoCommerce.Platform.Data.PostgreSql/Migrations/Security/*_AddUserSignInLog.cs`
- Create: `src/VirtoCommerce.Platform.Data.MySql/Migrations/Security/*_AddUserSignInLog.cs`

Migrations are **generated, never hand-written**. Each provider has an
`IDesignTimeDbContextFactory` for both contexts, so no extra wiring is needed.

- [ ] **Step 1: Ensure the EF tooling is at the pinned version**

```bash
dotnet tool update --global dotnet-ef --version 10.0.10
```

- [ ] **Step 2: Generate the SqlServer migration**

```bash
cd src/VirtoCommerce.Platform.Data.SqlServer && dotnet ef migrations add AddUserSignInLog --context SecurityDBContext
```

Expected: `Done. To undo this action, use 'ef migrations remove'` and three new files under `Migrations/Security/`.

- [ ] **Step 3: Generate the PostgreSql migration**

```bash
cd src/VirtoCommerce.Platform.Data.PostgreSql && dotnet ef migrations add AddUserSignInLog --context SecurityDBContext
```

Expected: same, under the PostgreSql project.

- [ ] **Step 4: Generate the MySql migration**

```bash
cd src/VirtoCommerce.Platform.Data.MySql && dotnet ef migrations add AddUserSignInLog --context SecurityDBContext
```

Expected: same, under the MySql project.

- [ ] **Step 5: Verify each migration creates the table and all three indexes**

Open each generated `*_AddUserSignInLog.cs` and confirm `Up()` contains
`migrationBuilder.CreateTable(name: "UserSignInLog", ...)` plus three
`CreateIndex` calls (`CreatedDate`; `UserId, CreatedDate`; `IpAddress, CreatedDate`).
If any is missing, the `SecurityDbContext` mapping from Task 2 is wrong — fix it
there and regenerate rather than editing the migration.

- [ ] **Step 6: Build the solution**

Run: `dotnet build VirtoCommerce.Platform.sln`
Expected: Build succeeded.

- [ ] **Step 7: Commit**

```bash
git add src/VirtoCommerce.Platform.Data.SqlServer/Migrations src/VirtoCommerce.Platform.Data.PostgreSql/Migrations src/VirtoCommerce.Platform.Data.MySql/Migrations
git commit -m "feat(security): add UserSignInLog migrations for SqlServer, PostgreSql and MySql"
```

---

### Task 4: Persistence service

**Files:**
- Create: `src/VirtoCommerce.Platform.Core/Security/IUserSignInLogService.cs`
- Create: `src/VirtoCommerce.Platform.Security/Services/UserSignInLogService.cs`
- Modify: `src/VirtoCommerce.Platform.Web/Security/ServiceCollectionExtensions.cs`
- Test: `tests/VirtoCommerce.Platform.Tests/Security/UserSignInLogServiceTests.cs`

- [ ] **Step 1: Write the failing test**

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;
using VirtoCommerce.Platform.Security.Services;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security;

public class UserSignInLogServiceTests
{
    [Fact]
    public async Task SaveChangesAsync_AddsOneEntityPerRecordAndCommitsOnce()
    {
        var added = new List<UserSignInLogEntity>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var repository = new Mock<ISecurityRepository>();
        repository.Setup(x => x.UserSignInLogs).Returns(new List<UserSignInLogEntity>().AsQueryable().BuildMock());
        repository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
        repository.Setup(x => x.Add(It.IsAny<UserSignInLogEntity>()))
            .Callback<UserSignInLogEntity>(added.Add);

        var service = new UserSignInLogService(() => repository.Object);

        await service.SaveChangesAsync(new[]
        {
            new UserSignInLog { UserName = "a", SignInType = SignInType.Password, Succeeded = true },
            new UserSignInLog { UserName = "b", SignInType = SignInType.Password, Succeeded = false },
        });

        added.Should().HaveCount(2);
        added.Select(x => x.UserName).Should().BeEquivalentTo("a", "b");
        unitOfWork.Verify(x => x.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task SaveChangesAsync_EmptyBatch_DoesNotTouchTheRepository()
    {
        var repository = new Mock<ISecurityRepository>();
        var service = new UserSignInLogService(() => repository.Object);

        await service.SaveChangesAsync(Array.Empty<UserSignInLog>());

        repository.Verify(x => x.Add(It.IsAny<UserSignInLogEntity>()), Times.Never);
    }

    [Fact]
    public async Task DeleteOlderThanAsync_RemovesOnlyRowsBeforeTheCutoff()
    {
        var cutoff = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc);
        var removed = new List<UserSignInLogEntity>();
        var rows = new List<UserSignInLogEntity>
        {
            new() { Id = "old", CreatedDate = cutoff.AddDays(-1) },
            new() { Id = "new", CreatedDate = cutoff.AddDays(1) },
        };

        var repository = new Mock<ISecurityRepository>();
        repository.Setup(x => x.UserSignInLogs).Returns(rows.AsQueryable().BuildMock());
        repository.Setup(x => x.UnitOfWork).Returns(Mock.Of<IUnitOfWork>());
        repository.Setup(x => x.Remove(It.IsAny<UserSignInLogEntity>()))
            .Callback<UserSignInLogEntity>(removed.Add);

        var service = new UserSignInLogService(() => repository.Object);

        var deleted = await service.DeleteOlderThanAsync(cutoff, batchSize: 100);

        deleted.Should().Be(1);
        removed.Should().ContainSingle().Which.Id.Should().Be("old");
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests --filter "FullyQualifiedName~UserSignInLogServiceTests"`
Expected: FAIL — `UserSignInLogService` does not exist.

- [ ] **Step 3: Create the contract**

`src/VirtoCommerce.Platform.Core/Security/IUserSignInLogService.cs`:

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security;

public interface IUserSignInLogService
{
    /// <summary>Insert a batch of audit rows.</summary>
    Task SaveChangesAsync(IList<UserSignInLog> records);

    /// <summary>Delete rows created before <paramref name="cutoff"/>. Returns the number deleted.</summary>
    Task<int> DeleteOlderThanAsync(DateTime cutoff, int batchSize);
}
```

- [ ] **Step 4: Implement it**

`src/VirtoCommerce.Platform.Security/Services/UserSignInLogService.cs`:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;

namespace VirtoCommerce.Platform.Security.Services;

public class UserSignInLogService : IUserSignInLogService
{
    private readonly Func<ISecurityRepository> _repositoryFactory;

    public UserSignInLogService(Func<ISecurityRepository> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public virtual async Task SaveChangesAsync(IList<UserSignInLog> records)
    {
        if (records is null || records.Count == 0)
        {
            return;
        }

        var pkMap = new PrimaryKeyResolvingMap();

        using var repository = _repositoryFactory();

        foreach (var record in records)
        {
            repository.Add(AbstractTypeFactory<UserSignInLogEntity>.TryCreateInstance().FromModel(record, pkMap));
        }

        await repository.UnitOfWork.CommitAsync();
    }

    public virtual async Task<int> DeleteOlderThanAsync(DateTime cutoff, int batchSize)
    {
        using var repository = _repositoryFactory();

        var expired = await repository.UserSignInLogs
            .Where(x => x.CreatedDate < cutoff)
            .OrderBy(x => x.CreatedDate)
            .Take(batchSize)
            .ToListAsync();

        if (expired.Count == 0)
        {
            return 0;
        }

        foreach (var entity in expired)
        {
            repository.Remove(entity);
        }

        await repository.UnitOfWork.CommitAsync();

        return expired.Count;
    }
}
```

- [ ] **Step 5: Register it**

In `src/VirtoCommerce.Platform.Web/Security/ServiceCollectionExtensions.cs`, next to the
existing `services.AddSingleton<IUserApiKeyService, UserApiKeyService>();` line, add:

```csharp
            services.AddSingleton<IUserSignInLogService, UserSignInLogService>();
```

- [ ] **Step 6: Run test to verify it passes**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests --filter "FullyQualifiedName~UserSignInLogServiceTests"`
Expected: PASS — 3 tests.

- [ ] **Step 7: Commit**

```bash
git add src/VirtoCommerce.Platform.Core/Security/IUserSignInLogService.cs src/VirtoCommerce.Platform.Security/Services/UserSignInLogService.cs src/VirtoCommerce.Platform.Web/Security/ServiceCollectionExtensions.cs tests/VirtoCommerce.Platform.Tests/Security/UserSignInLogServiceTests.cs
git commit -m "feat(security): add UserSignInLogService for batched insert and retention delete"
```

---

### Task 5: Buffered writer

The sign-in endpoints are unauthenticated. A synchronous insert per attempt puts the
database on the sign-in latency path and turns a credential-stuffing run into a write
amplifier. The writer accepts rows into a bounded channel and flushes them in batches.

**Files:**
- Create: `src/VirtoCommerce.Platform.Core/Security/IUserSignInLogWriter.cs`
- Create: `src/VirtoCommerce.Platform.Security/Services/BufferedUserSignInLogWriter.cs`
- Modify: `src/VirtoCommerce.Platform.Web/Security/ServiceCollectionExtensions.cs`
- Test: `tests/VirtoCommerce.Platform.Tests/Security/BufferedUserSignInLogWriterTests.cs`

- [ ] **Step 1: Write the failing test**

```csharp
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Services;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security;

public class BufferedUserSignInLogWriterTests
{
    [Fact]
    public async Task Write_ThenFlush_PersistsEveryQueuedRecord()
    {
        var saved = new ConcurrentBag<UserSignInLog>();
        var service = new Mock<IUserSignInLogService>();
        service.Setup(x => x.SaveChangesAsync(It.IsAny<IList<UserSignInLog>>()))
            .Callback<IList<UserSignInLog>>(batch => { foreach (var r in batch) saved.Add(r); })
            .Returns(Task.CompletedTask);

        var writer = new BufferedUserSignInLogWriter(service.Object, NullLogger<BufferedUserSignInLogWriter>.Instance, capacity: 100, batchSize: 10);

        writer.Write(new UserSignInLog { UserName = "a", SignInType = SignInType.Password });
        writer.Write(new UserSignInLog { UserName = "b", SignInType = SignInType.Password });

        await writer.FlushAsync(CancellationToken.None);

        saved.Select(x => x.UserName).Should().BeEquivalentTo("a", "b");
    }

    [Fact]
    public void Write_AssignsIdAndCreatedDateWhenMissing()
    {
        var service = new Mock<IUserSignInLogService>();
        var writer = new BufferedUserSignInLogWriter(service.Object, NullLogger<BufferedUserSignInLogWriter>.Instance, capacity: 10, batchSize: 10);

        var record = new UserSignInLog { UserName = "a", SignInType = SignInType.Password };

        writer.Write(record);

        record.Id.Should().NotBeNullOrEmpty();
        record.CreatedDate.Should().NotBe(default);
    }

    [Fact]
    public void Write_WhenBufferIsFull_DropsOldestAndCountsTheDrop()
    {
        var service = new Mock<IUserSignInLogService>();
        var writer = new BufferedUserSignInLogWriter(service.Object, NullLogger<BufferedUserSignInLogWriter>.Instance, capacity: 2, batchSize: 10);

        writer.Write(new UserSignInLog { UserName = "a", SignInType = SignInType.Password });
        writer.Write(new UserSignInLog { UserName = "b", SignInType = SignInType.Password });
        writer.Write(new UserSignInLog { UserName = "c", SignInType = SignInType.Password });

        // A silent gap in an audit trail is worse than a visible one: the drop must be observable.
        writer.DroppedCount.Should().Be(1);
    }

    [Fact]
    public async Task FlushAsync_WhenPersistenceThrows_DoesNotPropagate()
    {
        var service = new Mock<IUserSignInLogService>();
        service.Setup(x => x.SaveChangesAsync(It.IsAny<IList<UserSignInLog>>()))
            .ThrowsAsync(new InvalidOperationException("db down"));

        var writer = new BufferedUserSignInLogWriter(service.Object, NullLogger<BufferedUserSignInLogWriter>.Instance, capacity: 10, batchSize: 10);
        writer.Write(new UserSignInLog { UserName = "a", SignInType = SignInType.Password });

        var act = async () => await writer.FlushAsync(CancellationToken.None);

        await act.Should().NotThrowAsync();
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests --filter "FullyQualifiedName~BufferedUserSignInLogWriterTests"`
Expected: FAIL — `BufferedUserSignInLogWriter` does not exist.

- [ ] **Step 3: Create the contract**

`src/VirtoCommerce.Platform.Core/Security/IUserSignInLogWriter.cs`:

```csharp
namespace VirtoCommerce.Platform.Core.Security;

/// <summary>
/// Non-blocking entry point for audit rows. Implementations must never throw and never
/// block the caller — a failing audit log must not fail a sign-in.
/// </summary>
public interface IUserSignInLogWriter
{
    void Write(UserSignInLog record);
}
```

- [ ] **Step 4: Implement it**

`src/VirtoCommerce.Platform.Security/Services/BufferedUserSignInLogWriter.cs`:

```csharp
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.Services;

/// <summary>
/// Buffers audit rows in a bounded channel and flushes them in batches on a background loop.
/// Keeps the database off the sign-in latency path and stops an unauthenticated endpoint from
/// becoming a write amplifier.
/// </summary>
public class BufferedUserSignInLogWriter : BackgroundService, IUserSignInLogWriter
{
    private static readonly TimeSpan _flushInterval = TimeSpan.FromSeconds(5);

    private readonly IUserSignInLogService _service;
    private readonly ILogger<BufferedUserSignInLogWriter> _logger;
    private readonly Channel<UserSignInLog> _channel;
    private readonly int _batchSize;

    private int _droppedCount;

    public BufferedUserSignInLogWriter(
        IUserSignInLogService service,
        ILogger<BufferedUserSignInLogWriter> logger,
        int capacity = 10_000,
        int batchSize = 200)
    {
        _service = service;
        _logger = logger;
        _batchSize = batchSize;
        _channel = Channel.CreateBounded<UserSignInLog>(new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.DropOldest,
            SingleReader = true,
        });
    }

    /// <summary>Rows discarded because the buffer was full. Never silently zero when drops happened.</summary>
    public int DroppedCount => _droppedCount;

    public void Write(UserSignInLog record)
    {
        if (record is null)
        {
            return;
        }

        if (string.IsNullOrEmpty(record.Id))
        {
            record.Id = Guid.NewGuid().ToString("N");
        }

        if (record.CreatedDate == default)
        {
            record.CreatedDate = DateTime.UtcNow;
        }

        // DropOldest means TryWrite still returns true after evicting; compare counts to detect the drop.
        var before = _channel.Reader.Count;
        if (!_channel.Writer.TryWrite(record))
        {
            Interlocked.Increment(ref _droppedCount);
            return;
        }

        if (before >= _channel.Reader.Count && before > 0)
        {
            var dropped = Interlocked.Increment(ref _droppedCount);
            _logger.LogWarning("Sign-in audit buffer full; dropped {DroppedCount} record(s) so far.", dropped);
        }
    }

    public virtual async Task FlushAsync(CancellationToken cancellationToken)
    {
        var batch = new List<UserSignInLog>(_batchSize);

        while (_channel.Reader.TryRead(out var record))
        {
            batch.Add(record);

            if (batch.Count >= _batchSize)
            {
                await PersistAsync(batch);
                batch = new List<UserSignInLog>(_batchSize);
            }
        }

        if (batch.Count > 0)
        {
            await PersistAsync(batch);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_flushInterval);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await timer.WaitForNextTickAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }

            await FlushAsync(stoppingToken);
        }

        // Drain whatever is left so a graceful shutdown does not lose the tail of the trail.
        await FlushAsync(CancellationToken.None);
    }

    private async Task PersistAsync(List<UserSignInLog> batch)
    {
        try
        {
            await _service.SaveChangesAsync(batch);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to persist {Count} sign-in audit record(s).", batch.Count);
        }
    }
}
```

- [ ] **Step 5: Register it**

In `ServiceCollectionExtensions.cs`, below the `IUserSignInLogService` registration from Task 4:

```csharp
            // One instance serving both roles: the hosted service drains the same channel the writer fills.
            services.AddSingleton<BufferedUserSignInLogWriter>();
            services.AddSingleton<IUserSignInLogWriter>(provider => provider.GetRequiredService<BufferedUserSignInLogWriter>());
            services.AddHostedService(provider => provider.GetRequiredService<BufferedUserSignInLogWriter>());
```

- [ ] **Step 6: Run test to verify it passes**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests --filter "FullyQualifiedName~BufferedUserSignInLogWriterTests"`
Expected: PASS — 4 tests.

- [ ] **Step 7: Commit**

```bash
git add src/VirtoCommerce.Platform.Core/Security/IUserSignInLogWriter.cs src/VirtoCommerce.Platform.Security/Services/BufferedUserSignInLogWriter.cs src/VirtoCommerce.Platform.Web/Security/ServiceCollectionExtensions.cs tests/VirtoCommerce.Platform.Tests/Security/BufferedUserSignInLogWriterTests.cs
git commit -m "feat(security): add buffered sign-in audit writer with bounded channel"
```

---

### Task 6: Domain event

**Files:**
- Create: `src/VirtoCommerce.Platform.Core/Security/Events/UserSignInAttemptEvent.cs`

- [ ] **Step 1: Create the event**

```csharp
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Security.Events;

/// <summary>
/// Raised for every sign-in attempt, successful or failed, at every authentication entry point.
/// Separate from <see cref="UserLoginEvent"/> because that one requires an <see cref="ApplicationUser"/>
/// and the most security-relevant failure — unknown user name — has no user object at all.
/// <see cref="UserLoginEvent"/> and <see cref="UserLogoutEvent"/> keep their existing behaviour and
/// are still published alongside this one.
/// </summary>
public class UserSignInAttemptEvent : DomainEvent
{
    /// <summary>User name exactly as submitted.</summary>
    public string UserName { get; set; }

    /// <summary>Null when the account does not exist.</summary>
    public string UserId { get; set; }

    public bool Succeeded { get; set; }

    /// <summary>One of <see cref="SignInFailureReason"/>. Null when <see cref="Succeeded"/>.</summary>
    public string FailureReason { get; set; }

    /// <summary>One of <see cref="SignInType"/>.</summary>
    public string SignInType { get; set; }

    public string Provider { get; set; }

    public string OperatorUserId { get; set; }
    public string OperatorUserName { get; set; }

    public string ClientId { get; set; }
    public string SessionId { get; set; }

    /// <summary>Set from the signed-in user when one exists; null for unknown-user attempts.</summary>
    public string StoreId { get; set; }
    public string MemberId { get; set; }
}
```

- [ ] **Step 2: Build**

Run: `dotnet build src/VirtoCommerce.Platform.Core/VirtoCommerce.Platform.Core.csproj`
Expected: Build succeeded.

- [ ] **Step 3: Commit**

```bash
git add src/VirtoCommerce.Platform.Core/Security/Events/UserSignInAttemptEvent.cs
git commit -m "feat(security): add UserSignInAttemptEvent"
```

---

### Task 7: Settings

**Files:**
- Modify: `src/VirtoCommerce.Platform.Core/PlatformConstants.cs`

- [ ] **Step 1: Add the descriptors**

In `PlatformConstants.Settings.Security`, next to `CronPruneExpiredTokensJob` (around line 217):

```csharp
                public static SettingDescriptor SignInLogEnabled { get; } = new()
                {
                    Name = "VirtoCommerce.Platform.Security.SignInLogEnabled",
                    GroupName = "Platform|Security",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = true,
                };

                public static SettingDescriptor SignInLogRetentionDays { get; } = new()
                {
                    Name = "VirtoCommerce.Platform.Security.SignInLogRetentionDays",
                    GroupName = "Platform|Security",
                    ValueType = SettingValueType.Integer,
                    DefaultValue = 90,
                };

                public static SettingDescriptor EnableSignInLogCleanupJob { get; } = new()
                {
                    Name = "VirtoCommerce.Platform.Security.EnableSignInLogCleanupJob",
                    GroupName = "Platform|Security",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = true,
                };

                public static SettingDescriptor CronSignInLogCleanupJob { get; } = new()
                {
                    Name = "VirtoCommerce.Platform.Security.CronSignInLogCleanupJob",
                    GroupName = "Platform|Security",
                    ValueType = SettingValueType.Cron,
                    DefaultValue = "0 0 */1 * *",
                };
```

`EnableSignInLogCleanupJob` exists because `IRecurringJobScheduleBuilder.FromSettings`
takes an enabler **and** a cron descriptor — it is required by the framework API, not a
design preference.

- [ ] **Step 2: Yield them from the settings collection**

In the same class, find the `AllSecuritySettings` / `yield return` block containing
`yield return CronPruneExpiredTokensJob;` and add:

```csharp
                        yield return SignInLogEnabled;
                        yield return SignInLogRetentionDays;
                        yield return EnableSignInLogCleanupJob;
                        yield return CronSignInLogCleanupJob;
```

- [ ] **Step 3: Build**

Run: `dotnet build src/VirtoCommerce.Platform.Core/VirtoCommerce.Platform.Core.csproj`
Expected: Build succeeded.

- [ ] **Step 4: Commit**

```bash
git add src/VirtoCommerce.Platform.Core/PlatformConstants.cs
git commit -m "feat(security): add sign-in log settings"
```

---

### Task 8: Event handler

**Files:**
- Create: `src/VirtoCommerce.Platform.Security/Handlers/LogUserSignInEventHandler.cs`
- Modify: `src/VirtoCommerce.Platform.Web/Security/ApplicationBuilderExtensions.cs`
- Test: `tests/VirtoCommerce.Platform.Tests/Security/LogUserSignInEventHandlerTests.cs`

- [ ] **Step 1: Write the failing test**

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Security.Handlers;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security;

public class LogUserSignInEventHandlerTests
{
    [Fact]
    public async Task Handle_PasswordSignIn_WritesRecordWithRequestContext()
    {
        var written = new List<UserSignInLog>();
        var handler = CreateHandler(written, signInLogEnabled: true, ip: "203.0.113.7", userAgent: "UA/1.0");

        await handler.Handle(new UserSignInAttemptEvent
        {
            UserName = "b2badmin@test.com",
            UserId = "user-1",
            Succeeded = false,
            FailureReason = SignInFailureReason.InvalidPassword,
            SignInType = SignInType.Password,
            StoreId = "B2B-store",
            MemberId = "member-1",
        });

        var record = written.Should().ContainSingle().Subject;
        record.UserName.Should().Be("b2badmin@test.com");
        record.FailureReason.Should().Be(SignInFailureReason.InvalidPassword);
        record.IpAddress.Should().Be("203.0.113.7");
        record.UserAgent.Should().Be("UA/1.0");
        record.StoreId.Should().Be("B2B-store");
        record.MemberId.Should().Be("member-1");
    }

    [Fact]
    public async Task Handle_WhenDisabled_SuppressesOrdinarySignIn()
    {
        var written = new List<UserSignInLog>();
        var handler = CreateHandler(written, signInLogEnabled: false);

        await handler.Handle(new UserSignInAttemptEvent { UserName = "a", SignInType = SignInType.Password, Succeeded = true });

        written.Should().BeEmpty();
    }

    [Theory]
    [InlineData(SignInType.Impersonation)]
    [InlineData(SignInType.ImpersonationRevert)]
    public async Task Handle_WhenDisabled_StillWritesImpersonation(string signInType)
    {
        var written = new List<UserSignInLog>();
        var handler = CreateHandler(written, signInLogEnabled: false);

        await handler.Handle(new UserSignInAttemptEvent
        {
            UserName = "b2badmin@test.com",
            SignInType = signInType,
            Succeeded = true,
            OperatorUserName = "support@virtocommerce.com",
        });

        // The compliance anchor must not be switchable off.
        written.Should().ContainSingle().Which.OperatorUserName.Should().Be("support@virtocommerce.com");
    }

    [Fact]
    public async Task Handle_WhenEnricherThrows_StillWritesTheRecord()
    {
        var written = new List<UserSignInLog>();
        var enricher = new Mock<IUserSignInLogEnricher>();
        enricher.SetupGet(x => x.Priority).Returns(0);
        enricher.Setup(x => x.EnrichAsync(It.IsAny<UserSignInLog>())).ThrowsAsync(new System.Exception("module down"));

        var handler = CreateHandler(written, signInLogEnabled: true, enrichers: new[] { enricher.Object });

        await handler.Handle(new UserSignInAttemptEvent { UserName = "a", SignInType = SignInType.Password, Succeeded = true });

        written.Should().ContainSingle();
    }

    private static LogUserSignInEventHandler CreateHandler(
        List<UserSignInLog> written,
        bool signInLogEnabled,
        string ip = null,
        string userAgent = null,
        IEnumerable<IUserSignInLogEnricher> enrichers = null)
    {
        var writer = new Mock<IUserSignInLogWriter>();
        writer.Setup(x => x.Write(It.IsAny<UserSignInLog>())).Callback<UserSignInLog>(written.Add);

        var settings = new Mock<ISettingsManager>();
        settings.Setup(x => x.GetValue<bool>(PlatformConstants.Settings.Security.SignInLogEnabled)).Returns(signInLogEnabled);

        var httpContext = new DefaultHttpContext();
        if (ip is not null)
        {
            httpContext.Connection.RemoteIpAddress = System.Net.IPAddress.Parse(ip);
        }
        if (userAgent is not null)
        {
            httpContext.Request.Headers.UserAgent = userAgent;
        }

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.SetupGet(x => x.HttpContext).Returns(httpContext);

        return new LogUserSignInEventHandler(
            writer.Object,
            settings.Object,
            accessor.Object,
            enrichers ?? []);
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests --filter "FullyQualifiedName~LogUserSignInEventHandlerTests"`
Expected: FAIL — `LogUserSignInEventHandler` and `IUserSignInLogEnricher` do not exist.

- [ ] **Step 3: Create the enricher contract**

`src/VirtoCommerce.Platform.Core/Security/IUserSignInLogEnricher.cs`:

```csharp
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security;

/// <summary>
/// Extension point for modules to add context the platform cannot resolve on its own.
/// Organization is the motivating case: the platform knows MemberId but resolving it to an
/// organization lives in the customer module, and Platform.Security must not depend on a module.
/// Implementations run in <see cref="Priority"/> order and must be cheap — they sit on the
/// audit write path.
/// </summary>
public interface IUserSignInLogEnricher
{
    int Priority { get; }

    Task EnrichAsync(UserSignInLog record);
}
```

- [ ] **Step 4: Implement the handler**

`src/VirtoCommerce.Platform.Security/Handlers/LogUserSignInEventHandler.cs`:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Security.Handlers;

public class LogUserSignInEventHandler : IEventHandler<UserSignInAttemptEvent>
{
    private readonly IUserSignInLogWriter _writer;
    private readonly ISettingsManager _settingsManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEnumerable<IUserSignInLogEnricher> _enrichers;

    public LogUserSignInEventHandler(
        IUserSignInLogWriter writer,
        ISettingsManager settingsManager,
        IHttpContextAccessor httpContextAccessor,
        IEnumerable<IUserSignInLogEnricher> enrichers)
    {
        _writer = writer;
        _settingsManager = settingsManager;
        _httpContextAccessor = httpContextAccessor;
        _enrichers = enrichers;
    }

    public virtual async Task Handle(UserSignInAttemptEvent message)
    {
        if (!ShouldLog(message))
        {
            return;
        }

        var record = AbstractTypeFactory<UserSignInLog>.TryCreateInstance();

        record.CreatedDate = DateTime.UtcNow;
        record.UserName = message.UserName;
        record.UserId = message.UserId;
        record.Succeeded = message.Succeeded;
        record.FailureReason = message.FailureReason;
        record.SignInType = message.SignInType;
        record.Provider = message.Provider;
        record.OperatorUserId = message.OperatorUserId;
        record.OperatorUserName = message.OperatorUserName;
        record.ClientId = message.ClientId;
        record.SessionId = message.SessionId;
        record.StoreId = message.StoreId;
        record.MemberId = message.MemberId;

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is not null)
        {
            record.IpAddress = httpContext.Connection.RemoteIpAddress?.ToString();
            record.UserAgent = httpContext.Request.Headers.UserAgent.ToString().EmptyToNull();
        }

        await EnrichAsync(record);

        _writer.Write(record);
    }

    /// <summary>
    /// Impersonation is always logged, whatever the setting says: it is roughly ten rows a day and it
    /// is the compliance anchor. A switch that silently disables it will eventually be flipped.
    /// </summary>
    protected virtual bool ShouldLog(UserSignInAttemptEvent message)
    {
        if (message.SignInType is SignInType.Impersonation or SignInType.ImpersonationRevert)
        {
            return true;
        }

        return _settingsManager.GetValue<bool>(PlatformConstants.Settings.Security.SignInLogEnabled);
    }

    /// <summary>
    /// A broken enricher must never block a sign-in or lose a row — the record is written with
    /// whatever enrichment succeeded.
    /// </summary>
    protected virtual async Task EnrichAsync(UserSignInLog record)
    {
        foreach (var enricher in _enrichers.OrderBy(x => x.Priority))
        {
            try
            {
                await enricher.EnrichAsync(record);
            }
            catch
            {
                // Intentionally swallowed. The writer logs nothing here because the record still lands.
            }
        }
    }
}
```

- [ ] **Step 5: Register the handler**

In `src/VirtoCommerce.Platform.Web/Security/ApplicationBuilderExtensions.cs`, inside
`UseSecurityHandlers`, below the existing `LogChangesUserChangedEventHandler` block:

```csharp
            appBuilder.RegisterEventHandler<UserSignInAttemptEvent, LogUserSignInEventHandler>();
```

The two empty `UserLoginEvent` / `UserLogoutEvent` methods on
`LogChangesUserChangedEventHandler` stay exactly as they are — that handler keeps its
single responsibility.

- [ ] **Step 6: Run test to verify it passes**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests --filter "FullyQualifiedName~LogUserSignInEventHandlerTests"`
Expected: PASS — 5 tests (4 facts + 2 theory cases, minus overlap: 5 test cases).

- [ ] **Step 7: Commit**

```bash
git add src/VirtoCommerce.Platform.Core/Security/IUserSignInLogEnricher.cs src/VirtoCommerce.Platform.Security/Handlers/LogUserSignInEventHandler.cs src/VirtoCommerce.Platform.Web/Security/ApplicationBuilderExtensions.cs tests/VirtoCommerce.Platform.Tests/Security/LogUserSignInEventHandlerTests.cs
git commit -m "feat(security): add LogUserSignInEventHandler with module enrichment hook"
```

---

### Task 9: Capture point — SecurityController login and logout

**Files:**
- Modify: `src/VirtoCommerce.Platform.Web/Controllers/Api/SecurityController.cs:163-215`
- Test: `tests/VirtoCommerce.Platform.Web.Tests/Controllers/Api/SecurityControllerTests.cs`

- [ ] **Step 1: Write the failing test**

Append to the existing `SecurityControllerTests` class (reuse `SecurityMockHelper` from
`tests/VirtoCommerce.Platform.Web.Tests/Security/SecurityMockHelper.cs` for the
`UserManager` / `SignInManager` mocks — do not hand-roll new ones):

```csharp
    [Fact]
    public async Task Login_UnknownUser_PublishesFailedAttemptWithTypedUserName()
    {
        var published = new List<UserSignInAttemptEvent>();
        var controller = CreateControllerCapturingSignInAttempts(published, user: null);

        await controller.Login(new LoginRequest { UserName = "ghost@test.com", Password = "whatever" });

        var attempt = published.Should().ContainSingle().Subject;
        attempt.UserName.Should().Be("ghost@test.com");
        attempt.UserId.Should().BeNull();
        attempt.Succeeded.Should().BeFalse();
        attempt.FailureReason.Should().Be(SignInFailureReason.UserNotFound);
        attempt.SignInType.Should().Be(SignInType.Password);
    }

    [Fact]
    public async Task Login_WrongPassword_PublishesInvalidPasswordWithStoreAndMember()
    {
        var published = new List<UserSignInAttemptEvent>();
        var user = new ApplicationUser { Id = "user-1", UserName = "b2badmin@test.com", StoreId = "B2B-store", MemberId = "member-1" };
        var controller = CreateControllerCapturingSignInAttempts(published, user, signInResult: SignInResult.Failed);

        await controller.Login(new LoginRequest { UserName = "b2badmin@test.com", Password = "wrong" });

        var attempt = published.Should().ContainSingle().Subject;
        attempt.UserId.Should().Be("user-1");
        attempt.FailureReason.Should().Be(SignInFailureReason.InvalidPassword);
        attempt.StoreId.Should().Be("B2B-store");
        attempt.MemberId.Should().Be("member-1");
    }

    [Fact]
    public async Task Login_LockedOut_PublishesLockedOutReason()
    {
        var published = new List<UserSignInAttemptEvent>();
        var user = new ApplicationUser { Id = "user-1", UserName = "b2badmin@test.com" };
        var controller = CreateControllerCapturingSignInAttempts(published, user, signInResult: SignInResult.LockedOut);

        await controller.Login(new LoginRequest { UserName = "b2badmin@test.com", Password = "wrong" });

        published.Should().ContainSingle().Which.FailureReason.Should().Be(SignInFailureReason.LockedOut);
    }

    [Fact]
    public async Task Login_Success_PublishesSucceededAttempt()
    {
        var published = new List<UserSignInAttemptEvent>();
        var user = new ApplicationUser { Id = "user-1", UserName = "b2badmin@test.com" };
        var controller = CreateControllerCapturingSignInAttempts(published, user, signInResult: SignInResult.Success);

        await controller.Login(new LoginRequest { UserName = "b2badmin@test.com", Password = "right" });

        var attempt = published.Should().ContainSingle().Subject;
        attempt.Succeeded.Should().BeTrue();
        attempt.FailureReason.Should().BeNull();
    }
```

Add this helper to the same file:

```csharp
    private static SecurityController CreateControllerCapturingSignInAttempts(
        List<UserSignInAttemptEvent> published,
        ApplicationUser user,
        SignInResult signInResult = null)
    {
        var userManager = SecurityMockHelper.CreateUserManagerMock();
        userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
        userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        userManager.Setup(x => x.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(false);
        userManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

        var signInManager = SecurityMockHelper.CreateSignInManagerMock(userManager);
        signInManager.Setup(x => x.PasswordSignInAsync(
                It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(signInResult ?? SignInResult.Failed);

        var eventPublisher = new Mock<IEventPublisher>();
        eventPublisher.Setup(x => x.Publish(It.IsAny<UserSignInAttemptEvent>(), It.IsAny<CancellationToken>()))
            .Callback<UserSignInAttemptEvent, CancellationToken>((e, _) => published.Add(e))
            .Returns(Task.CompletedTask);
        eventPublisher.Setup(x => x.Publish(It.IsAny<IEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var controller = new SecurityController(
            signInManager.Object,
            roleManager: SecurityMockHelper.CreateRoleManagerMock().Object,
            permissionsProvider: Mock.Of<IPermissionsRegistrar>(),
            userSearchService: Mock.Of<IUserSearchService>(),
            roleSearchService: Mock.Of<IRoleSearchService>(),
            securityOptions: Options.Create(new PlatformOptions()),
            userOptionsExtended: Options.Create(new UserOptionsExtended()),
            passwordOptions: Options.Create(new PasswordOptionsExtended()),
            passwordLoginOptions: Options.Create(new PasswordLoginOptions { Enabled = true }),
            identityOptions: Options.Create(new IdentityOptions()),
            eventPublisher: eventPublisher.Object,
            userApiKeyService: Mock.Of<IUserApiKeyService>(),
            logger: NullLogger<SecurityController>.Instance,
            externalSigninProviderConfigs: [],
            userSessionsSearchService: Mock.Of<IUserSessionsSearchService>(),
            userSessionsService: Mock.Of<IUserSessionsService>(),
            adminUIAccessPolicy: Mock.Of<IAdminUIAccessPolicy>(),
            userSignInLogSearchService: Mock.Of<IUserSignInLogSearchService>());

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) },
        };

        return controller;
    }
```

If `SecurityMockHelper` lacks `CreateRoleManagerMock` or `CreateSignInManagerMock`, add
them there rather than inlining the plumbing in this test file — the helper exists so
these mocks are built one way across the suite.

**Ordering note:** the final `userSignInLogSearchService` argument is only added to
`SecurityController` in Task 15. When writing this helper during Task 9, omit that line —
then add it when Task 15 extends the constructor. More generally, the argument list above
must match `SecurityController`'s signature at the time you write it; follow the compiler
rather than this snippet.

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/VirtoCommerce.Platform.Web.Tests --filter "FullyQualifiedName~SecurityControllerTests"`
Expected: FAIL — no `UserSignInAttemptEvent` is published.

- [ ] **Step 3: Add the publish calls**

In `Login`, replace the unknown-user early return (currently lines 163-167):

```csharp
            if (user == null)
            {
                await PublishSignInAttempt(request.UserName, user: null, succeeded: false, SignInFailureReason.UserNotFound);
                await delayedResponse.FailAsync();
                return Ok(SignInResult.Failed);
            }
```

Replace the failed-login block (currently lines 173-177):

```csharp
            if (!loginResult.Succeeded)
            {
                await PublishSignInAttempt(request.UserName, user, succeeded: false, ToFailureReason(loginResult));
                await delayedResponse.FailAsync();
                return Ok(loginResult);
            }
```

After the existing `await _eventPublisher.Publish(new UserLoginEvent(user));` (line 180):

```csharp
            await PublishSignInAttempt(request.UserName, user, succeeded: true, failureReason: null);
```

In `Logout`, after the existing `await _eventPublisher.Publish(new UserLogoutEvent(user));`:

```csharp
                await PublishSignInAttempt(user.UserName, user, succeeded: true, failureReason: null, SignInType.Logout);
```

Add the two private helpers at the bottom of the controller, next to `SetLastLoginDate`:

```csharp
        private Task PublishSignInAttempt(
            string userName,
            ApplicationUser user,
            bool succeeded,
            string failureReason,
            string signInType = SignInType.Password)
        {
            return _eventPublisher.Publish(new UserSignInAttemptEvent
            {
                UserName = userName,
                UserId = user?.Id,
                Succeeded = succeeded,
                FailureReason = failureReason,
                SignInType = signInType,
                StoreId = user?.StoreId,
                MemberId = user?.MemberId,
                SessionId = User.FindFirstValue(Claims.Private.AuthorizationId),
            });
        }

        private static string ToFailureReason(SignInResult result)
        {
            if (result.IsLockedOut)
            {
                return SignInFailureReason.LockedOut;
            }

            if (result.IsNotAllowed)
            {
                return SignInFailureReason.NotAllowed;
            }

            if (result.RequiresTwoFactor)
            {
                return SignInFailureReason.RequiresTwoFactor;
            }

            return SignInFailureReason.InvalidPassword;
        }
```

Also add a publish to the `DuplicateEmailException` catch block (line 156) with
`SignInFailureReason.DuplicateEmail` and `user: null`.

**Timing:** every publish sits **before** `delayedResponse.FailAsync()` but the write
itself is buffered and non-blocking (Task 5), so both the user-found and user-not-found
branches do the same amount of synchronous work. Do not make this a synchronous insert —
that would reopen the user-enumeration timing side channel `DelayedResponse` exists to
close.

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test tests/VirtoCommerce.Platform.Web.Tests --filter "FullyQualifiedName~SecurityControllerTests"`
Expected: PASS.

- [ ] **Step 5: Commit**

```bash
git add src/VirtoCommerce.Platform.Web/Controllers/Api/SecurityController.cs tests/VirtoCommerce.Platform.Web.Tests/Controllers/Api/SecurityControllerTests.cs
git commit -m "feat(security): publish sign-in attempts from SecurityController login and logout"
```

---

### Task 10: Capture point — token endpoint password grant

**Files:**
- Modify: `src/VirtoCommerce.Platform.Web/Controllers/Api/AuthorizationController.cs:131-208`
- Test: `tests/VirtoCommerce.Platform.Web.Tests/Controllers/Api/AuthorizationControllerSignInLogTests.cs`

- [ ] **Step 1: Write the failing test**

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Controllers.Api;

public class AuthorizationControllerSignInLogTests
{
    [Fact]
    public async Task Exchange_PasswordGrant_UnknownUser_PublishesUserNotFound()
    {
        var published = new List<UserSignInAttemptEvent>();
        var controller = CreateControllerForPasswordGrant(published, user: null, userName: "ghost@test.com");

        await controller.Exchange();

        var attempt = published.Should().ContainSingle().Subject;
        attempt.UserName.Should().Be("ghost@test.com");
        attempt.UserId.Should().BeNull();
        attempt.Succeeded.Should().BeFalse();
        attempt.FailureReason.Should().Be(SignInFailureReason.UserNotFound);
        attempt.SignInType.Should().Be(SignInType.Password);
    }

    [Fact]
    public async Task Exchange_PasswordGrant_Success_PublishesSucceededAttemptWithClientId()
    {
        var published = new List<UserSignInAttemptEvent>();
        var user = new ApplicationUser { Id = "user-1", UserName = "b2badmin@test.com", StoreId = "B2B-store" };
        var controller = CreateControllerForPasswordGrant(published, user, userName: "b2badmin@test.com", clientId: "frontend");

        await controller.Exchange();

        var attempt = published.Should().ContainSingle().Subject;
        attempt.Succeeded.Should().BeTrue();
        attempt.ClientId.Should().Be("frontend");
        attempt.StoreId.Should().Be("B2B-store");
    }
}
```

Build `CreateControllerForPasswordGrant` with an `OpenIddictRequest` whose `GrantType`
is `password`, `Username` is `userName` and `ClientId` is `clientId`, pushed onto the
`HttpContext` OpenIddict feature; an `IEventPublisher` capturing `UserSignInAttemptEvent`;
and `UserManager` / `SignInManager` mocks from `SecurityMockHelper`.

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/VirtoCommerce.Platform.Web.Tests --filter "FullyQualifiedName~AuthorizationControllerSignInLogTests"`
Expected: FAIL — nothing published.

- [ ] **Step 3: Add the publish calls to the password grant branch**

Add the same `PublishSignInAttempt` / `ToFailureReason` helpers defined in Task 9 to
`AuthorizationController` (they are private to each controller; do not extract a shared
base class for two small methods), using `openIdConnectRequest.ClientId` for `ClientId`.

Then publish at each exit of the password-grant branch:

- `user is null` (line 152) → `SignInFailureReason.UserNotFound`, `user: null`
- `DuplicateEmailException` (line 145) → `SignInFailureReason.DuplicateEmail`, `user: null`
- `!_passwordLoginOptions.Enabled` (line 158) → `SignInFailureReason.PasswordLoginDisabled`
- `context.SignInResult` not succeeded, via the validator loop (line 180) → `ToFailureReason(context.SignInResult)`
- after `await _eventPublisher.Publish(new UserLoginEvent(user));` (line 203) → succeeded

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test tests/VirtoCommerce.Platform.Web.Tests --filter "FullyQualifiedName~AuthorizationControllerSignInLogTests"`
Expected: PASS — 2 tests.

- [ ] **Step 5: Commit**

```bash
git add src/VirtoCommerce.Platform.Web/Controllers/Api/AuthorizationController.cs tests/VirtoCommerce.Platform.Web.Tests/Controllers/Api/AuthorizationControllerSignInLogTests.cs
git commit -m "feat(security): publish sign-in attempts from the token endpoint password grant"
```

---

### Task 11: Capture point — impersonate grant

This is the task the whole feature exists for. The impersonate branch gains **only**
publish calls — no permission change, no target restriction, no `CanSignInAsync`, no
lifetime cap. See spec §3.

**Files:**
- Modify: `src/VirtoCommerce.Platform.Web/Controllers/Api/AuthorizationController.cs:309-378`
- Test: `tests/VirtoCommerce.Platform.Web.Tests/Controllers/Api/AuthorizationControllerSignInLogTests.cs`

- [ ] **Step 1: Write the failing test**

Append to `AuthorizationControllerSignInLogTests`:

```csharp
    [Fact]
    public async Task Exchange_ImpersonateGrant_PublishesImpersonationWithOperatorIdentity()
    {
        var published = new List<UserSignInAttemptEvent>();
        var operatorUser = new ApplicationUser { Id = "op-1", UserName = "support@virtocommerce.com" };
        var target = new ApplicationUser { Id = "user-1", UserName = "b2badmin@test.com", StoreId = "B2B-store", MemberId = "member-1" };
        var controller = CreateControllerForImpersonateGrant(published, operatorUser, target, targetUserId: "user-1", permitted: true);

        await controller.Exchange();

        var attempt = published.Should().ContainSingle().Subject;
        attempt.SignInType.Should().Be(SignInType.Impersonation);
        attempt.Succeeded.Should().BeTrue();
        attempt.UserId.Should().Be("user-1");
        attempt.UserName.Should().Be("b2badmin@test.com");
        attempt.OperatorUserId.Should().Be("op-1");
        attempt.OperatorUserName.Should().Be("support@virtocommerce.com");
        attempt.StoreId.Should().Be("B2B-store");
        attempt.MemberId.Should().Be("member-1");
    }

    [Fact]
    public async Task Exchange_ImpersonateGrant_WithoutPermission_PublishesForbidden()
    {
        var published = new List<UserSignInAttemptEvent>();
        var operatorUser = new ApplicationUser { Id = "op-1", UserName = "nosy@test.com" };
        var controller = CreateControllerForImpersonateGrant(published, operatorUser, target: null, targetUserId: "user-1", permitted: false);

        await controller.Exchange();

        var attempt = published.Should().ContainSingle().Subject;
        attempt.Succeeded.Should().BeFalse();
        attempt.FailureReason.Should().Be(SignInFailureReason.Forbidden);
        attempt.OperatorUserName.Should().Be("nosy@test.com");
    }

    [Fact]
    public async Task Exchange_ImpersonateGrant_NoUserId_PublishesRevert()
    {
        var published = new List<UserSignInAttemptEvent>();
        var operatorUser = new ApplicationUser { Id = "op-1", UserName = "support@virtocommerce.com" };
        var controller = CreateControllerForImpersonateGrant(published, operatorUser, target: operatorUser, targetUserId: null, permitted: true);

        await controller.Exchange();

        published.Should().ContainSingle().Which.SignInType.Should().Be(SignInType.ImpersonationRevert);
    }
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/VirtoCommerce.Platform.Web.Tests --filter "FullyQualifiedName~AuthorizationControllerSignInLogTests"`
Expected: FAIL — the three new tests publish nothing.

- [ ] **Step 3: Add the publish calls**

In the impersonate branch, inside the `if (!loginOnBehalfAuthResult.Succeeded)` block
(line 323), before `return Forbid();`:

```csharp
                        await _eventPublisher.Publish(new UserSignInAttemptEvent
                        {
                            UserName = (string)openIdConnectRequest.GetParameter("user_id"),
                            Succeeded = false,
                            FailureReason = SignInFailureReason.Forbidden,
                            SignInType = SignInType.Impersonation,
                            OperatorUserId = user.Id,
                            OperatorUserName = user.UserName,
                            ClientId = openIdConnectRequest.ClientId,
                        });
```

After `if (impersonatedUser == null)` returns `BadRequest` (line 351), publish a
`SignInFailureReason.UserNotFound` attempt with `SignInType.Impersonation` and the
operator identity resolved above.

After the ticket is created and before `return SignIn(...)` (line 377):

```csharp
                await _eventPublisher.Publish(new UserSignInAttemptEvent
                {
                    UserName = impersonatedUser.UserName,
                    UserId = impersonatedUser.Id,
                    Succeeded = true,
                    // An empty operatorUserId means this call reverted the operator back to themselves.
                    SignInType = string.IsNullOrEmpty(operatorUserId) ? SignInType.ImpersonationRevert : SignInType.Impersonation,
                    OperatorUserId = operatorUserId.EmptyToNull() ?? user.Id,
                    OperatorUserName = operatorUserName.EmptyToNull() ?? user.UserName,
                    ClientId = openIdConnectRequest.ClientId,
                    StoreId = impersonatedUser.StoreId,
                    MemberId = impersonatedUser.MemberId,
                    SessionId = ticket.Principal.FindFirstValue(Claims.Private.AuthorizationId),
                });
```

- [ ] **Step 4: Write the failing test for impersonation session extension**

Ordinary refresh-token rotation is deliberately **not** logged — it is high volume and low
value. But a refresh that carries operator claims is how an impersonation session extends
itself, and that is worth a row (spec §4.3).

```csharp
    [Fact]
    public async Task Exchange_RefreshToken_WithOperatorClaims_PublishesImpersonation()
    {
        var published = new List<UserSignInAttemptEvent>();
        var user = new ApplicationUser { Id = "user-1", UserName = "b2badmin@test.com" };
        var controller = CreateControllerForRefreshGrant(published, user,
            operatorUserId: "op-1", operatorUserName: "support@virtocommerce.com");

        await controller.Exchange();

        var attempt = published.Should().ContainSingle().Subject;
        attempt.SignInType.Should().Be(SignInType.Impersonation);
        attempt.OperatorUserName.Should().Be("support@virtocommerce.com");
    }

    [Fact]
    public async Task Exchange_RefreshToken_WithoutOperatorClaims_PublishesNothing()
    {
        var published = new List<UserSignInAttemptEvent>();
        var user = new ApplicationUser { Id = "user-1", UserName = "b2badmin@test.com" };
        var controller = CreateControllerForRefreshGrant(published, user,
            operatorUserId: null, operatorUserName: null);

        await controller.Exchange();

        published.Should().BeEmpty();
    }
```

- [ ] **Step 5: Add the conditional publish to the refresh branch**

In the refresh-token branch, after the three `CopyClaim` calls (lines 250-252) and before
`return SignIn(...)`:

```csharp
                var refreshedOperatorUserId = info.Principal.FindFirstValue(PlatformConstants.Security.Claims.OperatorUserId).EmptyToNull();
                if (refreshedOperatorUserId is not null)
                {
                    await _eventPublisher.Publish(new UserSignInAttemptEvent
                    {
                        UserName = user.UserName,
                        UserId = user.Id,
                        Succeeded = true,
                        SignInType = SignInType.Impersonation,
                        OperatorUserId = refreshedOperatorUserId,
                        OperatorUserName = info.Principal.FindFirstValue(PlatformConstants.Security.Claims.OperatorUserName).EmptyToNull(),
                        ClientId = openIdConnectRequest.ClientId,
                        StoreId = user.StoreId,
                        MemberId = user.MemberId,
                    });
                }
```

- [ ] **Step 6: Run test to verify it passes**

Run: `dotnet test tests/VirtoCommerce.Platform.Web.Tests --filter "FullyQualifiedName~AuthorizationControllerSignInLogTests"`
Expected: PASS — 7 tests.

- [ ] **Step 7: Commit**

```bash
git add src/VirtoCommerce.Platform.Web/Controllers/Api/AuthorizationController.cs tests/VirtoCommerce.Platform.Web.Tests/Controllers/Api/AuthorizationControllerSignInLogTests.cs
git commit -m "feat(security): audit login-on-behalf grant, revert, denial and session extension"
```

---

### Task 12: Capture point — external sign-in

**Files:**
- Modify: `src/VirtoCommerce.Platform.Web/Security/ExternalSignInService.cs:66-86`
- Test: `tests/VirtoCommerce.Platform.Web.Tests/Security/ExternalSignInServiceSignInLogTests.cs`

- [ ] **Step 1: Write the failing test**

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Security;

public class ExternalSignInServiceSignInLogTests
{
    [Fact]
    public async Task SignInAsync_Success_PublishesExternalAttemptWithProvider()
    {
        var published = new List<UserSignInAttemptEvent>();
        var service = CreateService(published, succeeds: true, provider: "AzureAD",
            user: new ApplicationUser { Id = "user-1", UserName = "b2badmin@test.com", StoreId = "B2B-store" });

        await service.SignInAsync();

        var attempt = published.Should().ContainSingle().Subject;
        attempt.SignInType.Should().Be(SignInType.External);
        attempt.Provider.Should().Be("AzureAD");
        attempt.Succeeded.Should().BeTrue();
        attempt.UserId.Should().Be("user-1");
    }

    [Fact]
    public async Task SignInAsync_Failure_PublishesNotAllowed()
    {
        var published = new List<UserSignInAttemptEvent>();
        var service = CreateService(published, succeeds: false, provider: "AzureAD", user: null);

        await service.SignInAsync();

        var attempt = published.Should().ContainSingle().Subject;
        attempt.Succeeded.Should().BeFalse();
        attempt.FailureReason.Should().Be(SignInFailureReason.NotAllowed);
        attempt.Provider.Should().Be("AzureAD");
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/VirtoCommerce.Platform.Web.Tests --filter "FullyQualifiedName~ExternalSignInServiceSignInLogTests"`
Expected: FAIL — nothing published.

- [ ] **Step 3: Add the publish calls**

After the existing `await _eventPublisher.Publish(new UserLoginEvent(platformUser, externalLoginInfo));`
(line 86):

```csharp
            await _eventPublisher.Publish(new UserSignInAttemptEvent
            {
                UserName = platformUser.UserName,
                UserId = platformUser.Id,
                Succeeded = true,
                SignInType = SignInType.External,
                Provider = externalLoginInfo.LoginProvider,
                StoreId = platformUser.StoreId,
                MemberId = platformUser.MemberId,
            });
```

On each failure path that returns an unsuccessful result, publish the same event with
`Succeeded = false`, `FailureReason = SignInFailureReason.NotAllowed`, `UserId = null`
when no platform user was resolved, and the provider name.

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test tests/VirtoCommerce.Platform.Web.Tests --filter "FullyQualifiedName~ExternalSignInServiceSignInLogTests"`
Expected: PASS — 2 tests.

- [ ] **Step 5: Run the whole suite and commit**

```bash
dotnet test tests/VirtoCommerce.Platform.Tests tests/VirtoCommerce.Platform.Web.Tests
git add src/VirtoCommerce.Platform.Web/Security/ExternalSignInService.cs tests/VirtoCommerce.Platform.Web.Tests/Security/ExternalSignInServiceSignInLogTests.cs
git commit -m "feat(security): publish sign-in attempts from external sign-in"
```

Expected: all tests pass.

> **Phase 1 checkpoint.** The trail now exists end to end. Stop here for review before Phase 2.

---

## Phase 2 — Retention

### Task 13: Cleanup job

`IUserSignInLogEnricher` was already created in Task 8 (the handler needed it to compile);
no separate task is required for it. The platform ships no implementation — vc-module-customer
registers one to fill `OrganizationId` / `OrganizationName` / `StoreName`.

**Files:**
- Create: `src/VirtoCommerce.Platform.Web/Security/BackgroundJobs/SignInLogCleanupJob.cs`
- Create: `src/VirtoCommerce.Platform.Web/Security/BackgroundJobs/SignInLogCleanupJobPayload.cs`
- Modify: `src/VirtoCommerce.Platform.Web/Startup.cs:551`
- Test: `tests/VirtoCommerce.Platform.Web.Tests/Security/SignInLogCleanupJobTests.cs`

- [ ] **Step 1: Write the failing test**

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Web.Security.BackgroundJobs;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Security;

public class SignInLogCleanupJobTests
{
    [Fact]
    public async Task Process_DeletesUsingTheConfiguredRetentionWindow()
    {
        DateTime? cutoff = null;
        var service = new Mock<IUserSignInLogService>();
        service.Setup(x => x.DeleteOlderThanAsync(It.IsAny<DateTime>(), It.IsAny<int>()))
            .Callback<DateTime, int>((c, _) => cutoff = c)
            .ReturnsAsync(0);

        var settings = new Mock<ISettingsManager>();
        settings.Setup(x => x.GetValue<int>(PlatformConstants.Settings.Security.SignInLogRetentionDays)).Returns(30);

        await new SignInLogCleanupJob(service.Object, settings.Object).Process();

        cutoff.Should().BeCloseTo(DateTime.UtcNow.AddDays(-30), TimeSpan.FromMinutes(1));
    }

    [Fact]
    public async Task Process_KeepsDeletingUntilABatchComesBackShort()
    {
        var service = new Mock<IUserSignInLogService>();
        service.SetupSequence(x => x.DeleteOlderThanAsync(It.IsAny<DateTime>(), It.IsAny<int>()))
            .ReturnsAsync(1000)
            .ReturnsAsync(1000)
            .ReturnsAsync(7);

        var settings = new Mock<ISettingsManager>();
        settings.Setup(x => x.GetValue<int>(PlatformConstants.Settings.Security.SignInLogRetentionDays)).Returns(90);

        await new SignInLogCleanupJob(service.Object, settings.Object).Process();

        service.Verify(x => x.DeleteOlderThanAsync(It.IsAny<DateTime>(), It.IsAny<int>()), Times.Exactly(3));
    }

    [Fact]
    public async Task Process_RetentionDaysZeroOrLess_DeletesNothing()
    {
        var service = new Mock<IUserSignInLogService>();
        var settings = new Mock<ISettingsManager>();
        settings.Setup(x => x.GetValue<int>(PlatformConstants.Settings.Security.SignInLogRetentionDays)).Returns(0);

        await new SignInLogCleanupJob(service.Object, settings.Object).Process();

        // A misconfigured retention of 0 must not be read as "delete everything".
        service.Verify(x => x.DeleteOlderThanAsync(It.IsAny<DateTime>(), It.IsAny<int>()), Times.Never);
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/VirtoCommerce.Platform.Web.Tests --filter "FullyQualifiedName~SignInLogCleanupJobTests"`
Expected: FAIL — `SignInLogCleanupJob` does not exist.

- [ ] **Step 3: Create the payload**

```csharp
namespace VirtoCommerce.Platform.Web.Security.BackgroundJobs;

/// <summary>
/// Payload for the recurring sign-in log cleanup job. Carries no data — the schedule and the
/// retention setting are the only inputs — but exists so the job runs through the
/// engine-agnostic message-based background-job pipeline.
/// </summary>
public sealed class SignInLogCleanupJobPayload
{
}
```

- [ ] **Step 4: Implement the job**

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Jobs;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Web.Security.BackgroundJobs;

/// <summary>
/// Trims the sign-in audit log to the configured retention window. Not optional: a store at
/// 50k logins/day produces roughly 18M rows a year.
/// </summary>
public class SignInLogCleanupJob : IBackgroundJobHandler<SignInLogCleanupJobPayload>
{
    private const int BatchSize = 1000;

    private readonly IUserSignInLogService _service;
    private readonly ISettingsManager _settingsManager;

    public SignInLogCleanupJob(IUserSignInLogService service, ISettingsManager settingsManager)
    {
        _service = service;
        _settingsManager = settingsManager;
    }

    public async Task Process()
    {
        var retentionDays = _settingsManager.GetValue<int>(PlatformConstants.Settings.Security.SignInLogRetentionDays);

        // A misconfigured 0 must not be read as "delete everything".
        if (retentionDays <= 0)
        {
            return;
        }

        var cutoff = DateTime.UtcNow.AddDays(-retentionDays);

        int deleted;
        do
        {
            deleted = await _service.DeleteOlderThanAsync(cutoff, BatchSize);
        }
        while (deleted == BatchSize);
    }

    public Task Execute(SignInLogCleanupJobPayload payload, IJobExecutionContext context, CancellationToken cancellationToken = default)
        => Process();
}
```

- [ ] **Step 5: Register the schedule**

In `Startup.cs`, directly after the `PruneExpiredTokensJob` registration (line 551):

```csharp
            services.AddRecurringJob<SignInLogCleanupJob, SignInLogCleanupJobPayload>(schedule => schedule
                .WithId("SignInLogCleanupJob")
                .FromSettings(
                    PlatformConstants.Settings.Security.EnableSignInLogCleanupJob,
                    PlatformConstants.Settings.Security.CronSignInLogCleanupJob));
```

- [ ] **Step 6: Run test to verify it passes**

Run: `dotnet test tests/VirtoCommerce.Platform.Web.Tests --filter "FullyQualifiedName~SignInLogCleanupJobTests"`
Expected: PASS — 3 tests.

- [ ] **Step 7: Commit**

```bash
git add src/VirtoCommerce.Platform.Web/Security/BackgroundJobs/SignInLogCleanupJob.cs src/VirtoCommerce.Platform.Web/Security/BackgroundJobs/SignInLogCleanupJobPayload.cs src/VirtoCommerce.Platform.Web/Startup.cs tests/VirtoCommerce.Platform.Web.Tests/Security/SignInLogCleanupJobTests.cs
git commit -m "feat(security): add cron-scheduled sign-in log retention job"
```

> **Phase 2 checkpoint.** Stop for review before Phase 3.

---

## Phase 3 — Surfacing

### Task 14: Search contracts and service

**Files:**
- Create: `src/VirtoCommerce.Platform.Core/Security/Search/UserSignInLogSearchCriteria.cs`
- Create: `src/VirtoCommerce.Platform.Core/Security/Search/UserSignInLogSearchResult.cs`
- Create: `src/VirtoCommerce.Platform.Core/Security/Search/IUserSignInLogSearchService.cs`
- Create: `src/VirtoCommerce.Platform.Security/Services/UserSignInLogSearchService.cs`
- Modify: `src/VirtoCommerce.Platform.Web/Security/ServiceCollectionExtensions.cs`
- Test: `tests/VirtoCommerce.Platform.Tests/Security/UserSignInLogSearchServiceTests.cs`

- [ ] **Step 1: Write the failing test**

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;
using VirtoCommerce.Platform.Security.Services;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security;

public class UserSignInLogSearchServiceTests
{
    private static readonly DateTime _now = new(2026, 9, 15, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public async Task SearchAsync_FiltersByUserId()
    {
        var service = CreateService(
            Row("r1", userId: "user-1"),
            Row("r2", userId: "user-2"));

        var result = await service.SearchAsync(new UserSignInLogSearchCriteria { UserId = "user-1", Take = 20 });

        result.TotalCount.Should().Be(1);
        result.Results.Should().ContainSingle().Which.Id.Should().Be("r1");
    }

    [Fact]
    public async Task SearchAsync_FiltersBySucceeded()
    {
        var service = CreateService(
            Row("ok", succeeded: true),
            Row("bad", succeeded: false));

        var result = await service.SearchAsync(new UserSignInLogSearchCriteria { Succeeded = false, Take = 20 });

        result.Results.Should().ContainSingle().Which.Id.Should().Be("bad");
    }

    [Fact]
    public async Task SearchAsync_FiltersBySignInTypeAndDateRange()
    {
        var service = CreateService(
            Row("recent", signInType: SignInType.Impersonation, createdDate: _now.AddHours(-1)),
            Row("old", signInType: SignInType.Impersonation, createdDate: _now.AddDays(-10)),
            Row("other", signInType: SignInType.Password, createdDate: _now.AddHours(-1)));

        var result = await service.SearchAsync(new UserSignInLogSearchCriteria
        {
            SignInTypes = [SignInType.Impersonation],
            StartDate = _now.AddDays(-1),
            Take = 20,
        });

        result.Results.Should().ContainSingle().Which.Id.Should().Be("recent");
    }

    [Fact]
    public async Task SearchAsync_DefaultsToNewestFirst()
    {
        var service = CreateService(
            Row("older", createdDate: _now.AddHours(-5)),
            Row("newest", createdDate: _now.AddHours(-1)));

        var result = await service.SearchAsync(new UserSignInLogSearchCriteria { Take = 20 });

        result.Results.First().Id.Should().Be("newest");
    }

    [Fact]
    public async Task GetStatsAsync_CountsTotalsAndTopFailingIps()
    {
        var service = CreateService(
            Row("a", succeeded: true, userId: "user-1", createdDate: _now.AddHours(-1)),
            Row("b", succeeded: false, ip: "203.0.113.7", createdDate: _now.AddHours(-1)),
            Row("c", succeeded: false, ip: "203.0.113.7", createdDate: _now.AddHours(-2)),
            Row("d", succeeded: false, ip: "198.51.100.1", createdDate: _now.AddHours(-3)),
            Row("e", succeeded: true, signInType: SignInType.Impersonation, createdDate: _now.AddHours(-1)));

        var stats = await service.GetStatsAsync(new UserSignInLogSearchCriteria { StartDate = _now.AddDays(-1) });

        stats.TotalCount.Should().Be(5);
        stats.FailedCount.Should().Be(3);
        stats.ImpersonationCount.Should().Be(1);
        stats.TopFailedIpAddresses.First().Key.Should().Be("203.0.113.7");
        stats.TopFailedIpAddresses.First().Value.Should().Be(2);
    }

    private static UserSignInLogEntity Row(
        string id,
        string userId = null,
        bool succeeded = true,
        string signInType = SignInType.Password,
        string ip = null,
        DateTime? createdDate = null)
        => new()
        {
            Id = id,
            UserId = userId,
            UserName = userId ?? "anonymous",
            Succeeded = succeeded,
            SignInType = signInType,
            IpAddress = ip,
            CreatedDate = createdDate ?? _now,
        };

    private static UserSignInLogSearchService CreateService(params UserSignInLogEntity[] rows)
    {
        var repository = new Mock<ISecurityRepository>();
        repository.Setup(x => x.UserSignInLogs).Returns(rows.AsQueryable().BuildMock());
        return new UserSignInLogSearchService(() => repository.Object);
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests --filter "FullyQualifiedName~UserSignInLogSearchServiceTests"`
Expected: FAIL — types do not exist.

- [ ] **Step 3: Create the contracts**

`UserSignInLogSearchCriteria.cs`:

```csharp
using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security.Search;

public class UserSignInLogSearchCriteria : SearchCriteriaBase
{
    public string UserId { get; set; }
    public bool? Succeeded { get; set; }
    public string[] SignInTypes { get; set; }
    public string[] FailureReasons { get; set; }
    public string IpAddress { get; set; }
    public string StoreId { get; set; }
    public string OrganizationId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
```

`UserSignInLogSearchResult.cs`:

```csharp
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security.Search;

public class UserSignInLogSearchResult : GenericSearchResult<UserSignInLog>
{
}
```

`UserSignInLogStats.cs`:

```csharp
using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Security.Search;

/// <summary>
/// Aggregates for the sign-in log blade. Every member is produced by a SQL aggregate against an
/// indexed column — never by materialising rows.
/// </summary>
public class UserSignInLogStats
{
    public int TotalCount { get; set; }
    public int FailedCount { get; set; }
    public int DistinctUserCount { get; set; }
    public int ImpersonationCount { get; set; }

    public IList<KeyValuePair<string, int>> TopFailedIpAddresses { get; set; } = [];
    public IList<KeyValuePair<string, int>> TopFailedUserNames { get; set; } = [];
    public IList<KeyValuePair<string, int>> FailureReasonBreakdown { get; set; } = [];
    public IList<KeyValuePair<string, int>> SignInsByOrganization { get; set; } = [];
}
```

`IUserSignInLogSearchService.cs`:

```csharp
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.Platform.Core.Security.Search;

public interface IUserSignInLogSearchService : ISearchService<UserSignInLogSearchCriteria, UserSignInLogSearchResult, UserSignInLog>
{
    Task<UserSignInLogStats> GetStatsAsync(UserSignInLogSearchCriteria criteria);
}
```

- [ ] **Step 4: Implement the service**

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;

namespace VirtoCommerce.Platform.Security.Services;

public class UserSignInLogSearchService : IUserSignInLogSearchService
{
    private const int TopN = 10;

    private readonly Func<ISecurityRepository> _repositoryFactory;

    public UserSignInLogSearchService(Func<ISecurityRepository> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public virtual async Task<UserSignInLogSearchResult> SearchAsync(UserSignInLogSearchCriteria criteria, bool clone = true)
    {
        var result = AbstractTypeFactory<UserSignInLogSearchResult>.TryCreateInstance();

        using var repository = _repositoryFactory();

        var query = BuildQuery(repository, criteria);

        result.TotalCount = await query.CountAsync();

        if (criteria.Take > 0)
        {
            var entities = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip(criteria.Skip)
                .Take(criteria.Take)
                .ToListAsync();

            result.Results = entities
                .Select(x => x.ToModel(AbstractTypeFactory<UserSignInLog>.TryCreateInstance()))
                .ToList();
        }

        return result;
    }

    public virtual async Task<UserSignInLogStats> GetStatsAsync(UserSignInLogSearchCriteria criteria)
    {
        using var repository = _repositoryFactory();

        var query = BuildQuery(repository, criteria);
        var failed = query.Where(x => !x.Succeeded);

        return new UserSignInLogStats
        {
            TotalCount = await query.CountAsync(),
            FailedCount = await failed.CountAsync(),
            DistinctUserCount = await query.Where(x => x.UserId != null).Select(x => x.UserId).Distinct().CountAsync(),
            ImpersonationCount = await query.CountAsync(x => x.SignInType == SignInType.Impersonation),
            TopFailedIpAddresses = await TopAsync(failed.Where(x => x.IpAddress != null), x => x.IpAddress),
            TopFailedUserNames = await TopAsync(failed, x => x.UserName),
            FailureReasonBreakdown = await TopAsync(failed.Where(x => x.FailureReason != null), x => x.FailureReason),
            SignInsByOrganization = await TopAsync(query.Where(x => x.OrganizationName != null), x => x.OrganizationName),
        };
    }

    protected virtual IQueryable<UserSignInLogEntity> BuildQuery(ISecurityRepository repository, UserSignInLogSearchCriteria criteria)
    {
        var query = repository.UserSignInLogs;

        if (!string.IsNullOrEmpty(criteria.UserId))
        {
            query = query.Where(x => x.UserId == criteria.UserId);
        }

        if (criteria.Succeeded != null)
        {
            query = query.Where(x => x.Succeeded == criteria.Succeeded);
        }

        if (!criteria.SignInTypes.IsNullOrEmpty())
        {
            query = query.Where(x => criteria.SignInTypes.Contains(x.SignInType));
        }

        if (!criteria.FailureReasons.IsNullOrEmpty())
        {
            query = query.Where(x => criteria.FailureReasons.Contains(x.FailureReason));
        }

        if (!string.IsNullOrEmpty(criteria.IpAddress))
        {
            query = query.Where(x => x.IpAddress == criteria.IpAddress);
        }

        if (!string.IsNullOrEmpty(criteria.StoreId))
        {
            query = query.Where(x => x.StoreId == criteria.StoreId);
        }

        if (!string.IsNullOrEmpty(criteria.OrganizationId))
        {
            query = query.Where(x => x.OrganizationId == criteria.OrganizationId);
        }

        if (criteria.StartDate != null)
        {
            query = query.Where(x => x.CreatedDate >= criteria.StartDate);
        }

        if (criteria.EndDate != null)
        {
            query = query.Where(x => x.CreatedDate <= criteria.EndDate);
        }

        if (!string.IsNullOrEmpty(criteria.Keyword))
        {
            query = query.Where(x => x.UserName.Contains(criteria.Keyword) || x.IpAddress.Contains(criteria.Keyword));
        }

        return query;
    }

    private static async Task<IList<KeyValuePair<string, int>>> TopAsync(
        IQueryable<UserSignInLogEntity> query,
        System.Linq.Expressions.Expression<Func<UserSignInLogEntity, string>> selector)
    {
        var grouped = await query
            .GroupBy(selector)
            .Select(g => new { g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(TopN)
            .ToListAsync();

        return grouped.Select(x => new KeyValuePair<string, int>(x.Key, x.Count)).ToList();
    }
}
```

- [ ] **Step 5: Register it**

In `ServiceCollectionExtensions.cs`:

```csharp
            services.AddSingleton<IUserSignInLogSearchService, UserSignInLogSearchService>();
```

- [ ] **Step 6: Run test to verify it passes**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests --filter "FullyQualifiedName~UserSignInLogSearchServiceTests"`
Expected: PASS — 5 tests.

- [ ] **Step 7: Commit**

```bash
git add src/VirtoCommerce.Platform.Core/Security/Search/ src/VirtoCommerce.Platform.Security/Services/UserSignInLogSearchService.cs src/VirtoCommerce.Platform.Web/Security/ServiceCollectionExtensions.cs tests/VirtoCommerce.Platform.Tests/Security/UserSignInLogSearchServiceTests.cs
git commit -m "feat(security): add sign-in log search service with statistics aggregates"
```

---

### Task 15: Permission and API endpoints

**Files:**
- Modify: `src/VirtoCommerce.Platform.Core/PlatformConstants.cs:92,121`
- Modify: `src/VirtoCommerce.Platform.Web/Controllers/Api/SecurityController.cs:105`
- Test: `tests/VirtoCommerce.Platform.Web.Tests/Controllers/Api/SecurityControllerTests.cs`

- [ ] **Step 1: Add the permission**

In `PlatformConstants.Security.Permissions`, next to `SecurityRevokeToken` (line 97):

```csharp
                public const string SecuritySignInLogRead = "platform:security:sign_in_log:read";
```

Add it to `AllPermissions` (line 121), on the same line as the other security permissions:

```csharp
                    SecurityLoginOnBehalf, SecurityVerifyEmail, SecurityConfirmEmail, SecurityGenerateToken, SecurityVerifyToken, SecurityRevokeToken, SecuritySignInLogRead,
```

- [ ] **Step 2: Write the failing test**

```csharp
    [Fact]
    public async Task SearchSignInLog_PassesCriteriaThroughAndReturnsResult()
    {
        var searchService = new Mock<IUserSignInLogSearchService>();
        searchService.Setup(x => x.SearchAsync(It.IsAny<UserSignInLogSearchCriteria>(), true))
            .ReturnsAsync(new UserSignInLogSearchResult { TotalCount = 3 });

        var controller = CreateControllerWithSignInLogSearch(searchService.Object);

        var response = await controller.SearchSignInLog(new UserSignInLogSearchCriteria { Take = 20 });

        response.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<UserSignInLogSearchResult>()
            .Which.TotalCount.Should().Be(3);
    }

    [Fact]
    public void SearchSignInLog_IsGuardedByTheSignInLogPermission()
    {
        var attribute = typeof(SecurityController)
            .GetMethod(nameof(SecurityController.SearchSignInLog))!
            .GetCustomAttributes(typeof(AuthorizeAttribute), inherit: false)
            .Cast<AuthorizeAttribute>()
            .Single();

        attribute.Policy.Should().Be(PlatformConstants.Security.Permissions.SecuritySignInLogRead);
    }
```

- [ ] **Step 3: Run test to verify it fails**

Run: `dotnet test tests/VirtoCommerce.Platform.Web.Tests --filter "FullyQualifiedName~SecurityControllerTests"`
Expected: FAIL — `SearchSignInLog` does not exist.

- [ ] **Step 4: Add the endpoints**

Inject `IUserSignInLogSearchService _userSignInLogSearchService` into `SecurityController`
(constructor parameter + field, following the existing `_userSessionsSearchService` pattern),
then add below the existing session endpoints (after line 125):

```csharp
        [HttpPost]
        [Route("sign-in-log/search")]
        [Authorize(PlatformConstants.Security.Permissions.SecuritySignInLogRead)]
        public async Task<ActionResult<UserSignInLogSearchResult>> SearchSignInLog([FromBody] UserSignInLogSearchCriteria criteria)
        {
            var result = await _userSignInLogSearchService.SearchAsync(criteria);
            return Ok(result);
        }

        [HttpPost]
        [Route("sign-in-log/stats")]
        [Authorize(PlatformConstants.Security.Permissions.SecuritySignInLogRead)]
        public async Task<ActionResult<UserSignInLogStats>> GetSignInLogStats([FromBody] UserSignInLogSearchCriteria criteria)
        {
            var result = await _userSignInLogSearchService.GetStatsAsync(criteria);
            return Ok(result);
        }
```

- [ ] **Step 5: Run test to verify it passes**

Run: `dotnet test tests/VirtoCommerce.Platform.Web.Tests --filter "FullyQualifiedName~SecurityControllerTests"`
Expected: PASS.

- [ ] **Step 6: Commit**

```bash
git add src/VirtoCommerce.Platform.Core/PlatformConstants.cs src/VirtoCommerce.Platform.Web/Controllers/Api/SecurityController.cs tests/VirtoCommerce.Platform.Web.Tests/Controllers/Api/SecurityControllerTests.cs
git commit -m "feat(security): add sign-in log search and stats endpoints with permission"
```

---

### Task 16: Active Sessions impersonation columns

**Files:**
- Modify: `src/VirtoCommerce.Platform.Core/Security/UserSession.cs`
- Modify: `src/VirtoCommerce.Platform.Security/Services/UserSessionsSearchService.cs:43-51`
- Test: `tests/VirtoCommerce.Platform.Tests/Security/UserSessionsSearchServiceTests.cs`

- [ ] **Step 1: Write the failing test**

```csharp
    [Fact]
    public async Task SearchAsync_ImpersonatedSession_ExposesOperatorIdentity()
    {
        // A token whose payload carries vc_operator_name must surface as an impersonated session,
        // so the Terminate command becomes a kill switch for a live login-on-behalf session.
        var service = CreateServiceWithToken(operatorUserId: "op-1", operatorUserName: "support@virtocommerce.com");

        var result = await service.SearchAsync(new UserSessionSearchCriteria { UserId = "user-1", Take = 20 });

        var session = result.Results.Should().ContainSingle().Subject;
        session.IsImpersonated.Should().BeTrue();
        session.OperatorUserName.Should().Be("support@virtocommerce.com");
    }

    [Fact]
    public async Task SearchAsync_OrdinarySession_IsNotMarkedImpersonated()
    {
        var service = CreateServiceWithToken(operatorUserId: null, operatorUserName: null);

        var result = await service.SearchAsync(new UserSessionSearchCriteria { UserId = "user-1", Take = 20 });

        result.Results.Should().ContainSingle().Which.IsImpersonated.Should().BeFalse();
    }
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests --filter "FullyQualifiedName~UserSessionsSearchServiceTests"`
Expected: FAIL — `IsImpersonated` does not exist.

- [ ] **Step 3: Extend the model**

In `UserSession.cs`:

```csharp
    /// <summary>True when this session was created by the login-on-behalf grant.</summary>
    public bool IsImpersonated { get; set; }

    /// <summary>The operator acting on behalf of the user, when <see cref="IsImpersonated"/>.</summary>
    public string OperatorUserId { get; set; }

    public string OperatorUserName { get; set; }
```

- [ ] **Step 4: Populate them**

In `UserSessionsSearchService.SearchAsync`, inside the `foreach (var token in tokensPage)`
loop, after `userSession.ExpirationDate = ...`:

```csharp
                var principal = await _tokenManager.GetPrincipalAsync(token);
                userSession.OperatorUserId = principal?.GetClaim(PlatformConstants.Security.Claims.OperatorUserId).EmptyToNull();
                userSession.OperatorUserName = principal?.GetClaim(PlatformConstants.Security.Claims.OperatorUserName).EmptyToNull();
                userSession.IsImpersonated = userSession.OperatorUserId != null;
```

- [ ] **Step 5: Run test to verify it passes**

Run: `dotnet test tests/VirtoCommerce.Platform.Tests --filter "FullyQualifiedName~UserSessionsSearchServiceTests"`
Expected: PASS.

- [ ] **Step 6: Commit**

```bash
git add src/VirtoCommerce.Platform.Core/Security/UserSession.cs src/VirtoCommerce.Platform.Security/Services/UserSessionsSearchService.cs tests/VirtoCommerce.Platform.Tests/Security/UserSessionsSearchServiceTests.cs
git commit -m "feat(security): surface impersonation and operator identity on active sessions"
```

---

### Task 17: Admin SPA — resource, widget, blade

**Files:**
- Modify: `src/VirtoCommerce.Platform.Web/wwwroot/js/app/security/resources/accounts.js:19`
- Create: `src/VirtoCommerce.Platform.Web/wwwroot/js/app/security/widgets/accountSignInLogWidget.js`
- Create: `src/VirtoCommerce.Platform.Web/wwwroot/js/app/security/widgets/accountSignInLogWidget.html`
- Create: `src/VirtoCommerce.Platform.Web/wwwroot/js/app/security/blades/sign-in-log.js`
- Create: `src/VirtoCommerce.Platform.Web/wwwroot/js/app/security/blades/sign-in-log.html`
- Modify: `src/VirtoCommerce.Platform.Web/wwwroot/js/app/security/security.js:336`
- Modify: `src/VirtoCommerce.Platform.Web/wwwroot/Localizations/en.VirtoCommerce.Platform.json`

There is no unit-test harness for the AngularJS admin SPA in this repo; this task is
verified in the browser at the Phase 3 checkpoint below.

- [ ] **Step 1: Add the resource actions**

In `accounts.js`, next to `searchSessions` (line 19):

```javascript
        searchSignInLog: { url: 'api/platform/security/sign-in-log/search', method: 'POST' },
        getSignInLogStats: { url: 'api/platform/security/sign-in-log/stats', method: 'POST' },
```

- [ ] **Step 2: Create the widget**

`accountSignInLogWidget.js`:

```javascript
angular.module('platformWebApp')
    .controller('platformWebApp.accountSignInLogWidgetController',
        ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.accounts',
            function ($scope, bladeNavigationService, accounts) {
                var blade = $scope.widget.blade;

                var userId = null;

                if (blade.data) {
                    userId = blade.data.id;
                }
                else if (blade.currentEntity) {
                    var account = _.first(blade.currentEntity.securityAccounts);
                    if (account) {
                        userId = account.id;
                    }
                }

                function refresh() {
                    blade.signInLogCount = 0;

                    if (!userId) {
                        return;
                    }

                    accounts.searchSignInLog(
                        {},
                        { userId: userId, take: 0 },
                        function (data) {
                            blade.signInLogCount = data.totalCount;
                        });
                }

                $scope.openBlade = function () {
                    if (!userId) {
                        return;
                    }

                    var newBlade = {
                        id: "signInLogBlade",
                        userId: userId,
                        refreshCountCallback: function (newCount) {
                            blade.signInLogCount = newCount;
                        },
                        controller: 'platformWebApp.signInLogController',
                        template: '$(Platform)/Scripts/app/security/blades/sign-in-log.html'
                    };
                    bladeNavigationService.showBlade(newBlade, $scope.blade);
                };

                refresh();
            }]);
```

`accountSignInLogWidget.html`:

```html
<div class="gridster-cnt" ng-click="openBlade()">
    <div class="cnt-inner">
        <div class="list-count">{{blade.signInLogCount | number:0}}</div>
        <div class="list-t">{{ 'platform.widgets.sign-in-log.title' | translate }}</div>
    </div>
</div>
```

- [ ] **Step 3: Create the blade**

`sign-in-log.js` — mirror `sessions-list.js` (same `uiGridHelper` /
`bladeUtils.initializePagination` wiring, same refresh/filter shape), calling
`accounts.searchSignInLog` with `{ userId: blade.userId, keyword, succeeded, signInTypes, startDate, skip, take }`.
Add a second call in `blade.refresh` to `accounts.getSignInLogStats` with the same
criteria minus paging, assigning the result to `blade.stats`. Add a period selector
(`24h` / `7d` / `30d`) that sets `filter.startDate` and re-runs `blade.refresh`.

`sign-in-log.html` — grid columns: date, user name, outcome, failure reason, sign-in
type, IP, store, organization. Two badges per row:

```html
<span class="badge badge-warning" ng-if="row.entity.signInType === 'Impersonation' || row.entity.signInType === 'ImpersonationRevert'">
    {{ 'platform.blades.sign-in-log.labels.impersonation' | translate }}
</span>
```

Statistics strip above the grid: four tiles bound to `blade.stats.totalCount`,
`blade.stats.failedCount`, `blade.stats.distinctUserCount`,
`blade.stats.impersonationCount`, then four top-10 tables bound to
`blade.stats.topFailedIpAddresses`, `topFailedUserNames`, `failureReasonBreakdown`,
`signInsByOrganization`, each row clickable to set the matching filter and refresh.

Label the organization table as covering known accounts only — failed attempts against
usernames that do not exist have no organization (spec §4.4):

```html
<div class="hint">{{ 'platform.blades.sign-in-log.hints.known-accounts-only' | translate }}</div>
```

- [ ] **Step 4: Register the widget**

In `security.js`, after the `accountSessionsWidgetController` registration (line 336):

```javascript
            widgetService.registerWidget({
                controller: 'platformWebApp.accountSignInLogWidgetController',
                template: '$(Platform)/Scripts/app/security/widgets/accountSignInLogWidget.html'
            }, 'accountDetail');
```

- [ ] **Step 5: Add English localizations**

In `en.VirtoCommerce.Platform.json`, add under `blades` and `widgets` respectively:

```json
      "sign-in-log": {
        "title": "Sign-in log",
        "labels": {
          "impersonation": "On behalf",
          "new-ip": "New IP",
          "outcome": "Outcome",
          "failure-reason": "Reason",
          "sign-in-type": "Type",
          "ip-address": "IP address",
          "organization": "Organization"
        },
        "stats": {
          "total": "Sign-ins",
          "failed": "Failed attempts",
          "users": "Distinct users",
          "impersonation": "On behalf sessions",
          "top-failed-ips": "Top IPs by failed attempts",
          "top-failed-users": "Top accounts by failed attempts",
          "failure-reasons": "Failure reasons",
          "by-organization": "Sign-ins by organization"
        },
        "hints": {
          "known-accounts-only": "Covers attempts against existing accounts only."
        }
      },
```

```json
      "sign-in-log": {
        "title": "Sign-in log"
      },
```

Only `en` is updated here; the other locale files are handled by the translation process.

- [ ] **Step 6: Build the admin SPA bundle**

Run: `npm run build` (from the repo root)
Expected: build succeeds, `wwwroot/dist/app.js` updated.

- [ ] **Step 7: Commit**

```bash
git add src/VirtoCommerce.Platform.Web/wwwroot/js/app/security src/VirtoCommerce.Platform.Web/wwwroot/Localizations/en.VirtoCommerce.Platform.json src/VirtoCommerce.Platform.Web/wwwroot/dist
git commit -m "feat(security): add sign-in log widget and blade with statistics"
```

---

### Task 18: Documentation

**Files:**
- Modify: `docs/user-guide/login-on-behalf.md`

- [ ] **Step 1: Rewrite the Security section**

The page currently claims *"All operations are strictly logged to avoid possible claims
from customers."* That is now true, but the page must also be honest about what the
feature does **not** restrict. Replace the note at line 8 and the Security section at
line 49 with:

```markdown
!!! note
    Every login-on-behalf session is recorded in the sign-in log, including who the
    operator was, which account they acted as, the IP address and the time. These rows
    are always written and cannot be disabled.

## Security

All actions take place inside the customer account, but `Created by` and `Modified by`
are recorded against the customer support account, not the customer.

To review login-on-behalf activity, open the account and use the **Sign-in log** widget,
or filter the sign-in log by the *On behalf* type. Reading the log requires the
`platform:security:sign_in_log:read` permission.

!!! warning
    The `platform:security:loginOnBehalf` permission is not limited to customer accounts.
    An operator holding it can sign in as any user, including an administrator, and the
    resulting session carries that user's full permissions. Grant this permission only to
    trusted support staff. Login-on-behalf activity is recorded for audit, but it is not
    restricted.
```

- [ ] **Step 2: Commit**

```bash
git add docs/user-guide/login-on-behalf.md
git commit -m "docs: describe sign-in log and login-on-behalf permission scope"
```

---

### Task 19: Full verification

- [ ] **Step 1: Run the whole test suite**

Run: `dotnet test VirtoCommerce.Platform.sln`
Expected: all tests pass, no skips in the new files.

- [ ] **Step 2: Confirm the change-log regression guard**

Confirm no code path added by this plan calls `IChangeLogService.SaveChangesAsync`:

Run: `git diff dev --stat -- src/ | grep -i changelog`
Expected: no output. If anything appears, the defect that ruled out the change log
(spec §2.4) has been reintroduced — sign-in volume would expire `ChangeLogCacheRegion`
and poison `/api/changes/lastmodifieddate` for every external cache consumer.

- [ ] **Step 3: Deploy and run the platform for browser verification**

**Stop here and ask the user to deploy and run the platform.** The following cannot be
proven by unit tests and must be checked in a browser against a running instance:

1. Sign in with a wrong password → a failed row appears in the sign-in log with
   `InvalidPassword`, the correct IP and the correct user agent
2. Sign in with a username that does not exist → a row with `UserNotFound`, no user id,
   no store, no organization
3. Sign in successfully → a succeeded row
4. Use **Login on behalf** on `b2badmin@test.com` → an `Impersonation` row with the
   operator's identity, and the Active Sessions blade shows the session marked as
   impersonated with the operator's name
5. Terminate that session from Active Sessions → the storefront session is killed
6. Revert impersonation → an `ImpersonationRevert` row
7. Set `SignInLogEnabled` to false → ordinary sign-ins stop producing rows, impersonation
   rows keep appearing
8. The statistics strip populates and each top-10 row filters the list when clicked

---

## Notes for the implementer

**Do not add authentication logic.** The impersonate branch at
[AuthorizationController.cs:309-378](../src/VirtoCommerce.Platform.Web/Controllers/Api/AuthorizationController.cs:309)
gains publish calls and nothing else. Specifically do not add a target restriction, do
not move the permission check, do not add `CanSignInAsync`, do not change token lifetime.
These were considered and deliberately excluded — see spec §3. If you think one of them
belongs here, raise it rather than adding it.

**Do not write to the change log.** See Task 19 Step 2 for why.

**Audit writes never fail a sign-in.** Every path from `IUserSignInLogWriter.Write`
downward swallows its exceptions. If you find yourself adding a `throw` on the audit
path, you have broken this.
