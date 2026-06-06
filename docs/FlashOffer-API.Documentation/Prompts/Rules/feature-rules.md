# Feature Prompt Rules

This file defines the rule set for feature prompt files in the `docs/FlashOffer-API.Documentation/Prompts` folder.

## Where feature prompts go
- Feature prompt files should be stored directly under `docs/FlashOffer-API.Documentation/Prompts/`.
- Use a numeric prefix for ordering, for example `01-Example-Prompt.md`.
- Keep file names short, descriptive, and consistent.

## Structure of a feature prompt
Each feature prompt should include:
1. A clear title
2. A short goal statement
3. Required output or files to create/change
4. Special constraints or rules to follow
5. A prompt text section that can be used directly by the assistant
6. `RETURN_JSON_ONLY: true` when the prompt is intended for automated patch generation

## Example structure
```md
# Example Prompt: Add Category feature

## Goal
Add Category management feature following project conventions.

## Requirements
- Create Category entity, DTOs, validator, controller.
- Use `ApiResponse<T>` / `ApiControllerBase`.
- Add `GET /api/v1/category/by-slug/{slug}`.

## Prompt
"Add a Category feature..."

RETURN_JSON_ONLY: true
```

## Rules for feature prompts
- Always follow the shared rules in `Prompts/Rules/*`.
- Reference reusable snippets when relevant (e.g. `controller-variants`, `di-registration`).
- Do not include implementation details that violate project layering.
- Keep prompts focused on one feature at a time.
- Prefer explicit acceptance criteria and expected files.
- Do not include secrets or environment-specific values.
- Create a feature note file under `Prompts/Features/` for each prompt feature executed.

## Notes
- This file is the canonical rule set for prompt feature files.
- If you need a feature prompt note file, create it under `Prompts/` with the same numeric-prefix convention.
