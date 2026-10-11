# Pre-commit self-review checklist

Run through this before finishing any change.

## Ownership and placement

- [ ] The change is in the right project: core / extensions / web / tests.
- [ ] No framework dependency was introduced into core domain types.
- [ ] The new file has one primary type or responsibility and is named after it.
- [ ] Any type split uses `partial` files named by logical aspect.

## Naming

- [ ] Types PascalCase and noun/domain based.
- [ ] Methods PascalCase and verb-first.
- [ ] Parameters and locals camelCase and intent-revealing (no `obj`, `tmp`, `data`).
- [ ] Properties PascalCase and domain-driven.
- [ ] Constants `const` and PascalCase.
- [ ] Test name describes the behavior under test and the expected result.

## API shape

- [ ] Creation goes through `Success` / `Failure` factories or an existing helper.
- [ ] No overload was added that is not genuinely needed.
- [ ] Members ordered: constructors/factories, public API, helpers, private/internal.
- [ ] Public surface stays minimal; no hidden behavior in framework adapters.

## Failure semantics

- [ ] Required arguments guarded with `ArgumentNullException.ThrowIfNull(...)`.
- [ ] Result-like values validated with the `AssertNotDefault` pattern.
- [ ] Original exception preserved in failure results.
- [ ] Every `catch` either converts to a failure result or rethrows.
- [ ] Domain-level failures return structured results instead of throwing.

## Tests

- [ ] Every new success branch, failure branch, and edge case has a test.
- [ ] Test project and namespace align with the component under test.

## Simplicity check

- [ ] Bodies are compact; guards come before nested conditionals.
- [ ] No abstraction added beyond the core result workflow.
- [ ] If an overload set or feature became hard to read, it was refactored to a smaller
      abstraction or clearer helper.
