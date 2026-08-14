# Writing a pull request description for the platform and modules

An upstream `vc-platform` / `vc-module-*` pull request ships a NuGet package to people who will never
read the branch. The description is the only artifact they read before upgrading.

Written as a narrative of the work — problem, what I did, what I tried and dropped, tests — it answers
questions nobody asked and buries the one thing a consumer needs. Write the sections instead as **the
consumer's questions, in the order they ask them**.

## The shape

```markdown
## Description
<what changed, in domain terms; the defect it fixes; then the "no new surface" line —
 no schema change / no migrations / no new settings / no new dependencies>
<link to a companion PR in another repo, if one exists>

## New GraphQL queries and mutations
## Changes to existing GraphQL queries and mutations
## New public services / interfaces / methods
## New protected methods (extensibility)
## Breaking changes
## New dependencies

> **Note (…)** — deep rationale, rejected alternatives, measurement detail

## References
### QA-test:
### Jira-link:
### Artifact URL:
```

`## References` is the repository's own PR template — keep its exact headings. The `Artifact URL` line
is written by CI after each build; read "The description has other writers" below before editing a
body by hand.

## Three mechanics, without which the skeleton is decoration

**Write `None` explicitly; never drop a section.** «None — this PR adds no query or mutation» is a
claim that the author checked. A missing section is indistinguishable from an author who never
considered it. On one x-catalog PR four of the seven sections came back `None`, and being forced to
answer them is exactly what surfaced the one fact a consumer needed.

**Phrase from the consumer, not the author.** Not «extracted a method», but «override this to change
ranking without touching hydration». The test: does the sentence tell a stranger what they may now do,
or tell a reviewer what you did?

**Demote deep rationale to `>` notes at the bottom.** Rejected alternatives, measurement detail and the
mechanism behind a trade-off belong in the PR — that is the right channel for them — but below
everything a consumer needs in order to act.

## Breaking changes splits three ways, because it breaks at three different times

| Kind | Breaks at | Example |
|---|---|---|
| Signature change / removed member | compile | a constructor parameter added, an interface method resignatured |
| Removed or renamed published surface | package upgrade | a `public` method deleted with no `[Obsolete]` overload |
| Behaviour or runtime-type change | runtime, with no warning | a resolver whose declared type is unchanged but whose runtime value is now an `IDataLoaderResult` |

The third is the one an undifferentiated list hides, and the one nothing in the toolchain will catch.

State the `[Obsolete]` decision explicitly too — «no `[Obsolete]` was introduced (module is
pre-release; the removed members had no external callers)» records that backward compatibility was
considered rather than forgotten.

## Why this ordering and not another

The reviewer's ranking is the mirror of the author's: a review starts with backward compatibility, and
a breaking public or `virtual` API change is the highest-severity finding a reviewer can raise. The top
of what a reviewer looks for is precisely what this template forces the author to declare. A
description organised as a branch narrative makes the reviewer reconstruct that from the diff.

## The description has other writers

CI rewrites the `Artifact URL` line after every build, and review bots append their own summary blocks.
A whole-body update prepared from an earlier draft silently reverts them: the request succeeds, returns
success, and reports nothing about what it overwrote. Read the live body immediately before writing,
change only your own lines, then read it back.

## Keep it true after every push

Nothing verifies a description, and every commit decays it. Re-read it against the diff after each
push and watch for:

- an identifier renamed in the code but not in the text;
- a claim superseded by a later commit on the same branch;
- a status or progress section that went stale;
- an absolute quantifier — «none», «all», «every» — that nobody actually enumerated;
- a claim about a group of items that was verified only on the first one.

Never publish a private customer identifier in an upstream description; grep the text before posting.

## Do not reference planning artifacts

No «Phase 2», «per the plan», or spec-section numbers. Planning documents are ephemeral and the
reference outlives them, leaving a pointer nobody can resolve. State the reason directly instead.

## Recognition signals of the wrong shape

- The sections are chronological — «Problem», «Change», «What I tried», «Tests».
- A `Tests` section. Test coverage is a review dimension, not consumer information; the reviewer runs
  the suite regardless.
- A behaviour change a consumer only meets at runtime, mentioned as a sub-bullet of a notes list rather
  than under `Breaking changes`.
- No `None` anywhere in a description for a change that plainly adds no GraphQL surface — the sections
  were dropped rather than answered.
- Prose explaining what an earlier revision of the branch did, above the sections describing what ships.

## Scope

Upstream GitHub pull requests in `vc-platform` and `vc-module-*`. Client-project pull requests have
their own template and their own conventions — do not carry the GraphQL sections there.
