# Career Grid documentation

This documentation describes the recruitment product confirmed by the project owner and the code present in both workspace projects. JobStreet is the product inspiration; this document does not claim feature parity or describe JobStreet's internal implementation.

Baseline: October 6, 2026. Proposed functionality is explicitly separated from the current scaffold. Documentation is the planning baseline until endpoints and UI behavior are implemented and tested.

## Reading paths

| Audience | Suggested order |
| --- | --- |
| Product owner | Product requirements, frontend specification, roadmap and decisions |
| Backend developer | Development guide, architecture, data model, API specification, quality and operations |
| Frontend developer | Frontend specification, API specification, development guide, product requirements |
| QA or reviewer | Product requirements, API specification, quality and operations, roadmap and decisions |

## Contents

1. [Product requirements](product-requirements.md)
2. [Architecture](architecture.md)
3. [Data model](data-model.md)
4. [API specification](api-specification.md)
5. [Frontend specification](frontend-specification.md)
6. [Development guide](development-guide.md)
7. [Quality and operations](quality-and-operations.md)
8. [Roadmap and decisions](roadmap-and-decisions.md)

## Source of truth

Current backend behavior is defined by [Program.cs](../CG/Program.cs), [models](../CG/Models), [AppDbContext](../CG/DAL/AppDbContext.cs), [repositories](../CG/Repositories), and [migrations](../CG/Migrations). Current frontend behavior is defined by [App.tsx](../../frontend-career-grid/src/App.tsx) and [main.tsx](../../frontend-career-grid/src/main.tsx). Package declarations live in the backend project files and frontend `package.json`; frontend resolved versions live in `package-lock.json`.

When implementation changes, update the affected specification, acceptance criteria, and current-status entries in the same change. The proposed API is not a substitute for generated OpenAPI from completed controllers.

## Glossary

| Term | Meaning |
| --- | --- |
| Applicant | A user seeking work, represented by an applicant profile |
| Employer | A user with a company membership and a company-specific role |
| Administrator | A platform user authorized to moderate companies and job publication |
| Company verification | Platform approval of a company |
| Membership approval | Approval for an employer to act within a company |
| Job approval | Moderation state controlling whether a posting can become public |
| Classification | Broad occupational category |
| Subclassification | Occupational category belonging to a classification |
| Primary resume | An applicant's default resume; multiple stored resumes may exist |
| Draft application | An application not yet submitted to an employer |
| Hiring pipeline | Submitted applications grouped by recruitment status |
| DTO | Explicit API request or response object that exposes approved fields |
