using FluentValidation;

namespace Application.Actions.User.Commands.Create
{
    public class CreateCommandValidator:AbstractValidator<CreateCommand>
    {
        public CreateCommandValidator() 
        {
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Login).NotEmpty();
        }
    }
}
