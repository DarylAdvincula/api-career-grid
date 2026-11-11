# Frontend specification

## Current implementation

`src/main.tsx` mounts `App` inside React StrictMode. `App.tsx`, `App.css`, and `index.css` display and style the Vite starter screen. No application routes, forms, query provider, authentication context, API client, or Career Grid screens exist.

Declared dependencies include React 19, React Router, TanStack Query, Axios, React Hook Form, `@hello-pangea/dnd`, `@uiw/react-md-editor`, React Markdown, and rehype-sanitize. Installed packages are capabilities available for implementation, not evidence of delivered features.

## Proposed user experience

Use JobStreet as inspiration for familiar job discovery and application workflows while creating Career Grid's own branding, content, and layouts. Prefer a strong search entry point, readable job cards, clear company identity, and visible application progress. Do not reproduce another product's logos or copy proprietary content.

## Proposed routes and screens

| Route | Audience | Main content/actions |
| --- | --- | --- |
| `/` | Public | Career Grid introduction and job search |
| `/jobs` | Public | URL-driven filters, result cards, pagination |
| `/jobs/:jobId` | Public | Full posting, company link, eligibility-aware apply action |
| `/companies/:companyId` | Public | Approved company profile and public jobs |
| `/login`, `/register`, `/verify-email` | Public | Account access, registration role choice, verification feedback |
| `/applicant/profile` | Applicant | Profile, experience, and skills editing |
| `/applicant/resumes` | Applicant | Upload/library, primary selection, permitted download/delete |
| `/applicant/applications` | Applicant | Drafts and submitted application tracking |
| `/applicant/applications/:id` | Applicant | Draft review/edit or submitted details/status history |
| `/employer` | Employer | Membership/company selector and onboarding status |
| `/employer/companies/new`, `/employer/join` | Employer | Company creation or pending membership request |
| `/employer/companies/:companyId` | Scoped employer | Company information and verification status |
| `/employer/companies/:companyId/members` | Owner/Admin | Membership decisions and roles |
| `/employer/companies/:companyId/jobs` | Scoped employer | Draft/pending/published/rejected jobs and availability |
| `/employer/companies/:companyId/jobs/new` | Permitted author | Job authoring form |
| `/employer/companies/:companyId/jobs/:id` | Scoped employer | Job review/edit and publication workflow |
| `/employer/companies/:companyId/applications` | Permitted hiring team | Company/job pipeline and filters |
| `/employer/companies/:companyId/applications/:id` | Permitted hiring team | Candidate details, attached resume, status/history |
| `/admin/companies`, `/admin/companies/:id` | Platform Admin | Verification queue and decision detail |
| `/admin/jobs`, `/admin/jobs/:id` | Platform Admin | Publication queue and decision detail |

Unknown routes display a useful 404 page. Protected routes resolve identity before rendering private content. Preserve a safe same-origin return location after login; do not redirect to arbitrary external URLs.

## Page behavior

Job search supports keyword/location entry and taxonomy/type/work-environment filters. Reset the page when filters change. Display salary only when known and label currency/period once that model is added. Render unknown salary as undisclosed, not zero. Make mobile filters reachable without hiding the active filter summary.

Job details show application eligibility. Guests can sign in and return to the job; applicants with an existing draft resume it; submitted applicants see their application. Closed/unavailable jobs block new submission. Applicant-owned application detail remains available even when the public job detail no longer is.

Application drafting chooses an owned resume and optional cover letter, then provides a review step. Explicit draft saving is the MVP baseline; autosave can be added only with clear save/error indicators and conflict handling. Display success only after server confirmation. Retrying a failed submission must not duplicate records.

Company onboarding distinguishes account verification, membership status, and company verification. Explain the next available action for each state. Pending company owners may work on allowed information/drafts; pending joining members do not enter the private company workspace.

Job authoring uses classification-dependent subclassification choices, skill selection, salary validation, Markdown editor/preview, and the publication states in product requirements. Review screens show moderation reasons once supported by the API/schema.

The hiring board groups submitted applications by Applied, Screening, Interviewing, Hired, and Rejected. Drafts are excluded. A board cannot pretend to show all candidates from a single truncated page: use independently paginated columns or a paginated list until a board query contract is implemented. Apply global counts only when supplied by the server.

Provide explicit status actions alongside drag-and-drop for keyboard/touch accessibility. Only allow valid transitions. On stale-version or server rejection, restore the authoritative state and explain the failure. Terminal columns cannot accept prohibited transitions.

## Shared components

Proposed components: role-aware navigation, search form, filter controls, job card, company summary, status badge, pagination, form field/error display, resume picker, Markdown preview, application review panel, status timeline, candidate card, decision dialog, and loading/empty/error panels.

Use centralized design tokens for spacing, colors, type, borders, and focus states. Show text labels with status colors. Support narrow screens and long titles, avoid page-level horizontal scrolling, and provide visible focus, associated labels, semantic headings, and announced errors/status feedback.

## API and state management

Use one configured Axios client and feature-specific query/mutation modules. Proposed `VITE_API_BASE_URL` defaults are not yet implemented. Vite environment variables are public client configuration.

Query keys include authenticated user/company scope, filters, and page. On logout/account switch, clear private caches. After successful mutations, invalidate affected details and lists: resume primary selection refreshes the library; application submission refreshes own applications; status change refreshes candidate/history/pipeline; moderation refreshes public eligibility and review queues.

Abort obsolete requests when practical. Avoid treating every `401` as an endless refresh loop. Map field errors to forms; distinguish unauthenticated, forbidden, missing, conflicting, and network failures. Sensitive candidate data must not be persisted in browser storage by default.

Render requirements with an allowlisted Markdown pipeline and rehype-sanitize. Validate URL protocols, avoid unsanitized raw HTML, and do not trust employer-authored content. Resume viewing uses authorized API download rather than a permanent public file path.

## Frontend acceptance checks

- Direct navigation and browser refresh work on every proposed route after hosting fallback is configured.
- URL search filters are shareable, resettable, and restored by browser back/forward.
- Unauthorized users never see cached private screens after account switching.
- All forms expose server validation and prevent duplicate mutation feedback.
- Keyboard users can apply, manage resumes, review memberships, and change candidate status.
- Loading, no-results, offline/failure, and stale-update states have actionable messages.
- Markdown and uploaded filenames cannot introduce script execution or unsafe navigation.

These checks describe intended behavior; passing build/lint on the starter page does not satisfy them.
