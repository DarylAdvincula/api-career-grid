# Quality, security, and operations

## Status

**Current:** builds and frontend lint pass; no automated test cases, authentication, authorization, upload implementation, deployment pipeline, health endpoints, or operational monitoring exist. Everything below is a **proposed release requirement**, not a completed control. Compilation alone does not establish production readiness.

## Security requirements

| Area | Required behavior |
| --- | --- |
| Identity | Maintained password hashing implementation; no plaintext passwords; controlled Admin provisioning; throttle registration/login/verification |
| Verification | Expiring single-use tokens; protected token storage; generic resend/reset responses; no secret logging |
| Sessions | Defined expiry, revocation, renewal, logout; secure cookies and CSRF defenses if cookie sessions are selected |
| Authorization | Server-enforced account role, ownership, approved membership, company scope, and action permission |
| Data exposure | DTO allowlists; no password hash, verification code, storage path, or internal notes in public/applicant responses |
| Inputs | Validate lengths, enums, IDs, date/salary relationships, URL protocols, file metadata, and cross-entity ownership |
| Browser content | Sanitize Markdown/URLs; avoid unsafe raw HTML; deployment headers compatible with required assets |
| Files | Private storage, opaque generated names, authorized downloads, content validation |
| Persistence | Parameterized queries; constraints for races; reviewed migrations; no arbitrary client-supplied SQL/sort expressions |
| Secrets | Environment/secret manager configuration; no credentials in source, builds, logs, or documentation |

Knowing another company's resource ID must never grant access. Platform admins have moderation access; blanket access to applicant resumes/private notes is not automatically granted.

## Resume storage proposal

Proposed initial policy: PDF and DOCX, maximum 5 MiB. This planning default requires confirmation and is not implemented. Validate signatures/container structure as well as extension and declared content type. Store generated filenames outside public assets or in private object storage; original filenames are display metadata, never trusted paths. Consider scanning/quarantine before availability. Downloads require owner access or authorized access through a submitted application.

Coordinate file/database operations: remove staged files on metadata-save failure; maintain retryable cleanup after physical deletion failure. Avoid long database transactions during transfers/scanning. Backups must recover storage and SQL together.

Submitted applications should preserve the selected resume version. Do not overwrite referenced files in place. Define retention and deletion before removing referenced files. The current schema has metadata only, with no provider, scanning state, or cleanup queue.

## Privacy and audit

Collect only recruitment-related information. Employer access covers submitted applications within approved company scope. Internal notes stay within the hiring team. Public company DTOs exclude membership discovery codes.

ApplicationStatusLog supports recruitment history, not company verification, job moderation, membership decisions, or security audit. Add records for those workflows with actor, action, resource, timestamp, and appropriate state, excluding secrets and full sensitive documents.

Before public release, agree retention, deletion/anonymization, access/export, and privacy policies. This documentation does not assert legal compliance or prescribe jurisdiction-specific deadlines.

## Test strategy

| Level | Coverage |
| --- | --- |
| Service unit | Permissions, transition matrix, ownership, submission eligibility, derived fields |
| SQL Server integration | FKs/checks/unique indexes, transactions, primary resume switching, duplicate submissions, concurrency |
| HTTP integration | Identity/company scope, DTOs, validation problems, status codes, concealed resources |
| Frontend interaction | Forms, URL filters, cache isolation, errors, valid board actions/rollback |
| Browser end-to-end | Applicant submission, employer review, moderation, route refresh, keyboard paths |

Use SQL Server for relational constraint tests; an in-memory EF provider does not establish SQL Server check/filtered-index behavior. Use an isolated test database with explicit safeguards against other environments.

Priority scenarios:

1. Guests cannot access private data; applicants cannot read another applicant's profile, draft, or resume.
2. Pending/rejected members cannot enter company workspaces; approved members cannot cross companies.
3. Unverified applicants/ineligible jobs cannot complete submission under the proposed policy.
4. Duplicate/concurrent submissions produce one application and one initial log.
5. Status update and log persist atomically; stale versions cannot overwrite newer changes.
6. Applicants cannot attach another applicant's resume or mutate submitted content.
7. Primary selection prevents multiple primaries; invalid dates and enum values fail.
8. Public reads exclude unpublished/inactive jobs and unapproved companies.
9. Unsafe Markdown, filenames, URL schemes, oversized/mismatched files are handled safely.
10. Logout/account switching clears private caches; failed mutations never display success.

Fixtures should include two companies, multiple roles/membership states, two applicants, taxonomy, job states, owned resumes, and draft/submitted applications. Use synthetic data. Assert behavior/invariants rather than reproducing implementation statements.

## CI proposal

Backend: restore/build application and tests; run actual discovery; fail on zero tests once the harness exists; execute isolated SQL tests; archive safe diagnostics. Include tests in the solution or explicitly target them. Frontend: `npm ci`, lint, production build, then interaction/browser tests when configured. Review dependency and API/schema changes.

Backend package pinning/locking is unresolved; frontend has `package-lock.json`. No CI provider/workflow exists.

## Deployment proposal

1. Build `dotnet publish CG/CG.csproj -c Release` and frontend `npm.cmd run build` from their repositories.
2. Exclude local settings/secrets from artifacts; supply database/storage/mail/auth configuration through the host.
3. Back up SQL and files; review idempotent migration SQL and old/new application compatibility.
4. Apply approved migrations using a controlled deployment identity; runtime identity should not have unrestricted schema privileges.
5. Deploy API with HTTPS, trusted proxy configuration, and selected origin/session policy.
6. Deploy static assets with SPA fallback, correct asset paths, and public API base configuration.
7. Smoke-test readiness, search, authentication, a controlled application flow, and file authorization.
8. Monitor errors, latency, storage/mail failures; roll back only when schema compatibility permits.

Hosting provider, environments, secret manager, and delivery mechanism are undecided. Swagger is currently Development-only; production access must be deliberate.

## Observability and recovery

Proposed health endpoints distinguish liveness/readiness. Readiness checks dependencies without exposing credentials. Use structured logs/trace IDs without sensitive request bodies, file contents, tokens, passwords, or internal notes.

Monitor HTTP/database errors, latency, storage/mail failures, and failed authorization trends. Agree service objectives and thresholds after representative load measurements. Implement pagination/projection before large-dataset testing; inspect query plans before adding speculative indexes.

Protect SQL/file backups and rehearse coordinated restore into an isolated environment. Agree recovery time and acceptable data-loss objectives. Destructive migration recovery may require forward repair or restore; `migrations remove` is not a production rollback plan.
