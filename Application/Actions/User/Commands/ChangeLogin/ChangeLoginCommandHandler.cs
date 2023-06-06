using Application.Common.Exceptions;
using Application.Common.Workers;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.User.Commands.ChangeLogin
{
    public class ChangeLoginCommandHandler:DbRequestHandler<TreeNoteUser>, IRequestHandler<ChangeLoginCommand>
    {
        public ChangeLoginCommandHandler(IDbContext<TreeNoteUser> context) : base(context) { }
        public async Task Handle(ChangeLoginCommand request, CancellationToken cancellationToken)
        {
            var result = await TreeNoteUserWorker.Finder
                .UserFromLoginPassword(_dbContext, request.OldLogin, request.Password)
                .ToListAsync();
            var user = result.FirstOrDefault();

            #region Checks

            var resultUserExist = await TreeNoteUserWorker.Finder
                .UserFromLogin(_dbContext, request.OldLogin)
                .ToListAsync();
            var userExist = resultUserExist.FirstOrDefault();
            if (userExist != null && user == null)
            {
                throw new TreeNoteUserWrongPasswordException(nameof(TreeNoteUser), $"log:{request.OldLogin} pas:{request.Password}");
            }

            if (user == null)
            {
                throw new EntityNotFoundException(nameof(TreeNoteUser), $"log:{request.OldLogin} pas:{request.Password}");
            }

            #endregion

            user.Login = request.NewLogin;
            user.Code = TreeNoteUserWorker.Encoder
                .CodeFromLoginPassword(request.NewLogin, request.Password);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
