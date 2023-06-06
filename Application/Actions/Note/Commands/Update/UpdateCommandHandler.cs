using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Common.Workers;
using Domain;
using MediatR;

namespace Application.Actions.Note.Commands.Update
{
    public class UpdateCommandHandler : DbRequestHandler<TreeNote>, IRequestHandler<UpdateCommand>
    {
        public UpdateCommandHandler(IDbContext<TreeNote> dbContext) : base(dbContext) { }
        public async Task Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var note = TreeNoteWorker.Finder.NoteFromId(_dbContext, request.NoteId).FirstOrDefault();

            #region Checks

            if (note == null)
            {
                throw new EntityNotFoundException(nameof(TreeNote), request.NoteId);
            }

            if (!(note.Creator == request.UserId || note.User == request.UserId))
            {
                throw new EntityDeniedPermissionException(nameof(TreeNote), request.NoteId);
            }

            var parentsChecked = note.Parent == Guid.Empty ? 
                false : TreeNoteWorker.Finder.AllParentsFromId(_dbContext, note.Parent).Any(p => p.Check == true);
            if (parentsChecked)
            {
                throw new TreeNoteParentCheckException(nameof(TreeNote), request.NoteId);
            }

            var ownerPropsChange =
                !(request.Share == null &&
                request.User == null &&
                request.Parent == null &&
                request.Number == null &&
                request.Title == null);
            if (ownerPropsChange && request.UserId == note.User) 
            {
                throw new EntityDeniedPermissionException(nameof(TreeNote), note.Id);
            }

            #endregion

            var oldParents = TreeNoteWorker.Finder.AllParentsFromId(_dbContext, note.Parent);

            note.Parent = request.Parent?? note.Parent;
            note.User = request.User ?? note.User;
            note.Description = request.Description ?? note.Description;
            note.Title = request.Title ?? note.Title;
            note.Share = request.Share ?? note.Share;
            note.Check = request.Check ?? note.Check;
            note.Weight = request.Weight ?? note.Weight;
            note.Number = request.Number ?? note.Number;

            var newParents = TreeNoteWorker.Finder.AllParentsFromId(_dbContext, note.Parent);
            var allParents = oldParents.Concat(newParents);

            note.LastEdit = DateTime.Now;

            foreach (var parent in allParents)
            {
                parent.LastEdit = note.LastEdit;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
