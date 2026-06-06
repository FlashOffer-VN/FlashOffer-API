# Code Snippets for AI Prompts

This folder contains reusable code templates for generating new features.

## Available Templates

| Template | File | Use when |
|----------|------|----------|
| Entity | entity-template.md | Creating a new entity in Domain layer |
| DTOs | dto-template.md | Creating Response, Create, Update DTOs |
| Controller | controller-template.md | Creating API controller |
| Validator | validator-template.md | Creating FluentValidation validators |
| AutoMapper | automapper-template.md | Adding mappings to MappingProfile |
| Repository | repository-template.md | Creating custom repository (if needed) |
| Migration | migration-template.md | Creating EF Core migrations |

## How to Use

When writing a prompt, reference these templates by name.

Example: "Create a Category feature using the entity-template, dto-template, controller-template, validator-template, and automapper-template"

## Template Variables

All templates use {EntityName} as a placeholder. Replace with your actual entity name (e.g., Category, Product, Order).

## Adding New Templates

1. Create a new .md file in this folder
2. Use the same format as existing templates
3. Update this README
