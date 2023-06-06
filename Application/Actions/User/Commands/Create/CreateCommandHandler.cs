using Application.Common.Exceptions;
using Application.Common.Workers;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.User.Commands.Create
{
    public class CreateCommandHandler : DbRequestHandler<TreeNoteUser>, IRequestHandler<CreateCommand, Guid>
    {
        public CreateCommandHandler(IDbContext<TreeNoteUser> context) : base(context) { }
        public async Task<Guid> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            var result = await TreeNoteUserWorker.Finder
                .UserFromLogin(_dbContext, request.Login)
                .ToListAsync();
            var user = result.FirstOrDefault();

            #region Checks

            if (user != null)
            {
                throw new TreeNoteUserExistException(nameof(TreeNoteUser), request.Login);
            }

            #endregion

            var code = TreeNoteUserWorker.Encoder.CodeFromLoginPassword(request.Login, request.Password);

            var newUser = new TreeNoteUser
            {
                Code = code,
                Login = request.Login,
                Id = Guid.NewGuid(),
            };

            await _dbContext.Set.AddAsync(newUser, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return newUser.Id;
        }
    }
}
