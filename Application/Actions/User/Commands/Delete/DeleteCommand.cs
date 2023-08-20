using MediatR;

namespace Application.Actions.User.Commands.Delete
{
    public class DeleteCommand:IRequest
    {
        public string Passcode { get; set; }
    }
}
