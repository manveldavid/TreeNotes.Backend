using Application.Actions.Note.Queries.FromTitle;
using Tests.Common;

namespace Tests.Application.Actions.Note.Queries
{
    public class FromTitleQueryHandlerTests : TestBase
    {
        [Fact]
        public async Task FromTitleQueryHandler_Success_Child()
        {
            //Arrange
            var handler = new FromTitleQueryHandler(_notes);

            var userId = ContentFactory.UserA_Id;
            var titleFragment = "child";

            //Act
            var result = await handler.Handle(
                new FromTitleQuery
                {
                    UserId = userId,
                    Fragment = titleFragment
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count() == 3);
        }

        [Fact]
        public async Task FromTitleQueryHandler_Success_Fragment()
        {
            //Arrange
            var handler = new FromTitleQueryHandler(_notes);

            var userId = ContentFactory.UserA_Id;
            var titleFragment = "note";
            //Act
            var result = await handler.Handle(
                new FromTitleQuery
                {
                    UserId = userId,
                    Fragment = titleFragment
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count() == 4);
        }

        [Fact]
        public async Task FromTitleQueryHandler_Success_NoOne_Fragment()
        {
            //Arrange
            var handler = new FromTitleQueryHandler(_notes);

            var userId = ContentFactory.UserA_Id;
            var titleFragment = "randomFragment";
            //Act
            var result = await handler.Handle(
                new FromTitleQuery
                {
                    UserId = userId,
                    Fragment = titleFragment
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count() == 0);
        }

        [Fact]
        public async Task FromTitleQueryHandler_Success_NoOne_UserId()
        {
            //Arrange
            var handler = new FromTitleQueryHandler(_notes);

            var userId = Guid.NewGuid();
            var titleFragment = "note";
            //Act
            var result = await handler.Handle(
                new FromTitleQuery
                {
                    UserId = userId,
                    Fragment = titleFragment
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count() == 0);
        }
    }
}
