using Domain;
using MediatR;

namespace Application.Actions.Note.Queries.FromDescription
{
    public class FromDescriptionQuery:IRequest<ICollection<TreeNote>>
    {
        public Guid UserId { get; set; }
        public string Fragment { get;  set; }
    }
}
