using Domain;
using MediatR;

namespace Application.Actions.Note.Queries.FromId
{
    public class FromIdQuery:IRequest<TreeNote>
    {
        public Guid UserId { get; set; }
        public Guid NoteId { get; set; }
    }
}
