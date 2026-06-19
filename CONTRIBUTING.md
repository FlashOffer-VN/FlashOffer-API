# Contributing to FlashOffer.API (short)

This repository uses the detailed contributing guide under docs/FlashOffer.API.Documentation/CONTRIBUTING.md.

Before opening a PR:

- Run `dotnet build` and `dotnet test` and ensure tests pass.
- Follow the step-by-step API template in `docs/FlashOffer.API.Documentation/core/11-how-to-add-new-api.md`.
- If you change package versions, update `Directory.Packages.props` and explain the decision in the PR description.
- Ensure validators using localization are supported by calling `services.AddLocalization()` before `AddControllers().AddFluentValidation(...)`.

See `docs/FlashOffer.API.Documentation/` for full contribution and code conventions.
