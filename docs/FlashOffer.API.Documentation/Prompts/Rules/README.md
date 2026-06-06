# Prompts / Rules

Thư mục này chứa các quy tắc và yêu cầu dùng chung của project mà mọi prompt phải tuân theo trước khi gửi cho model.

Mục đích:
- Tập trung các constraint (code style, security, response schema, dependency rules) để LLM áp dụng nhất quán.
- Giúp compose prompt nhanh: luôn ghép `00-context.md` + files trong `Rules/` + prompt cụ thể.

Files chính:
- `project-requirements.md` — yêu cầu project-level (framework, API format, conventions)
- `code-style.md` — quy tắc code, naming, DI, layering
- `security-rules.md` — tránh commit secret, validation, auth expectations
- `prompt-guidelines.md` — cách viết prompt, snippets usage, yêu cầu trả JSON theo `00-ai-rules.md`
- `feature-rules.md` — quy tắc cho prompt feature files và folder structure

Quy trình ngắn:
1. Luôn include `00-ai-rules.md` + `Rules/*` khi gửi prompt.
2. Yêu cầu `RETURN_JSON_ONLY: true` nếu cần patch tự động.
3. Review manual trước khi commit các thay đổi.
