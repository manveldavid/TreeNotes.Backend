using FluentValidation;

namespace Application.Actions.Note.Queries.ParentCheck
{
    public class ParentCheckQueryValidator:AbstractValidator<ParentCheckQuery>
    {
        public ParentCheckQueryValidator() 
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.NoteId).NotEqual(Guid.Empty);
        }
    }
}
