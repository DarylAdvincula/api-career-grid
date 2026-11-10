# API specification

## Contract status

**Proposed:** every business endpoint in this document is unimplemented. The current host has no business controllers or mapped controller routes. These contracts guide development; generated OpenAPI and contract tests must validate them as endpoints are delivered.

Proposed base path: `/api/v1`. Local development host: `https://localhost:7265`. Use JSON request/response bodies except multipart resume upload and file download. IDs are positive integers. Enum values use their model names as strings. Instants use UTC ISO 8601, such as `2026-10-06T02:00:00Z`; experience dates use `YYYY-MM-DD`.

## Shared conventions

- Return `200` for reads/updates, `201` plus `Location` for created resources, and `204` for successful deletion/logout with no body. Public registration may instead return a generic `202` to support enumeration resistance; finalize this with authentication design.
- Return `400` for malformed/invalid input, `401` for missing/invalid authentication, `403` for forbidden operations, `404` for missing or concealed private resources, and `409` for duplicates, invalid transitions, or stale versions.
- Uploads may return `413` for size and `415` for unsupported media. Throttled requests return `429` with retry guidance.
- Use a consistent Problem Details error shape. Never return exception traces, SQL, file paths, hashes, or verification tokens.
- Scope private resource lookup before returning data. A cross-company request must not reveal whether the resource exists.
- Proposed partial updates use `PATCH` with explicit DTOs: omitted property means unchanged; nullable property set to null means clear. Reject forbidden/unknown fields rather than binding database entities.
- Collections use `page` (default 1) and `pageSize` (default 20, maximum 100). Reject invalid bounds. Default public job ordering is PostedAt descending, then Id descending; management collections use documented timestamp/Id ordering. Filtering occurs before counting/paging.
- A proposed opaque `version` property supports optimistic concurrency for editable workflows. Update requests supply the version they read; mismatches produce `409`. The database support is not implemented.

Example collection envelope:

```json
{
  "items": [],
  "page": 1,
  "pageSize": 20,
  "totalCount": 0,
  "totalPages": 0
}
```

Example validation problem:

```json
{
  "type": "about:blank",
  "title": "Validation failed",
  "status": 400,
  "detail": "One or more fields are invalid.",
  "errors": { "endDate": ["End date cannot precede start date."] },
  "traceId": "request-correlation-id"
}
```

## Authentication and accounts

Session/cookie versus bearer design remains undecided. The routes describe behavior rather than promising a token format. Admin accounts are provisioned through a controlled process, not public registration.

| Method | Path | Access | Behavior |
| --- | --- | --- | --- |
| POST | `/auth/register` | Guest | Register Applicant/Employer with email, password, firstName, lastName, role |
| POST | `/auth/login` | Guest | Authenticate and establish the selected session mechanism |
| POST | `/auth/logout` | Authenticated | Revoke the current session |
| GET | `/auth/me` | Authenticated | Safe account summary and available memberships; no secrets |
| POST | `/auth/verify-email` | Token holder | Consume single-use verification token |
| POST | `/auth/resend-verification` | Guest/authenticated | Generic throttled response; send only when eligible |

Password reset, password change, session renewal, and session listing must be specified before launch if required by the selected authentication design. Token/code hashing and expiry cannot be inferred from the current `VerificationCode` field.

## Public discovery and taxonomy

| Method | Path | Access | Behavior |
| --- | --- | --- | --- |
| GET | `/jobs` | Public | Eligible paginated jobs; keyword, location, classificationId, subClassificationId, jobType, workEnvironment, minSalary, maxSalary filters |
| GET | `/jobs/{jobId}` | Public | Published/active job from approved company; otherwise 404 |
| GET | `/companies/{companyId}` | Public | Approved company's public profile; excludes membership code/internal approval fields |
| GET | `/classifications` | Public | Paginated taxonomy list |
| GET | `/classifications/{id}/subclassifications` | Public | Paginated children |
| GET | `/skills` | Public | Paginated skills, optionally filtered by keyword and taxonomy |

Salary filtering must wait until comparable currency and period semantics are defined. Unknown salary does not mean zero. Query combinations must validate that a supplied subclassification belongs to the selected classification.

## Applicant workspace

All `/applicant/me` resources are resolved from the authenticated Applicant account. Child IDs are checked against that profile; clients cannot supply a different profile ID.

| Method | Path | Behavior |
| --- | --- | --- |
| GET | `/applicant/me/profile` | Own profile |
| PATCH | `/applicant/me/profile` | Update headline, homeLocation, bio |
| GET / POST | `/applicant/me/experiences` | List/create owned experience |
| PATCH / DELETE | `/applicant/me/experiences/{id}` | Update/delete owned experience |
| GET / POST | `/applicant/me/skills` | List/add unique skill with optional proficiency and experience years |
| PATCH / DELETE | `/applicant/me/skills/{id}` | Update/delete own association |
| GET / POST | `/applicant/me/resumes` | List metadata/upload multipart field `file` |
| PUT | `/applicant/me/resumes/{id}/primary` | Select primary resume atomically |
| GET | `/applicant/me/resumes/{id}/download` | Authorized file response with safe Content-Disposition |
| DELETE | `/applicant/me/resumes/{id}` | Delete only if retention/reference rules permit |
| GET | `/applicant/me/applications` | Paginated owned applications; optional submitted/status filters |
| POST | `/applicant/me/applications` | Create draft using jobPostingId, resumeId, coverLetter |
| GET / PATCH | `/applicant/me/applications/{id}` | Read own application/edit draft fields only |
| DELETE | `/applicant/me/applications/{id}` | Delete own unsubmitted draft only |
| POST | `/applicant/me/applications/{id}/submit` | Validate and submit draft atomically |
| GET | `/applicant/me/applications/{id}/history` | Public status history without internal notes/actor private information |

Profile creation is proposed as part of applicant registration, so profile POST is not exposed separately. Missing own profile should be handled as an account setup failure with an intentional recovery path.

Example draft request:

```json
{ "jobPostingId": 101, "resumeId": 22, "coverLetter": "I would like to apply for this role." }
```

Example submission response:

```json
{
  "id": 305,
  "jobPostingId": 101,
  "applicationStatus": "Applied",
  "isSubmitted": true,
  "submittedAt": "2026-10-06T02:00:00Z",
  "updatedAt": "2026-10-06T02:00:00Z",
  "version": "opaque-version"
}
```

The submission route takes the expected version and uses server identity for the actor. Duplicate/retried submission must never create another application or initial status log; return a documented conflict or the existing result consistently. Race handling must rely on constraints/concurrency, not only a prior existence query.

## Employer company workspace

All management routes require Employer authentication. Company scope comes from approved memberships; the owner bootstrap exception allows pending-company information/draft management as specified in product requirements.

| Method | Path | Role/behavior |
| --- | --- | --- |
| GET | `/employer/me/memberships` | Own memberships and pending states |
| POST | `/employer/companies` | Create company plus owner membership transaction |
| POST | `/employer/membership-requests` | Request membership using company code; no automatic approval |
| GET | `/employer/companies/{companyId}` | Scoped private company view |
| PATCH | `/employer/companies/{companyId}` | Owner/Admin updates allowed company fields |
| GET | `/employer/companies/{companyId}/memberships` | Owner/Admin lists company memberships |
| POST | `/employer/companies/{companyId}/memberships/{id}/approve` | Owner/Admin approves pending member and assigns role |
| POST | `/employer/companies/{companyId}/memberships/{id}/reject` | Owner/Admin rejects pending member |
| GET / POST | `/employer/companies/{companyId}/jobs` | Authorized list/create draft |
| GET / PATCH | `/employer/companies/{companyId}/jobs/{id}` | Authorized read/edit under state policy |
| POST | `/employer/companies/{companyId}/jobs/{id}/submit` | Owner/Admin/Recruiter sends valid draft for review |
| POST | `/employer/companies/{companyId}/jobs/{id}/reopen-draft` | Owner/Admin/Recruiter moves rejected/published job to Draft under policy |
| PATCH | `/employer/companies/{companyId}/jobs/{id}/availability` | Owner/Admin/Recruiter sets isActive; not approval status |
| GET | `/employer/companies/{companyId}/applications` | Permitted hiring team; submitted only; jobId/status filters |
| GET | `/employer/companies/{companyId}/applications/{id}` | Scoped candidate/application details |
| GET | `/employer/companies/{companyId}/applications/{id}/resume` | Download attached resume only through authorized application access |
| POST | `/employer/companies/{companyId}/applications/{id}/status` | Valid transition with new status, notes, expected version |
| GET | `/employer/companies/{companyId}/applications/{id}/history` | Scoped history including permitted internal notes |

Example status mutation:

```json
{ "applicationStatus": "Screening", "notes": "Review qualifications.", "version": "opaque-version" }
```

Candidate details expose only information needed for recruitment. Do not serialize the full `UserAccount`. Personal verification information and other-company applications are excluded. Internal notes are never included in applicant-facing DTOs.

Job write DTO: title, requirementsMarkdown, optional jobClassificationId/jobSubClassificationId, jobType, workEnvironment, location, minSalary/maxSalary, and skill requirements. Creator/company identity and moderation fields are server-owned. Published content cannot be silently edited without following the material-edit policy.

## Platform moderation

| Method | Path | Behavior; platform Admin only |
| --- | --- | --- |
| GET | `/admin/companies` | Paginated companies filtered by verificationStatus |
| GET | `/admin/companies/{id}` | Company review details |
| POST | `/admin/companies/{id}/approve` | Approve pending company, set admin/time |
| POST | `/admin/companies/{id}/reject` | Reject pending company with reason |
| GET | `/admin/jobs` | Paginated jobs filtered by approvalStatus |
| GET | `/admin/jobs/{id}` | Review job/company information |
| POST | `/admin/jobs/{id}/publish` | Publish pending job if company remains eligible |
| POST | `/admin/jobs/{id}/reject` | Reject pending job with reason |

Moderation reasons/audit records require schema additions before these contracts can be completed. Taxonomy administration can be specified later; public taxonomy reads alone do not imply writable administration endpoints.

## Implementation checklist

Map controllers, configure enum serialization, implement DTO validation and Problem Details, add authentication and scoped authorization, implement service transactions, add concurrency support and storage providers, and write contract/integration tests. Use generated OpenAPI to resolve details such as content types, nullable fields, enum schemas, and exact success/error responses. A listed endpoint is complete only when its authorization and failure cases are tested.
