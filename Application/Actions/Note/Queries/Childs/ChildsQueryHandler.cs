using Application.Interfaces;
using Application.Common.Workers;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.Note.Queries.Childs
{
    public class ChildsQueryHandler : DbRequestHandler<TreeNote>, IRequestHandler<ChildsQuery, ICollection<TreeNote>>
    {
        public ChildsQueryHandler(IDbContext<TreeNote> dbContext) : base(dbContext) { }
        public async Task<ICollection<TreeNote>> Handle(
            ChildsQuery request, CancellationToken cancellationToken)
        {
            var result = TreeNoteWorker.Finder
                .FirstChildsFromId(_dbContext, request.NoteId);
            var childs = await result.Where(c => (c.Creator == request.UserId) ||
                (c.User == request.UserId) || (c.Share == true)).ToListAsync();

            return childs;
        }
    }
}
