using Application.Actions.Note.Queries.Childs;
using Tests.Common;

namespace Tests.Application.Actions.Note.Queries
{
    public class ChildsQueryHandlerTests : TestBase
    {
        [Fact]
        public async Task ChildsQueryHandler_Success_UserId()
        {
            //Arrange
            var handler = new ChildsQueryHandler(_notes);
            var userId = ContentFactory.UserC_Id;
            var noteId = ContentFactory.NoteId_Root;
            var childA = ContentFactory.NoteId_Parent_A;
            var childB = ContentFactory.NoteId_Parent_B;
            var childC = ContentFactory.NoteId_Parent_C;

            //Act
            var result = await handler.Handle(
                new ChildsQuery
                {
                    NoteId = noteId,
                    UserId = userId
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count() == 3 &&
                result.Select(note => note.Id).Contains(childA) &&
                result.Select(note => note.Id).Contains(childB) &&
                result.Select(note => note.Id).Contains(childC));
        }

        [Fact]
        public async Task ChildsQueryHandler_Success_AppointedUserIdAndIsShared()
        {
            //Arrange
            var handler = new ChildsQueryHandler(_notes);
            var userId = ContentFactory.UserA_Id;
            var noteId = ContentFactory.NoteId_Root;
            var childA = ContentFactory.NoteId_Parent_A;
            var childC = ContentFactory.NoteId_Parent_C;
            //Act
            var result = await handler.Handle(
                new ChildsQuery
                {
                    NoteId = noteId,
                    UserId = userId
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count() == 2 &&
                result.Select(note => note.Id).Contains(childA) &&
                result.Select(note => note.Id).Contains(childC));
        }

        [Fact]
        public async Task ChildsQueryHandler_Success_NoChild()
        {
            //Arrange
            var handler = new ChildsQueryHandler(_notes);
            var userId = ContentFactory.UserA_Id;
            var noteId = ContentFactory.NoteId_Child_A;
            //Act
            var result = await handler.Handle(
                new ChildsQuery
                {
                    NoteId = noteId,
                    UserId = userId
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count() == 0);
        }

        [Fact]
        public async Task ChildsQueryHandler_Fail_UserId()
        {
            //Arrange
            var handler = new ChildsQueryHandler(_notes);
            var userId = ContentFactory.UserB_Id;
            var noteId = ContentFactory.NoteId_Parent_A;
            //Act
            var result = await handler.Handle(
                new ChildsQuery
                {
                    NoteId = noteId,
                    UserId = userId
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count() == 0);
        }

        [Fact]
        public async Task ChildsQueryHandler_Fail_NoteId()
        {
            //Arrange
            var handler = new ChildsQueryHandler(_notes);
            var userId = ContentFactory.UserC_Id;
            var noteId = Guid.NewGuid();
            //Act
            var result = await handler.Handle(
                new ChildsQuery
                {
                    NoteId = noteId,
                    UserId = userId
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count() == 0);
        }
    }
}
