using FluentValidation;

namespace Application.Actions.Note.Commands.Delete
{
    public class DeleteCommandValidator:AbstractValidator<DeleteCommand>
    {
        public DeleteCommandValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.NoteId).NotEqual(Guid.Empty);
        }
    }
}
