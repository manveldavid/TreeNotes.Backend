using Application.Common.Workers;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.Note.Queries.RootNotes
{
    public class RootNotesQueryHandler : DbRequestHandler<TreeNote>, IRequestHandler<RootNotesQuery, ICollection<TreeNote>>
    {
        public RootNotesQueryHandler(IDbContext<TreeNote> dbContext) : base(dbContext) { }
        public async Task<ICollection<TreeNote>> Handle(
            RootNotesQuery request, CancellationToken cancellationToken)
        {
            var noteList = await TreeNoteWorker.Finder
                .RootNotesFromUserId(_dbContext, request.UserId).ToListAsync();
            return noteList;
        }
    }
}
