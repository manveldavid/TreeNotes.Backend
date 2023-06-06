using FluentValidation;

namespace Application.Actions.Note.Commands.Create
{
    public class CreateCommandValidator:AbstractValidator<CreateCommand>
    {
        public CreateCommandValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
        }
    }
}
