using Application.Actions.Note.Queries.Parents;
using Application.Common.Exceptions;
using Tests.Common;

namespace Tests.Application.Actions.Note.Queries
{
    public class ParentsQueryHandlerTests : TestBase
    {
        [Fact]
        public async Task ParentsQueryHandler_Success_UserId()
        {
            //Arrange
            var handler = new ParentsQueryHandler(_notes);

            var userId = ContentFactory.UserC_Id;
            var noteId = ContentFactory.NoteId_Child_F;

            var parentId = ContentFactory.NoteId_Parent_C;
            var rootId = ContentFactory.NoteId_Root;

            //Act
            var result = await handler.Handle(
                new ParentsQuery
                {
                    UserId = userId,
                    NoteId = noteId
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count() == 2 &&
                result.Select(note => note.Id).Contains(parentId) &&
                result.Select(note => note.Id).Contains(rootId));
        }

        [Fact]
        public async Task ParentsQueryHandler_Success_AppointedUserId()
        {
            //Arrange
            var handler = new ParentsQueryHandler(_notes);

            var userId = ContentFactory.UserA_Id;
            var noteId = ContentFactory.NoteId_Parent_A;
            var rootId = ContentFactory.NoteId_Root;

            //Act
            var result = await handler.Handle(
                new ParentsQuery
                {
                    UserId = userId,
                    NoteId = noteId
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count() == 1 &&
                result.Select(note => note.Id).Contains(rootId));
            Assert.True(result.First().Id ==  rootId);
        }

        [Fact]
        public async Task ParentsQueryHandler_Success_IsShared()
        {
            //Arrange
            var handler = new ParentsQueryHandler(_notes);

            var userId = ContentFactory.UserA_Id;
            var noteId = ContentFactory.NoteId_Child_F;
            var rootId = ContentFactory.NoteId_Root;

            //Act
            var result = await handler.Handle(
                new ParentsQuery
            {
                UserId = userId,
                NoteId = noteId
            }, CancellationToken.None);

            //Assert
            Assert.True(result.Count() == 2 &&
                result.Select(note => note.Id).Contains(rootId));
            Assert.True(result.Last().Id == rootId);
        }

        [Fact]
        public async Task ParentsQueryHandler_Fail_UserId()
        {
            //Arrange
            var handler = new ParentsQueryHandler(_notes);

            var userId = ContentFactory.UserB_Id;
            var noteId = ContentFactory.NoteId_Child_A;

            //Act
            //Assert
            await Assert.ThrowsAsync<EntityDeniedPermissionException>(
                async () => await handler.Handle(
                    new ParentsQuery
                    {
                        UserId = userId,
                        NoteId = noteId
                    }, CancellationToken.None));
        }

        [Fact]
        public async Task ParentsQueryHandler_Fail_NoteId()
        {
            //Arrange
            var handler = new ParentsQueryHandler(_notes);

            var userId = ContentFactory.UserC_Id;
            var noteId = Guid.NewGuid();

            //Act
            //Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(
                async () =>
                await handler.Handle(
                new ParentsQuery
                {
                    UserId = userId,
                    NoteId = noteId
                }, CancellationToken.None));
        }
    }
}
