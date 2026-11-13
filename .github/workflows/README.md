# Workflows

GitHub Actions workflows for this repo. This document covers the supply-chain security setup; for what each workflow does, see the individual files.

## Supply-chain security: pinned third-party actions

Every third-party `uses:` reference in this repo (anything not under `VirtoCommerce/*`) is pinned to a full 40-character commit SHA with a trailing `# tag` comment, per the [GitHub Actions hardening guide](https://docs.github.com/en/actions/security-for-github-actions/security-guides/security-hardening-for-github-actions#using-third-party-actions). Tags are mutable; SHAs are not.

```yaml
# Correct
uses: actions/checkout@de0fac2e4500dabe0009e67214ff5f5447ce83dd # v6

# Rejected by CI
uses: actions/checkout@v6
```

### How updates happen

- **Dependabot** ([`.github/dependabot.yml`](../dependabot.yml)) scans `.github/workflows/` weekly. When upstream cuts a new tag, it opens a grouped PR bumping the SHA + trailing comment.
- **Pin-check CI** ([`pin-check.yml`](pin-check.yml)) runs `pinact run -check` on every PR that touches workflows. PRs with unpinned third-party `uses:` lines fail.
- **Scope** is configured in [`.pinact.yaml`](../../.pinact.yaml) at the repo root — `VirtoCommerce/*` is intentionally ignored (internal, not third-party).

### For contributors

- When adding a new third-party action, write the SHA, not the tag. Quick lookup:

  ```sh
  gh api repos/OWNER/REPO/commits/TAG --jq '.sha'
  ```

- `VirtoCommerce/vc-github-actions/<dir>@master` and other `VirtoCommerce/*` refs remain version-/branch-pinned as before — only non-VirtoCommerce owners require SHA pinning.
