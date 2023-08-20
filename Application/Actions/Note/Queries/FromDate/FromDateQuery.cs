using Domain;
using MediatR;

namespace Application.Actions.Note.Queries.FromDate
{
    public class FromDateQuery : IRequest<ICollection<TreeNote>>
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
    }
}