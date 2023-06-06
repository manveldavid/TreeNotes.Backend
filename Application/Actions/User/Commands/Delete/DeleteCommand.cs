using MediatR;

namespace Application.Actions.User.Commands.Delete
{
    public class DeleteCommand:IRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
