using BookStore.Commands;
using BookStore.Models;
using FluentValidation;


namespace BookStore.Validation
{
    public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>

    {
        public CreateAuthorCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Author name cannot be empty");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Author surname cannot be empty");
        }
    }
}
