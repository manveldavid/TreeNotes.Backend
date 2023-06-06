using MediatR;

namespace Application.Actions.Note.Commands.Create
{
    public class CreateCommand:IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public Guid Parent { get; set; }
    }
}
