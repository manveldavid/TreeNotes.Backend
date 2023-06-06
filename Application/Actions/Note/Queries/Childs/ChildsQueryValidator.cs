using FluentValidation;

namespace Application.Actions.Note.Queries.Childs
{
    public class ChildsQueryValidator:AbstractValidator<ChildsQuery>
    {
        public ChildsQueryValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.NoteId).NotEqual(Guid.Empty);
        }
    }
}
