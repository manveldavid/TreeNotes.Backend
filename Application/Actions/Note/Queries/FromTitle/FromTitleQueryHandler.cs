using Application.Common.Workers;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.Note.Queries.FromTitle
{
    public class FromTitleQueryHandler : DbRequestHandler<TreeNote>, IRequestHandler<FromTitleQuery, ICollection<TreeNote>>
    {
        public FromTitleQueryHandler(IDbContext<TreeNote> dbContext) : base(dbContext) { }
        public async Task<ICollection<TreeNote>> Handle(
            FromTitleQuery request, CancellationToken cancellationToken)
        {
            var noteList = await TreeNoteWorker.Finder
                .NotesFromTitle(_dbContext, request.UserId, request.Fragment)
                .ToListAsync();

            return noteList;
        }
    }
}
