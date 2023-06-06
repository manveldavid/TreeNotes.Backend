using Application.Actions.Note.Queries.ParentCheck;
using Application.Common.Exceptions;
using Tests.Common;

namespace Tests.Application.Actions.Note.Queries
{
    public class ParentCheckQueryHandlerTests : TestBase
    {
        [Fact]
        public async Task ParentCheckQueryHandler_Success()
        {
            //Arrange
            var handler = new ParentCheckQueryHandler(_notes);

            var userId = ContentFactory.UserB_Id;
            var noteId = ContentFactory.NoteId_Child_D;

            //Act
            var result = await handler.Handle(
                new ParentCheckQuery
                {
                    UserId = userId,
                    NoteId = noteId
                }, CancellationToken.None);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ParentCheckQueryHandler_Fail_WrongNoteId()
        {
            //Arrange
            var handler = new ParentCheckQueryHandler(_notes);

            var userId = ContentFactory.UserB_Id;
            var noteId = Guid.NewGuid();

            //Act
            //Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(
                async () =>
                await handler.Handle(
                new ParentCheckQuery
                {
                    UserId = userId,
                    NoteId = noteId
                }, CancellationToken.None));
        }

        [Fact]
        public async Task ParentCheckQueryHandler_Fail_WrongUserId()
        {
            //Arrange
            var handler = new ParentCheckQueryHandler(_notes);

            var userId = ContentFactory.UserC_Id;
            var noteId = ContentFactory.NoteId_Child_D;

            //Act
            //Assert
            await Assert.ThrowsAsync<EntityDeniedPermissionException>(
                async () =>
                await handler.Handle(
                new ParentCheckQuery
                {
                    UserId = userId,
                    NoteId = noteId
                }, CancellationToken.None));
        }
    }
}
