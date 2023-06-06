using Domain;
using MediatR;

namespace Application.Actions.Note.Queries.Childs
{
    public class ChildsQuery : IRequest<ICollection<TreeNote>>
    {
        public Guid UserId { get; set; }
        public Guid NoteId { get; set; }
    }
}
