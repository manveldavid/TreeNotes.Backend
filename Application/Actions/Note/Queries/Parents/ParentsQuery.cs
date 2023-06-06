using Domain;
using MediatR;

namespace Application.Actions.Note.Queries.Parents
{
    public class ParentsQuery:IRequest<ICollection<TreeNote>>
    {
        public Guid UserId { get; set; }
        public Guid NoteId { get; set; }
    }
}
