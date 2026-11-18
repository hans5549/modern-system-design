---
paths:
  - "tests/**"
---

# Testing rules

The user writes the tests by hand; Claude reviews them (see `code-review.md`).

## Setup (run by the user)

```
dotnet new xunit -o tests/ModernSystemDesign.Tests
dotnet sln ModernSystemDesign.slnx add tests/ModernSystemDesign.Tests
dotnet add tests/ModernSystemDesign.Tests reference src/ModernSystemDesign
```

## Rules

- Framework: xUnit. One test class per class under test; mirror the `src/` folder layout.
- Naming: `MethodName_Scenario_ExpectedResult` (see `naming.md`).
- Structure: Arrange / Act / Assert, one behavior per test. Tests are deterministic: no clock, randomness,
  network, or shared state between tests.
- Services: unit-test with hand-written fake repositories (implement the interface). Adopt a mocking library
  only if fakes become painful.
- Repositories: integration-test against SQLite in-memory. Keep the connection open for the lifetime of the
  test (`Data Source=:memory:` is destroyed when the connection closes), and create the schema per test.
- Test public behavior only. Do not test private members or assert on log output.
- A bug fix starts with a failing test that reproduces it.
- Components: test only when they contain real logic (bUnit); otherwise cover the logic in services.
