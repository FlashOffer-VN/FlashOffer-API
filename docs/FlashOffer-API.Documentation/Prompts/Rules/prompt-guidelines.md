# Prompt Guidelines (for AI)

- Always include `00-context.md` and `00-ai-rules.md` plus relevant files from `Rules/` when sending any prompt.
- Reference snippets by name (e.g. `controller-variants`, `di-registration`) to encourage reuse.
- Provide clear acceptance criteria: files to create, files to change, tests to add.
- If automation desired, require `RETURN_JSON_ONLY: true` and specify the JSON schema (see `00-ai-rules.md`).
- Ask for small, incremental patches (one feature per prompt) to simplify review.
- Request code comments only where necessary; keep patches minimal and focused.
- Require UTF-8 no BOM outputs and avoid NUL chars in generated markdown/code.
