using MediatR;

namespace Application.Actions.Note.Queries.ParentCheck
{
    public class ParentCheckQuery:IRequest<bool>
    {
        public Guid NoteId { get; set; }
        public Guid UserId { get; set; }
    }
}
