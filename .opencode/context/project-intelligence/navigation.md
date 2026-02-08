<!-- Context: project-intelligence/nav | Priority: high | Version: 1.1 | Updated: 2026-02-08 -->

# Project Intelligence

> Start here for quick project understanding. These files bridge business and technical domains.

## Structure

```
.opencode/context/project-intelligence/
├── navigation.md              # This file - quick overview
├── business-domain.md         # Business context and problem statement
├── technical-domain.md        # Stack, architecture, technical decisions
├── business-tech-bridge.md    # How business needs map to solutions
├── decisions-log.md           # Major decisions with rationale
└── living-notes.md            # Active issues, debt, open questions
```

## Quick Routes

| What You Need | File | Description | Priority |
|---------------|------|-------------|----------|
| Understand the "how" | `technical-domain.md` | Stack, architecture, patterns | critical |
| Understand the "why" | `business-domain.md` | Problem, users, value proposition | high |
| See the connection | `business-tech-bridge.md` | Business → technical mapping | medium |
| Know the context | `decisions-log.md` | Why decisions were made | medium |
| Current state | `living-notes.md` | Active issues and open questions | low |

## Usage

**New Team Member / Agent**:
1. Start with `navigation.md` (this file)
2. Read all files in order for complete understanding
3. Follow onboarding checklist in each file

**Quick Reference**:
- Technical focus → `technical-domain.md`
- Business focus → `business-domain.md`
- Decision context → `decisions-log.md`

## Integration

This folder is referenced from:
- `.opencode/context/core/standards/project-intelligence.md` (standards and patterns)
- `.opencode/context/core/system/context-guide.md` (context loading)

## Maintenance

Keep this folder current:
- Update when business direction changes
- Document decisions as they're made
- Review `living-notes.md` regularly
- Archive resolved items from decisions-log.md
