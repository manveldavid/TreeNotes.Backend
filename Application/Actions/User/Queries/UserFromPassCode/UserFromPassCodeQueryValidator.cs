using FluentValidation;

namespace Application.Actions.User.Queries.UserFromPassCode
{
    public class UserFromPassCodeQueryValidator : AbstractValidator<UserFromPassCodeQuery>
    {
        public UserFromPassCodeQueryValidator()
        {
            RuleFor(x => x.PassCode).NotEmpty();
        }
    }
}
