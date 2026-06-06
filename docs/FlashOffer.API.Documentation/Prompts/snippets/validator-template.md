# Validator Template

namespace FlashOffer.API.Application.Validators;

using FluentValidation;
using FlashOffer.API.Application.DTOs;

public class Create{EntityName}Validator : AbstractValidator<Create{EntityName}Dto>
{
    public Create{EntityName}Validator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");
    }
}

public class Update{EntityName}Validator : AbstractValidator<Update{EntityName}Dto>
{
    public Update{EntityName}Validator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");
    }
}
