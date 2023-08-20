using FluentValidation;

namespace Application.Actions.Note.Queries.FromDate
{
    public class FromDateQueryValidator:AbstractValidator<FromDateQuery>
    {
        public FromDateQueryValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
        }
    }
}
