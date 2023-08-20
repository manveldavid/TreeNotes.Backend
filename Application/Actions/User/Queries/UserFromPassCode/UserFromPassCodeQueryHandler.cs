
using Application.Actions.Note.Queries.FromDate;
using Application.Common.Workers;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.User.Queries.UserFromPassCode
{
    public class UserFromPassCodeQueryHandler : DbRequestHandler<TreeNoteUser>, IRequestHandler<UserFromPassCodeQuery, TreeNoteUser>
    {
        public UserFromPassCodeQueryHandler(IDbContext<TreeNoteUser> dbContext) : base(dbContext) { }
        public async Task<TreeNoteUser> Handle(
            UserFromPassCodeQuery request, CancellationToken cancellationToken)
        {
            var result = await TreeNoteUserWorker.Finder.UserFromPasscode(_dbContext, request.PassCode).ToListAsync();
            var user = result.FirstOrDefault();

            return user;
        }
    }
}
