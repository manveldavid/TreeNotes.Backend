using MediatR;

namespace Application.Actions.User.Commands.ChangePassword
{
    public class ChangePasswordCommand:IRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Login { get; set; }
    }
}
