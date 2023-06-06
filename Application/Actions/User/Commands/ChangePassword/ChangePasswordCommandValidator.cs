using FluentValidation;

namespace Application.Actions.User.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator:AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator() 
        {
            RuleFor(x => x.NewPassword).NotEmpty();
            RuleFor(x => x.OldPassword).NotEmpty();
            RuleFor(x => x.Login).NotEmpty();
        }
    }
}
