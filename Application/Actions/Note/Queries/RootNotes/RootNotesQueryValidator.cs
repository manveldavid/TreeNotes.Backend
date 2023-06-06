using FluentValidation;

namespace Application.Actions.Note.Queries.RootNotes
{
    public class RootNotesQueryValidator:AbstractValidator<RootNotesQuery>
    {
        public RootNotesQueryValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
        }
    }
}
