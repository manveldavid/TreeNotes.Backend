using Application.Actions.Note.Queries.Childs;
using Application.Actions.Note.Queries.FromDate;
using Tests.Common;

namespace Tests.Application.Actions.Note.Queries
{
    public class FromDateQueryHandlerTests : TestBase
    {
        [Fact]
        public async Task FromDateQueryHandler_Success()
        {
            //Arrange
            var handler = new FromDateQueryHandler(_notes);
            var userId = ContentFactory.UserC_Id;

            //Act
            var result = await handler.Handle(
                new FromDateQuery
                {
                    Date = DateTime.Now.AddMinutes(-5),
                    UserId = userId
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count() == 1);
        }

        [Fact]
        public async Task FromDateQueryHandler_Success_All()
        {
            //Arrange
            var handler = new FromDateQueryHandler(_notes);
            var userId = ContentFactory.UserC_Id;

            //Act
            var result = await handler.Handle(
                new FromDateQuery
                {
                    Date = DateTime.Today.AddDays(-1),
                    UserId = userId
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count() == 5);
        }

        [Fact]
        public async Task FromDateQueryHandler_Fail()
        {
            //Arrange
            var handler = new FromDateQueryHandler(_notes);
            var userId = ContentFactory.UserC_Id;

            //Act
            var result = await handler.Handle(
                new FromDateQuery
                {
                    Date = DateTime.Now,
                    UserId = userId
                }, CancellationToken.None);

            //Assert
            Assert.True(result.Count() == 0);
        }
    }
}
