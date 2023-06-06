using FluentValidation;

namespace Application.Actions.User.Commands.ChangeLogin
{
    public class ChangeLoginCommandValidator : AbstractValidator<ChangeLoginCommand>
    {
        public ChangeLoginCommandValidator() 
        {
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.OldLogin).NotEmpty();
            RuleFor(x => x.NewLogin).NotEmpty();
        }
    }
}
