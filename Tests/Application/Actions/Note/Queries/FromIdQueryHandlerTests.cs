using Application.Actions.Note.Queries.FromId;
using Application.Common.Exceptions;
using Application.Common.Workers;
using Domain;
using Tests.Common;

namespace Tests.Application.Actions.Note.Queries
{
    public class FromIdQueryHandlerTests : TestBase
    {
        [Fact]
        public async Task FromIdQueryHandler_Success_UserId()
        {
            //Arrange
            var handler = new FromIdQueryHandler(_notes);

            var userId = ContentFactory.UserC_Id;
            var noteId = ContentFactory.NoteId_Root;

            var title = "noteRoot";
            var description = "root";
            var number = 1;
            var weight = 100;
            var appointedUserId = Guid.Empty;
            var parentId = Guid.Empty;
            var creationDate = DateTime.Today;
            var lastEditDate = DateTime.Today;
            var isChecked = false;
            var isShared = false;

            //Act
            var result = await handler.Handle(
                new FromIdQuery
                {
                    UserId = userId,
                    NoteId = noteId
                }, CancellationToken.None);

            //Assert
            Assert.Equal(result,
                TreeNoteWorker.Finder
                .NoteFromId(_notes, noteId)
                .FirstOrDefault(
                note =>
                note.Parent == parentId &&
                note.User == appointedUserId &&
                note.Creation == creationDate &&
                note.LastEdit == lastEditDate &&
                note.Description == description &&
                note.Title == title &&
                note.Share == isShared &&
                note.Check == isChecked &&
                note.Weight == weight &&
                note.Number == number));
        }

        [Fact]
        public async Task FromIdQueryHandler_Fail_WrongNoteId()
        {
            //Arrange
            var handler = new FromIdQueryHandler(_notes);

            var userId = ContentFactory.UserC_Id;
            var noteId = Guid.NewGuid();

            //Act
            //Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(
                async () =>
                await handler.Handle(
                new FromIdQuery
                {
                    UserId = userId,
                    NoteId = noteId
                }, CancellationToken.None));
        }

        [Fact]
        public async Task FromIdQueryHandler_Fail_WrongUserId()
        {
            //Arrange
            var handler = new FromIdQueryHandler(_notes);

            var userId = Guid.NewGuid();
            var noteId = ContentFactory.NoteId_Root;

            //Act
            //Assert
            await Assert.ThrowsAsync<EntityDeniedPermissionException>(
                async () =>
                await handler.Handle(
                new FromIdQuery
                {
                    UserId = userId,
                    NoteId = noteId
                }, CancellationToken.None));
        }
    }
}
