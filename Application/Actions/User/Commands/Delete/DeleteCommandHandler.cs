using Application.Common.Exceptions;
using Application.Common.Workers;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.User.Commands.Delete
{
    public class DeleteCommandHandler:DbRequestHandler<TreeNoteUser>, IRequestHandler<DeleteCommand>
    {
        public DeleteCommandHandler(IDbContext<TreeNoteUser> context) : base(context) { }
        public async Task Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var result = await TreeNoteUserWorker.Finder
                .UserFromPasscode(_dbContext, request.Passcode)
                .ToListAsync();
            var user = result.FirstOrDefault();

            #region Checks

            if (user == null)
            {
                throw new EntityNotFoundException(nameof(TreeNoteUser), $"passcode:{request.Passcode}");
            }

            #endregion

            _dbContext.Set.Remove(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
