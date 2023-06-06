using Application.Interfaces;
using Application.Common.Workers;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.Note.Queries.FromDescription
{
    public class FromDescriptionQueryHandler : DbRequestHandler<TreeNote>, IRequestHandler<FromDescriptionQuery, ICollection<TreeNote>>
    {
        public FromDescriptionQueryHandler(IDbContext<TreeNote> dbContext) : base(dbContext) { }
        public async Task<ICollection<TreeNote>> Handle(
            FromDescriptionQuery request, CancellationToken cancellationToken)
        {
            var noteList = await TreeNoteWorker.Finder
                .NotesFromDescription(_dbContext, request.UserId, request.Fragment)
                .ToListAsync();

            return noteList;
        }
    }
}
