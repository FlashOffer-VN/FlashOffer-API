# Pull Request Template

## Summary
Brief description of the change and why it is needed.

## Changes
- What changed
- Files added/modified
- Any migrations or package updates

## Checklist
- [ ] I ran `dotnet build` and `dotnet test` locally
- [ ] I updated documentation in `docs/FlashOffer.API.Documentation/` if applicable
- [ ] I updated `Directory.Packages.props` and explained version choices in PR when package changes were made
- [ ] New validators that need localization follow the DI ordering (AddLocalization before AddControllers().AddFluentValidation())
- [ ] Integration tests seed data using `Factory.Server.Services.CreateScope()` and deterministic InMemory DB name

## Notes for reviewers
Add any guidance for reviewers (e.g., how to run migrations, special setup)