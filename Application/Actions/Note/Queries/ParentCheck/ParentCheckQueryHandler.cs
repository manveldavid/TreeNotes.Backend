using Application.Common.Exceptions;
using Application.Common.Workers;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.Note.Queries.ParentCheck
{
    public class ParentCheckQueryHandler:DbRequestHandler<TreeNote>,IRequestHandler<ParentCheckQuery, bool>
    {
        public ParentCheckQueryHandler(IDbContext<TreeNote> context) : base(context) { }
        public async Task<bool> Handle(ParentCheckQuery request, CancellationToken cancellationToken)
        {
            var result = await TreeNoteWorker.Finder
                .AllParentsFromId(_dbContext, request.NoteId)
                .ToListAsync();
            var note = result.FirstOrDefault();

            #region Checks

            if (note == null)
            {
                throw new EntityNotFoundException(nameof(TreeNote), request.NoteId);
            }

            if (!(note.Creator == request.UserId || note.User == request.UserId || note.Share == true))
            {
                throw new EntityDeniedPermissionException(nameof(TreeNote), request.NoteId);
            }

            #endregion

            return result.Skip(1).Any(x => x.Check == true);
        }
    }
}
