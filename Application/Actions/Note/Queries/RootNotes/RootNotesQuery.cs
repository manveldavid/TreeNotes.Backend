using Domain;
using MediatR;

namespace Application.Actions.Note.Queries.RootNotes
{
    public class RootNotesQuery:IRequest<ICollection<TreeNote>>
    {
        public Guid UserId { get; set; }
    }
}
