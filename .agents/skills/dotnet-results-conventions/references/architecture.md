# File organization and architecture

## Layout

Separate core domain logic from framework-specific integrations and keep framework
dependencies isolated in dedicated areas.

Current structure (the reference model):

| Project | Responsibility |
|---|---|
| `Y.Results` | Core result abstraction and behavior |
| `Y.Results.Extensions` | Fluent extension methods |
| `Y.Results.Web` | ASP.NET integration and filters |
| `Y.Results.Tests` | Unit tests for core behavior |

## Rules

- Keep framework dependencies out of core domain types; ASP.NET integration lives in `Y.Results.Web`.
- Keep tests in dedicated test projects with namespace alignment to the component under test.
- Group logic by capability rather than by incidental implementation details.
- Prefer a single responsibility per project/folder rather than mixing web, core, and tests
  into one assembly.
- Use `partial` files to split a large type by logical aspect while keeping one type.

## Extending the solution

- Preserve the existing conventions when extending the library.
- Keep framework integration separate from the core result model.
- When adding overloads, check whether they are truly needed; if they become confusing,
  prefer a clearer minimal API.
- Add tests for every new success/failure branch or edge case.
- Keep implementation and test naming consistent with the repository conventions.
