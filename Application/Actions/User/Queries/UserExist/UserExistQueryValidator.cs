using FluentValidation;

namespace Application.Actions.User.Queries.UserExist
{
    public class UserExistQueryValidator:AbstractValidator<UserExistQuery>
    {
        public UserExistQueryValidator() 
        {
            RuleFor(x => x.Login).NotEmpty();
        }
    }
}
