using Application.Actions.Note.Queries.RootNotes;
using Tests.Common;

namespace Tests.Application.Actions.Note.Queries
{
    public class RootNotesQueryHandlerTests : TestBase
    {
        [Fact]
        public async Task RootNotesQueryHandler_Success_UserId()
        {
            //Arrange
            var handle = new RootNotesQueryHandler(_notes);

            var userId = ContentFactory.UserC_Id;
            var noteId = ContentFactory.NoteId_Root;
            var title = "noteRoot";
            //Act
            var result = await handle.Handle(
                new RootNotesQuery
                {
                    UserId = userId
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count == 1 &&
                result.FirstOrDefault()?.Title == title &&
                result.FirstOrDefault()?.Id == noteId);
        }

        [Fact]
        public async Task RootNotesQueryHandler_Success_AppointedUserId()
        {
            //Arrange
            var handle = new RootNotesQueryHandler(_notes);

            var userId = ContentFactory.UserA_Id;
            var noteId = ContentFactory.NoteId_Parent_A;
            var title = "noteFirstParent";
            //Act
            var result = await handle.Handle(
                new RootNotesQuery
                {
                    UserId = userId
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count == 1 &&
                result.FirstOrDefault()?.Title == title &&
                result.FirstOrDefault()?.Id == noteId);
        }

        [Fact]
        public async Task RootNotesQueryHandler_Fail_UserId()
        {
            //Arrange
            var handle = new RootNotesQueryHandler(_notes);
            var userId = Guid.NewGuid();
            //Act
            var result = await handle.Handle(
                new RootNotesQuery
                {
                    UserId = userId
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count == 0);
        }
    }
}
