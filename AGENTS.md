# Agent instructions

Repository coding conventions live in the vendor-neutral Agent Skills layout:

- Skills root: `.agents/skills/`
- Available skill: `.agents/skills/dotnet-results-conventions/SKILL.md`

Read that `SKILL.md` before adding, refactoring, or reviewing C# in this solution. Its
`references/` files (paths resolve against the skill directory) carry the verified detail:
`api-surface.md`, `architecture.md`, `naming.md`, `code-style.md`, `failure-handling.md`,
`testing.md`, `principles.md`, `checklist.md`.

Highest-risk rule, whatever tool you use: source and test files are compiled twice - the
`Y.Results.Structs*` projects link the `Y.Results.Classes*` files via `<Compile Include>` in the
`.csproj`, so every new or renamed file must be added to both projects, and every partial type needs
both `#if CLASSES` and `#elif STRUCTS` branches.

Verify with `dotnet build Y.Results.sln` and `dotnet test Y.Results.sln` (12 test assemblies:
2 variants x 3 layers x 2 target frameworks).

If your tool does not support skills, load the `SKILL.md` and `references/*.md` files directly.
