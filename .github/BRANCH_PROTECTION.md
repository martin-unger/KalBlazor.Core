# Recommended protection for `main`

Configure this as a branch protection rule or repository ruleset targeting `main`.

## Required settings

- Require a pull request before merging.
- Require status checks before merging.
- Require the `build` check from the `Build and publish NuGet package` workflow.
- Require branches to be up to date before merging.
- Block force pushes.
- Block branch deletion.
- Restrict direct pushes to `main`.

## Strongly recommended

- Require at least 1 approval before merging.
- Dismiss stale approvals when new commits are pushed.
- Require conversation resolution before merging.
- Do not allow bypassing the above settings except for repository administrators who genuinely need emergency access.

## Why these settings matter here

The release pipeline publishes a NuGet package automatically on every successful push to `main`.
Protecting `main` ensures that every published version has first passed through review, build, and test checks.

The workflow also creates one automated release commit before tagging so that the project file contains the released version.
If direct pushes to `main` are restricted, allow the GitHub Actions identity used by this repository to create that specific automated commit, or use a dedicated automation token/app for the release step.

## NuGet secret

The workflow expects an Actions secret named `NUGET_API_KEY`.
If this secret is stored at organization level with access limited to selected repositories, this repository must be included in that allow-list.
