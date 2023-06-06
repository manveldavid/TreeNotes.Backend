using FluentValidation;

namespace Application.Actions.Note.Queries.FromDescription
{
    public class FromDescriptionQueryValidator:AbstractValidator<FromDescriptionQuery>
    {
        public FromDescriptionQueryValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.Fragment).NotEmpty();
        }
    }
}
