using BookStore.Commands;
using FluentValidation;

namespace BookStore.Validation
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Book name cannot be empty");

            RuleFor(x => x.PublishYear).NotEmpty().WithMessage("Book name cannot be empty").
                Length(4).WithMessage("Publish year invalid input" );

            RuleFor(x => x.Author.Id).NotEmpty().WithMessage("Author ID cannot be empty")
                .GreaterThanOrEqualTo(0).WithMessage("Invalid Id entry");

            RuleFor(x => x.Author.Name).NotEmpty().WithMessage("Author name cannot be empty");

            RuleFor(x => x.Author.Surname).NotEmpty().WithMessage("Author surname cannot be empty");
        }
    }
}
