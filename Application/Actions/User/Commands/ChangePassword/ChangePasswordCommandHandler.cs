using Application.Common.Exceptions;
using Application.Common.Workers;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.User.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler:DbRequestHandler<TreeNoteUser>, IRequestHandler<ChangePasswordCommand>
    {
        public ChangePasswordCommandHandler(IDbContext<TreeNoteUser> context) : base(context) { }
        public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await TreeNoteUserWorker.Finder
                .UserFromLoginPassword(_dbContext, request.Login, request.OldPassword)
                .ToListAsync();
            var user = result.FirstOrDefault();

            #region Checks

            var resultUserExist = await TreeNoteUserWorker.Finder
                .UserFromLogin(_dbContext, request.Login)
                .ToListAsync();
            var userExist = resultUserExist.FirstOrDefault();
            if (userExist != null && user == null)
            {
                throw new TreeNoteUserWrongPasswordException(nameof(TreeNoteUser), $"log:{request.Login} pas:{request.OldPassword}");
            }

            if (user == null)
            {
                throw new EntityNotFoundException(nameof(TreeNoteUser), $"log:{request.Login} pas:{request.OldPassword}");
            }

            #endregion

            user.Code = TreeNoteUserWorker.Encoder
                .CodeFromLoginPassword(request.Login, request.NewPassword);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
