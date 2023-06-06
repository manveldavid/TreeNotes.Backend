using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Common.Workers;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.Note.Queries.FromId
{
    public class FromIdQueryHandler : DbRequestHandler<TreeNote>, IRequestHandler<FromIdQuery, TreeNote>
    {
        public FromIdQueryHandler(IDbContext<TreeNote> dbContext) : base(dbContext) { }
        public async Task<TreeNote> Handle(FromIdQuery request, CancellationToken cancellationToken)
        {
            var result = await TreeNoteWorker.Finder
                .NoteFromId(_dbContext, request.NoteId)
                .ToListAsync();
            var note = result.FirstOrDefault();

            #region Checks

            if(note == null)
            {
                throw new EntityNotFoundException(nameof(TreeNote), request.NoteId);
            }

            if(!(note.Creator == request.UserId || note.User == request.UserId || note.Share == true))
            {
                throw new EntityDeniedPermissionException(nameof(TreeNote), request.NoteId);
            }

            #endregion

            return note;
        }
    }
}
