# Roadmap and decisions

## Current baseline

Reviewed October 6, 2026. Backend contains models, SQL Server mapping/migration, and basic repositories; frontend is the React/Vite starter. Workflows remain implementation scope.

Observed verification: backend build passed with zero warnings/errors; frontend build/lint passed. Direct execution of the test project discovered no tests. No SQL Server migration/integration test or recruitment browser flow was executed during review.

## Implementation sequence

| Phase | Deliverables | Completion gate |
| --- | --- | --- |
| 0. Agree baseline | Critical decisions below, MVP priority, frontend version control | Accepted product/state/permission contracts |
| 1. Foundation | Profile uniqueness, nullable approver, ownership/date/enum rules, concurrency, UTC, application persistence, controller mapping/errors | Reviewed migrations and isolated SQL tests |
| 2. Identity/tenancy | Registration/verification/session, policies, company create/join/approval, admin bootstrap | Cross-user/company authorization tests |
| 3. Discovery | Taxonomy seed/read, public company views, paginated eligible search/detail, public UI | Filter/direct-detail eligibility tests |
| 4. Applicant | Profile/skills/experience, private resumes, draft/review/submit/tracking | Full flow, duplicate/race/ownership tests |
| 5. Employer | Job authoring, membership UI, candidate detail/status pipeline | Valid transitions, atomic logs, accessible actions |
| 6. Admin | Company/job review, decisions, reasons/audit | Moderation controls public visibility |
| 7. Release | CI, production configuration, monitoring, retention, restore, browser acceptance | Release checks and restore rehearsal |

Phases describe dependencies, not dates/estimates. Thin end-to-end slices can span phases; validate API and permissions before completing every screen.

## Known gaps

| Gap | Consequence | Resolution |
| --- | --- | --- |
| No controllers/route mapping | No business HTTP API | DTO/controllers/services and MapControllers |
| Required membership approver | Pending requests need premature approver | Nullable FK and workflow/bootstrap policy |
| Duplicate applicant profiles | Ambiguous lookup/application uniqueness bypass | Unique account FK and safe data migration |
| Resume ownership unenforced | Cross-applicant references persist | Ownership/integrity strategy |
| Missing date/CompanyRole checks | Invalid values despite model comments | Service validation and database checks |
| Employer lookup returns first by account | Ambiguous multiple memberships | Decide cardinality and scope by company |
| No application service/repository | No dedicated submission/status workflow | Transactional service/persistence |
| UpdatedAt initialized only | Stale timestamps after writes | Central write timestamp policy |
| No concurrency token | Recruiter updates overwrite each other | Row version and conflict responses |
| Salary lacks currency/period | Ambiguous display/comparisons | Product and schema/DTO changes |
| No moderation reason/history | Incomplete feedback/audit | Moderation records |
| No auth/storage/mail/CORS integration | No complete workflows | Selected providers and policies |
| Empty tests outside solution | Successful command can hide zero coverage | Test cases and execution wiring |
| Frontend starter | No recruitment UI | Implement frontend specification |

## Decision register

Unresolved entries use proposed defaults to make the specification concrete; they are not approved business policies.

| ID | Question | Proposed baseline / impact |
| --- | --- | --- |
| D-01 | Applicant and employer on one account? | Single role MVP matches current enum; multi-role changes schema/auth/navigation |
| D-02 | Multiple employer companies? | Multiple distinct memberships, unique account/company pair; company-scoped routes |
| D-03 | First owner approval? | Transactional system bootstrap; company stays pending platform verification |
| D-04 | Which actions require email verification? | Submission/employer mutations; decide profile/draft exceptions |
| D-05 | Session mechanism? | Browser-first secure cookie proposed; finalize CSRF/expiry/renewal |
| D-06 | Company permissions? | Proposed product table; per-job assignment deferred |
| D-07 | Rejection/revocation behavior? | Job rejection can return to Draft; company/member reopening/suspension need policy |
| D-08 | Published job edits? | Material edits return to Draft/review |
| D-09 | Skip/backward stages? | No MVP; Hired/Rejected terminal |
| D-10 | Salary currency/period? | Cannot infer; add before comparisons/display |
| D-11 | Resume policy? | PDF/DOCX, 5 MiB, immutable referenced versions; choose storage/scanning/retention |
| D-12 | Resume required at submission? | Proposed yes; nullable ResumeId supports drafts |
| D-13 | Company codes? | Generated unique discovery code, never an approval shortcut; decide visibility/rotation |
| D-14 | Notes/history visibility? | Separate DTOs; notes hiring-team-only; applicant status/time |
| D-15 | Notifications? | Verification email required; status/moderation notifications need provider/event scope |
| D-16 | Hosting/environments? | Dev, isolated test, staging, production; providers/CI undecided |
| D-17 | Retention/deletion? | Preserve history by closure until policy is accepted |
| D-18 | DraftStep semantics? | Boolean currently; decide multi-step persistence before replacement |

## Release checklist

- Critical decisions are accepted and reflected in code/contracts.
- Migrations are reviewed, staged, and tested with representative data.
- Authentication, tenant isolation, ownership, and file access tests pass.
- Search eligibility, duplicate submission, and stale-write behavior are correct.
- Applicant/employer/admin browser flows pass with accessible alternatives to drag-and-drop.
- Artifacts/configuration contain no local settings, secrets, or personal test data.
- Monitoring, schema-compatible rollback, coordinated restore, and operational owners are established.
- Documentation states implemented endpoints, outstanding limitations, and actual test counts.

## Maintenance

Assign owners to accepted decisions/features. Record resolution date, rationale, and affected documents. Update baseline statements when behavior is delivered; keep proposals marked until then. Release notes describe user-visible changes and migration/configuration requirements without presenting planned features as live.
