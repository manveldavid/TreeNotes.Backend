using MediatR;

namespace Application.Actions.User.Queries.UserExist
{
    public class UserExistQuery : IRequest<bool>
    {
        public string Login { get; set; }
    }
}
