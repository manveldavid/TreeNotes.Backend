using Application.Common.Exceptions;
using Application.Common.Workers;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.Actions.Note.Commands.Create
{
    public class CreateCommandHandler: DbRequestHandler<TreeNote>, IRequestHandler<CreateCommand,Guid> 
    {
        public CreateCommandHandler(IDbContext<TreeNote> dbContext) : base(dbContext) { }

        public async Task<Guid> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            #region Checks

            var parentsChecked = request.Parent == Guid.Empty ?
                false : TreeNoteWorker.Finder.AllParentsFromId(_dbContext, request.Parent)
                .Any(p => p.Check == true);
            if (parentsChecked)
            {
                throw new TreeNoteParentCheckException(nameof(TreeNote), request.Parent);
            }

            if(request.Parent != Guid.Empty) 
            {
                var parentNote = TreeNoteWorker.Finder
                    .NoteFromId(_dbContext, request.Parent)
                    .FirstOrDefault();
                if(parentNote == null)
                {
                    throw new EntityNotFoundException(nameof(TreeNote), request.Parent);
                }

                if (!(parentNote.Creator == request.UserId || parentNote.User == request.UserId))
                {
                    throw new EntityDeniedPermissionException(nameof(TreeNote), request.Parent);
                }
            }

            #endregion

            var newNote = new TreeNote
            {
                Creation = DateTime.Today,
                LastEdit = DateTime.Today,
                Id = Guid.NewGuid(),
                User = Guid.Empty,
                Creator = request.UserId,
                Parent = request.Parent,
                Check = false,
                Weight = 1,
                Description = string.Empty,
                Title = string.Empty,
                Share = false,
                Number = 0
            };

            await _dbContext.Set.AddAsync(newNote, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return newNote.Id;
        }
    }
}
