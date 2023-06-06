using MediatR;

namespace Application.Actions.Note.Commands.Delete
{
    public class DeleteCommand:IRequest
    {
        public Guid NoteId { get; set; }
        public Guid UserId { get; set; }
    }
}
