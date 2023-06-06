using Application.Actions.Note.Commands.Create;
using Application.Common.Exceptions;
using Application.Common.Workers;
using Tests.Common;

namespace Tests.Application.Actions.Note.Commands
{
    public class CreateCommandHandlerTests : TestBase
    {
        [Fact]
        public async Task CreateCommandHandler_Success()
        {
            //Arrange
            var handler = new CreateCommandHandler(_notes);

            var userId = ContentFactory.UserA_Id;

            //Act
            var noteId = await handler.Handle(
                new CreateCommand
                {
                    UserId = userId
                },
                CancellationToken.None);

            //Assert
            Assert.NotNull(TreeNoteWorker.Finder
                .NoteFromId(_notes, noteId)
                .SingleOrDefault(note =>
                note.Id == noteId &&
                note.Parent == Guid.Empty &&
                note.Creator == userId &&
                note.Creation == DateTime.Today &&
                note.LastEdit == DateTime.Today));
        }

        [Fact]
        public async Task CreateCommandHandler_Fail_WrongParent()
        {
            //Arrange
            var handler = new CreateCommandHandler(_notes);

            var userId = ContentFactory.UserA_Id;
            var parentId = Guid.NewGuid();

            //Act
            //Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(
                async () => 
                await handler.Handle(
                new CreateCommand
                {
                    UserId = userId,
                    Parent = parentId
                },
                CancellationToken.None));
        }

        [Fact]
        public async Task CreateCommandHandler_Fail_WrongUser()
        {
            //Arrange
            var handler = new CreateCommandHandler(_notes);

            var userId = ContentFactory.UserA_Id;
            var parentId = ContentFactory.NoteId_Root;

            //Act
            //Assert
            await Assert.ThrowsAsync<EntityDeniedPermissionException>(
                async () =>
                await handler.Handle(
                new CreateCommand
                {
                    UserId = userId,
                    Parent = parentId
                },
                CancellationToken.None));
        }

        [Fact]
        public async Task CreateCommandHandler_Fail_NoteBlock()
        {
            //Arrange
            var handler = new CreateCommandHandler(_notes);

            var userId = ContentFactory.UserB_Id;
            var parentId = ContentFactory.NoteId_Parent_B;

            //Act
            //Assert
            await Assert.ThrowsAsync<TreeNoteParentCheckException>(
                async () =>
                await handler.Handle(
                new CreateCommand
                {
                    UserId = userId,
                    Parent = parentId
                },
                CancellationToken.None));
        }
    }
}
