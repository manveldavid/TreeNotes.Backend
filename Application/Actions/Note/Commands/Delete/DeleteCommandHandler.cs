using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Common.Workers;
using Domain;
using MediatR;

namespace Application.Actions.Note.Commands.Delete
{
    public class DeleteCommandHandler : DbRequestHandler<TreeNote>, IRequestHandler<DeleteCommand>
    {
        public DeleteCommandHandler(IDbContext<TreeNote> dbContext) : base(dbContext) { }
        public async Task Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var note = TreeNoteWorker.Finder.NoteFromId(_dbContext, request.NoteId).FirstOrDefault();

            #region Checks

            if (note == null)
            {
                throw new EntityNotFoundException(nameof(TreeNote), request.NoteId);
            }
            
            if (!(note.Creator == request.UserId))
            {
                throw new EntityDeniedPermissionException(nameof(TreeNote), request.NoteId);
            }

            var parentsChecked = note.Parent == Guid.Empty ?
                false : TreeNoteWorker.Finder.AllParentsFromId(_dbContext, note.Parent).Any(p => p.Check == true);
            if (parentsChecked)
            {
                throw new TreeNoteParentCheckException(nameof(TreeNote), request.NoteId);
            }

            #endregion

            var noteChilds = TreeNoteWorker.Finder.AllChildsFromId(_dbContext, note.Id);
            if (noteChilds.Count() > 0)
            {
                foreach (var child in noteChilds)
                {
                    _dbContext.Set.Remove(child);
                }
            }
            _dbContext.Set.Remove(note);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
