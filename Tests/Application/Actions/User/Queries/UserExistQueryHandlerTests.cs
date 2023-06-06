using Application.Actions.User.Queries.UserExist;
using Application.Common.Workers;
using Tests.Common;

namespace Tests.Application.Actions.User.Queries
{
    public class UserExistQueryHandlerTests : TestBase
    {
        [Fact]
        public async Task CreateCommandHandler_Success()
        {
            //Arrange
            var handler = new UserExistQueryHandler(_users);

            var login = "userA";

            //Act
            var result = await handler.Handle(
                new UserExistQuery
                {
                    Login = login
                },
                CancellationToken.None);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CreateCommandHandler_Fail()
        {
            //Arrange
            var handler = new UserExistQueryHandler(_users);

            var login = "login";

            //Act
            var result = await handler.Handle(
                new UserExistQuery
                {
                    Login = login
                },
                CancellationToken.None);

            //Assert
            Assert.False(result);
        }
    }
}
