using Application.Actions.User.Queries.UserId;
using MediatR;
using Serilog;

namespace Application.Common.Behaviors
{
    public class LoggerBehavior<Request, Responce> :
    IPipelineBehavior<Request, Responce> where Request : IRequest<Responce>
    {
        public LoggerBehavior() { }
        public async Task<Responce> Handle(Request request,
            RequestHandlerDelegate<Responce> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(Request).Name;
            if (requestName != typeof(UserIdQuery).Name)
            {
                Log.Information("Notes Request: {Name} {@Request}",
                    requestName, request);
            }
            var response = await next();
            return response;
        }
    }
}
