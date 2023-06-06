using FluentValidation;

namespace Application.Actions.Note.Queries.FromTitle
{
    public class FromTitleQueryValidator:AbstractValidator<FromTitleQuery>
    {
        public FromTitleQueryValidator() 
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.Fragment).NotEmpty();
        }
    }
}
