using FluentValidation;

namespace Application.Actions.User.Queries.UserId
{
    public class UserIdQueryValidator:AbstractValidator<UserIdQuery>
    {
        public UserIdQueryValidator()
        {
            RuleFor(x => x.Login).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
