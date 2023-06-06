using Application.Common.Exceptions;
using Application.Common.Workers;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.User.Queries.UserExist
{
    public class UserExistQueryHandler : DbRequestHandler<TreeNoteUser>, IRequestHandler<UserExistQuery, bool>
    {
        public UserExistQueryHandler(IDbContext<TreeNoteUser> context) : base(context) { }
        public async Task<bool> Handle(UserExistQuery request, CancellationToken cancellationToken)
        {
            var result = await TreeNoteUserWorker.Finder
                .UserFromLogin(_dbContext, request.Login)
                .ToListAsync();
            var user = result.FirstOrDefault();

            return user != null;
        }
    }
}
