using Application.Interfaces;
using Application.Common.Workers;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.Note.Queries.FromDate
{
    public class FromDateQueryHandler : DbRequestHandler<TreeNote>, IRequestHandler<FromDateQuery, ICollection<TreeNote>>
    {
        public FromDateQueryHandler(IDbContext<TreeNote> dbContext) : base(dbContext) { }
        public async Task<ICollection<TreeNote>> Handle(
            FromDateQuery request, CancellationToken cancellationToken)
        {
            var result = TreeNoteWorker.Finder
                .NotesFromDate(_dbContext, request.UserId, request.Date);
            var notes = await result.ToListAsync();

            return notes;
        }
    }
}
