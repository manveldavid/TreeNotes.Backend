using Application.Common.Workers;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.Note.Queries.Parents
{
    public class ParentsQueryHandler : DbRequestHandler<TreeNote>, IRequestHandler<ParentsQuery, ICollection<TreeNote>>
    {
        public ParentsQueryHandler(IDbContext<TreeNote> dbContext) : base(dbContext) { }
        public async Task<ICollection<TreeNote>> Handle(
            ParentsQuery request, CancellationToken cancellationToken)
        {
            var result = await TreeNoteWorker.Finder
                .AllParentsFromId(_dbContext, request.NoteId)
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

            return result.Skip(1).ToList();
        }
    }
}
