using MediatR;

namespace Application.Actions.User.Commands.Create
{
    public class CreateCommand:IRequest<Guid>
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
