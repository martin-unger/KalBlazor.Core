# Contributing

By contributing to this project, you certify that:

- You have the right to submit the contribution
- The contribution is your original work or appropriately licensed
- You agree that your contribution may be distributed under the MIT License

## Developer Certificate of Origin (DCO)

Please sign your commits using:

git commit -s

This adds a Signed-off-by line to your commit message.

## Branching and releases

All changes must be made through a pull request into `main`.

The source branch name controls the next package version after the pull request is merged:

| Branch prefix | Version bump | Example |
| --- | --- | --- |
| `feat/` or `feature/` | Minor | `feat/drawer-overlay` |
| `fix/`, `bugfix/`, `hotfix/`, `perf/`, `refactor/` | Patch | `fix/drawer-width` |
| `breaking/` | Major | `breaking/layout-api` |

After a pull request is merged into `main`, GitHub Actions automatically:

1. calculates the next semantic version,
2. writes that version into the project file,
3. creates an automated release commit,
4. restores, builds, and tests the solution,
5. creates the NuGet package with the same version,
6. publishes `SoftwareThingies.KalBlazor.Core` to NuGet.org,
7. creates the corresponding Git tag, for example `v0.2.0`.

Direct commits to `main` should be disabled in the repository settings. If one is ever allowed, the release pipeline treats it as a patch release.

The repository starts at version `0.0.0`. If no release tag exists yet, the pipeline uses the version currently stored in the project file as its baseline.

## Pull request expectations

- Use one of the supported branch prefixes above.
- Keep pull requests focused enough that the version bump is unambiguous.
- Ensure the CI workflow is green before merging.
- If a change is intentionally breaking, use a `breaking/` branch so the automated release receives a major version bump.
