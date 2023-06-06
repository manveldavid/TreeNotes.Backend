using Application.Actions.Note.Commands.Update;
using Application.Common.Exceptions;
using Application.Common.Workers;
using Tests.Common;

namespace Tests.Application.Actions.Note.Commands
{
    public class UpdateCommandHandlerTests:TestBase
    {
        [Fact]
        public async Task UpdateNoteCommandHandler_Success_UserId()
        {
            //Arrange
            var handler = new UpdateCommandHandler(_notes);

            var userId = ContentFactory.UserC_Id;
            var note = TreeNoteWorker.Finder
                .NoteFromId(_notes, ContentFactory.NoteId_Parent_A)
                .FirstOrDefault();

            var title = "title";
            var description = "description";

            var appointedUserId = ContentFactory.UserB_Id;
            var parentId = ContentFactory.NoteId_Child_F;

            var isChecked = true;
            var isShared = true;

            var weight = 1000;

            var oldChecked = note.Check;

            //Act
            await handler.Handle(new UpdateCommand
            {
                UserId = userId,
                Description = description,
                Check = isChecked,
                NoteId = note.Id,
                Weight = weight,
                Title = title,
                User = appointedUserId,
                Share = isShared,
                Parent = parentId
            }, CancellationToken.None);

            //Assert
            Assert.NotNull(
                TreeNoteWorker.Finder
                .NoteFromId(_notes, note.Id)
                .FirstOrDefault(note =>

                note.User == appointedUserId &&
                note.Parent == parentId &&
                note.Share == isShared &&
                note.Check == isChecked &&
                note.Title == title &&
                note.Description == description &&
                note.Weight == weight));

            Assert.Null(
               TreeNoteWorker.Finder
               .NoteFromId(_notes, note.Id)
               .FirstOrDefault(note =>
               note.Check == oldChecked));

            var newParents = TreeNoteWorker.Finder.AllParentsFromId(_notes, note.Id);
            var newChilds = TreeNoteWorker.Finder.AllChildsFromId(_notes, parentId);

            Assert
                .True(newParents.Count() == 4);
            Assert
                .True(newChilds.Select(note => note.Id).Contains(note.Id));
            Assert
                .True(newParents.All(p => p.LastEdit == note.LastEdit));
            Assert
                .True(TreeNoteWorker.Finder.AllChildsFromId(_notes, note.Id)
                .All(c => c.Share != note.Share));
        }

        [Fact]
        public async Task UpdateNoteCommandHandler_Success_AppointedUserId()
        {
            //Arrange
            var handler = new UpdateCommandHandler(_notes);

            var userId = ContentFactory.UserA_Id;
            var note =
                TreeNoteWorker.Finder
                .NoteFromId(_notes, ContentFactory.NoteId_Parent_A)
                .FirstOrDefault();

            var oldChecked = note.Check;
            var oldDescription = note.Description;
            var oldWeight = note.Weight;

            var description = "description";

            var isChecked = true;

            var weight = 1000;

            //Act
            await handler.Handle(new UpdateCommand
            {
                NoteId = note.Id,
                UserId = userId,

                Description = description,
                Check = isChecked,
                Weight = weight,

            }, CancellationToken.None);

            //Assert
            var oldNote = TreeNoteWorker.Finder
                .NoteFromId(_notes, note.Id)
                .FirstOrDefault(note =>

                note.Check == oldChecked ||
                note.Description == oldDescription ||
                note.Weight == oldWeight);
            Assert.Null(oldNote);

            var newNote = TreeNoteWorker.Finder
                .NoteFromId(_notes, note.Id)
                .FirstOrDefault(note =>

                note.Check == isChecked &&
                note.Description == description &&
                note.Weight == weight);
            Assert.NotNull(newNote);

            var parents = TreeNoteWorker.Finder.AllParentsFromId(_notes, note.Id);
            Assert
                .True(parents.All(p => p.LastEdit == note.LastEdit));
        }

        [Fact]
        public async Task UpdateNoteCommandHandler_Fail_UserId()
        {
            //Arrange
            var handler = new UpdateCommandHandler(_notes);

            var userId = ContentFactory.UserB_Id;
            var note =
                TreeNoteWorker.Finder
                .NoteFromId(_notes, ContentFactory.NoteId_Parent_A)
                .FirstOrDefault();
            var oldNote = note;

            //Act
            //Assert
            await Assert.ThrowsAsync<EntityDeniedPermissionException>(async () =>
            await handler.Handle(new UpdateCommand
            {
                NoteId = note.Id,

                UserId = userId,

                Title = note.Title,
                Description = note.Description,
                Parent = note.Parent,
                Check = note.Check,
                Weight = note.Weight,
                Share = note.Share
            }, CancellationToken.None));

            Assert.NotNull(
                TreeNoteWorker.Finder
                .NoteFromId(_notes, note.Id)
                .FirstOrDefault(note =>

                note.User == oldNote.User &&
                note.Parent == oldNote.Parent &&
                note.Share == oldNote.Share &&
                note.Check == oldNote.Check &&
                note.Title == oldNote.Title &&
                note.Description == oldNote.Description &&
                note.Weight == oldNote.Weight));


            var parents = TreeNoteWorker.Finder.AllParentsFromId(_notes, note.Id);
            var childs = TreeNoteWorker.Finder.AllChildsFromId(_notes, note.Id);

            Assert
                .True(parents.Count() == 2);
            Assert
                .True(childs.Count() == 3);
            Assert
                .True(note.LastEdit == DateTime.Today);
        }

        [Fact]
        public async Task UpdateNoteCommandHandler_Fail_AppointedUserId()
        {
            //Arrange
            var handler = new UpdateCommandHandler(_notes);

            var userId = ContentFactory.UserA_Id;
            var note = TreeNoteWorker.Finder
                .NoteFromId(_notes, ContentFactory.NoteId_Parent_A)
                .FirstOrDefault();

            var title = "title";
            var oldNote = note;

            //Act
            //Assert
            await Assert.ThrowsAsync<EntityDeniedPermissionException>(async () =>
            await handler.Handle(new UpdateCommand
            {
                NoteId = note.Id,

                Title = title,
                UserId = userId,

                Description = note.Description,
                Parent = note.Parent,
                Check = note.Check,
                Weight = note.Weight,
                Share = note.Share
            }, CancellationToken.None));

            Assert.Null(
                TreeNoteWorker.Finder
                .NoteFromId(_notes, note.Id)
                .FirstOrDefault(n => n.Title == title));
            Assert.NotNull(
                TreeNoteWorker.Finder
                .NoteFromId(_notes, note.Id)
                .FirstOrDefault(note =>

                note.User == oldNote.User &&
                note.Parent == oldNote.Parent &&
                note.Share == oldNote.Share &&
                note.Check == oldNote.Check &&
                note.Title == oldNote.Title &&
                note.Description == oldNote.Description &&
                note.Weight == oldNote.Weight));


            var parents = TreeNoteWorker.Finder.AllParentsFromId(_notes, note.Id);
            var childs = TreeNoteWorker.Finder.AllChildsFromId(_notes, note.Id);

            Assert
                .True(parents.Count() == 2);
            Assert
                .True(childs.Count() == 3);
            Assert
                .True(note.LastEdit == DateTime.Today);
        }

        [Fact]
        public async Task UpdateNoteCommandHandler_Fail_NoteId()
        {
            //Arrange
            var handler = new UpdateCommandHandler(_notes);

            var userId = ContentFactory.UserC_Id;
            var noteId = Guid.NewGuid();

            //Act
            //Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            await handler.Handle(new UpdateCommand
            {
                UserId = userId,
                NoteId = noteId
            }, CancellationToken.None));

            Assert.Null(TreeNoteWorker.Finder
                .NoteFromId(_notes, noteId)
                .FirstOrDefault());


            var newParents = TreeNoteWorker.Finder.AllParentsFromId(_notes, noteId);
            var newChilds = TreeNoteWorker.Finder.AllChildsFromId(_notes, noteId);

            Assert.True(newParents.Count() == 0);
            Assert.True(newChilds.Count() == 0);
        }

        [Fact]
        public async Task UpdateNoteCommandHandler_Fail_ParentChecked()
        {
            //Arrange
            var handler = new UpdateCommandHandler(_notes);

            var userId = ContentFactory.UserB_Id;
            var note =
                TreeNoteWorker.Finder
                .NoteFromId(_notes, ContentFactory.NoteId_Child_D)
                .FirstOrDefault();

            var title = "title";
            var description = "description";

            var appointedUserId = ContentFactory.UserC_Id;
            var parentId = ContentFactory.NoteId_Child_E;

            var isChecked = true;
            var isShared = true;

            var weight = 1000;

            //Act
            //Assert
            await Assert.ThrowsAsync<TreeNoteParentCheckException>(async () =>
            await handler.Handle(new UpdateCommand
            {
                UserId = userId,
                Description = description,
                Check = isChecked,
                NoteId = note.Id,
                Weight = weight,
                Title = title,
                User = appointedUserId,
                Share = isShared,
                Parent = parentId
            }, CancellationToken.None));

            Assert.Null(
                TreeNoteWorker.Finder
                .NoteFromId(_notes, note.Id)
                .FirstOrDefault(note =>

                note.User == appointedUserId &&
                note.Parent == parentId &&
                note.Share == isShared &&
                note.Check == isChecked &&
                note.Title == title &&
                note.Description == description &&
                note.Weight == weight));


            var newParents = TreeNoteWorker.Finder.AllParentsFromId(_notes, note.Id);
            var newChilds = TreeNoteWorker.Finder.AllChildsFromId(_notes, parentId);

            Assert.True(note.LastEdit == DateTime.Today);

            Assert
                .False(newParents.Count() == 4);
            Assert
                .False(newChilds.Select(note => note.Id).Contains(note.Id));
        }
    }
}