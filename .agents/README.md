# `.agents` - vendor-neutral agent resources

Agent Skills ([agentskills.io](https://agentskills.io/specification)) layout shared across
coding agents. Each skill is a directory containing `SKILL.md` with `name` and `description`
frontmatter, plus optional `scripts/`, `references/`, and `assets/`.

```text
.agents/
├── README.md
└── skills/
    └── dotnet-results-conventions/
        ├── SKILL.md
        └── references/
```

Skill names match their directory names so any implementation that enforces that rule works too.
Root `AGENTS.md` points here for agents without skill support.
