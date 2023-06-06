using Domain;
using MediatR;

namespace Application.Actions.Note.Queries.FromTitle
{
    public class FromTitleQuery:IRequest<ICollection<TreeNote>>
    {
        public Guid UserId { get; set; }
        public string Fragment { get; set; }
    }
}
