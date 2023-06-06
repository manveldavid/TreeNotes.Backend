using Application.Actions.Note.Queries.FromDescription;
using Tests.Common;

namespace Tests.Application.Actions.Note.Queries
{
    public class FromDescriptionQueryHandlerTests : TestBase
    {
        [Fact]
        public async Task FromDescriptionQueryHandler_Success_Child()
        {
            //Arrange
            var handler = new FromDescriptionQueryHandler(_notes);

            var userId = ContentFactory.UserA_Id;
            var descriptionFragment = "child";

            //Act
            var result = await handler.Handle(
                new FromDescriptionQuery
                {
                    UserId = userId,
                    Fragment = descriptionFragment
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count() == 3);
        }

        [Fact]
        public async Task FromDescriptionQueryHandler_Success_Root()
        {
            //Arrange
            var handler = new FromDescriptionQueryHandler(_notes);

            var userId = ContentFactory.UserC_Id;
            var descriptionFragment = "root";

            //Act
            var result = await handler.Handle(
                new FromDescriptionQuery
                {
                    UserId = userId,
                    Fragment = descriptionFragment
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count() == 1);
        }

        [Fact]
        public async Task FromDescriptionQueryHandler_Success_NoOne_Fragment()
        {
            //Arrange
            var handler = new FromDescriptionQueryHandler(_notes);

            var userId = ContentFactory.UserA_Id;
            var descriptionFragment = "randomFragment";

            //Act
            var result = await handler.Handle(
                new FromDescriptionQuery
                {
                    UserId = userId,
                    Fragment = descriptionFragment
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count() == 0);
        }

        [Fact]
        public async Task FromDescriptionQueryHandler_Success_NoOne_UserId()
        {
            //Arrange
            var handler = new FromDescriptionQueryHandler(_notes);

            var userId = Guid.NewGuid();
            var descriptionFragment = "child";

            //Act
            var result = await handler.Handle(
                new FromDescriptionQuery
                {
                    UserId = userId,
                    Fragment = descriptionFragment
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count() == 0);
        }
    }
}
