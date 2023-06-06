using FluentValidation;

namespace Application.Actions.User.Commands.Delete
{
    public class DeleteCommandValidator:AbstractValidator<DeleteCommand>
    {
        public DeleteCommandValidator() 
        {
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Login).NotEmpty();
        }
    }
}
