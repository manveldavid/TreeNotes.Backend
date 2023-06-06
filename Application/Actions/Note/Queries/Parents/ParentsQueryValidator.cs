using FluentValidation;

namespace Application.Actions.Note.Queries.Parents
{
    public class ParentsQueryValidator:AbstractValidator<ParentsQuery>
    {
        public ParentsQueryValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.NoteId).NotEqual(Guid.Empty);
        }
    }
}
