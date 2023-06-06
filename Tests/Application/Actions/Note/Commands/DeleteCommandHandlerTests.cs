using Application.Actions.Note.Commands.Delete;
using Application.Common.Exceptions;
using Application.Common.Workers;
using Tests.Common;

namespace Tests.Application.Actions.Note.Commands
{
    public class DeleteCommandHandlerTests : TestBase
    {
        [Fact]
        public async Task DeleteCommandHandler_Success_Root()
        {
            //Arrange
            var handler = new DeleteCommandHandler(_notes);

            var userId = ContentFactory.UserC_Id;
            var rootId = ContentFactory.NoteId_Root;

            //Act
            await handler.Handle(new DeleteCommand
            {
                UserId = userId,
                NoteId = rootId
            }, CancellationToken.None);

            //Assert
            var childs = TreeNoteWorker.Finder.AllChildsFromId(_notes, rootId);
            var rootNote = TreeNoteWorker.Finder.NoteFromId(_notes, rootId).FirstOrDefault();
            var userNotes = TreeNoteWorker.Finder.RootNotesFromUserId(_notes, userId);
            Assert.Null(rootNote);
            Assert.True(childs.Count() == 0);
            Assert.True(userNotes.Count() == 0);
        }

        [Fact]
        public async Task DeleteCommandHandler_Success_Parent()
        {
            //Arrange
            var handler = new DeleteCommandHandler(_notes);

            var userId = ContentFactory.UserC_Id;
            var rootId = ContentFactory.NoteId_Root;
            var parentId = ContentFactory.NoteId_Parent_A;
            var childId = ContentFactory.NoteId_Child_A;
            var noChildId = ContentFactory.NoteId_Child_F;
            var noParentId = ContentFactory.NoteId_Parent_B;

            //Act
            await handler.Handle(new DeleteCommand
            {
                UserId = userId,
                NoteId = parentId
            }, CancellationToken.None);

            //Assert
            var childs = TreeNoteWorker.Finder.AllChildsFromId(_notes, parentId);
            var parentNote = TreeNoteWorker.Finder.NoteFromId(_notes, parentId).FirstOrDefault();
            var childNote = TreeNoteWorker.Finder.NoteFromId(_notes, childId).FirstOrDefault();
            var rootNote = TreeNoteWorker.Finder.NoteFromId(_notes, rootId).FirstOrDefault();
            var noChildNote = TreeNoteWorker.Finder.NoteFromId(_notes, noChildId).FirstOrDefault();
            var noParentNote = TreeNoteWorker.Finder.NoteFromId(_notes, noParentId).FirstOrDefault();
            Assert.Null(parentNote);
            Assert.Null(childNote);
            Assert.NotNull(rootNote);
            Assert.NotNull(noChildNote);
            Assert.NotNull(noParentNote);
            Assert.True(childs.Count() == 0);
        }

        [Fact]
        public async Task DeleteCommandHandler_Fail_NoteId()
        {
            //Arrange
            var handler = new DeleteCommandHandler(_notes);

            var userId = ContentFactory.UserC_Id;
            var noteId = Guid.NewGuid();

            //Act
            //Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            await handler.Handle(new DeleteCommand
            {
                UserId = userId,
                NoteId = noteId
            }, CancellationToken.None));
        }

        [Fact]
        public async Task DeleteCommandHandler_Fail_AppointedUserId()
        {
            //Arrange
            var handler = new DeleteCommandHandler(_notes);

            var userId = ContentFactory.UserA_Id;
            var noteId = ContentFactory.NoteId_Parent_A;

            //Act
            //Assert
            await Assert.ThrowsAsync<EntityDeniedPermissionException>(async () =>
            await handler.Handle(new DeleteCommand
            {
                UserId = userId,
                NoteId = noteId
            }, CancellationToken.None));

            Assert.NotNull(TreeNoteWorker.Finder.NoteFromId(_notes, noteId).FirstOrDefault());
        }

        [Fact]
        public async Task DeleteCommandHandler_Fail_UserId()
        {
            //Arrange
            var handler = new DeleteCommandHandler(_notes);

            var userId = ContentFactory.UserA_Id;
            var noteId = ContentFactory.NoteId_Parent_C;

            //Act
            //Assert
            await Assert.ThrowsAsync<EntityDeniedPermissionException>(async () =>
            await handler.Handle(new DeleteCommand
            {
                UserId = userId,
                NoteId = noteId
            }, CancellationToken.None));

            Assert.NotNull(TreeNoteWorker.Finder.NoteFromId(_notes, noteId).FirstOrDefault());
        }

        [Fact]
        public async Task DeleteCommandHandler_Fail_ParentChecked()
        {
            //Arrange
            var handler = new DeleteCommandHandler(_notes);

            var userId = ContentFactory.UserB_Id;
            var noteId = ContentFactory.NoteId_Child_D;

            //Act
            //Assert
            await Assert.ThrowsAsync<TreeNoteParentCheckException>(async () =>
            await handler.Handle(new DeleteCommand
            {
                UserId = userId,
                NoteId = noteId
            }, CancellationToken.None));
            Assert.NotNull(TreeNoteWorker.Finder.NoteFromId(_notes, noteId).FirstOrDefault());
        }
    }
}
