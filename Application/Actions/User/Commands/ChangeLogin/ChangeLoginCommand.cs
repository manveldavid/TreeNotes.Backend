using MediatR;

namespace Application.Actions.User.Commands.ChangeLogin
{
    public class ChangeLoginCommand:IRequest
    {
        public string OldLogin { get; set; }
        public string NewLogin { get; set; }
        public string Password { get; set; }
    }
}
