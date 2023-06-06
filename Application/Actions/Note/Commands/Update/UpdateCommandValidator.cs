using FluentValidation;

namespace Application.Actions.Note.Commands.Update
{
    public class UpdateCommandValidator:AbstractValidator<UpdateCommand>
    {
        public UpdateCommandValidator() 
        { 
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.NoteId).NotEqual(Guid.Empty);
        }
    }
}
