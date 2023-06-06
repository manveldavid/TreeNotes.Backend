using MediatR;

namespace Application.Actions.User.Queries.UserId
{
    public class UserIdQuery : IRequest<Guid>
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
