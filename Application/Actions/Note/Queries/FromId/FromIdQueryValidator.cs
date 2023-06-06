using FluentValidation;

namespace Application.Actions.Note.Queries.FromId
{
    public class FromIdQueryValidator:AbstractValidator<FromIdQuery>
    {
        public FromIdQueryValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.NoteId).NotEqual(Guid.Empty);
        }
    }
}
