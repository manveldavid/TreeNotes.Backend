using Application.Actions.User.Queries.UserId;
using Application.Common.Exceptions;
using Tests.Common;

namespace Tests.Application.Actions.User.Queries
{
    public class UserIdQueryHandlerTests : TestBase
    {
        [Fact]
        public async Task UserIdQueryHandler_Success()
        {
            //Arrange
            var handler = new UserIdQueryHandler(_users);

            var login = "userA";
            var password = "password";

            //Act
            var userId = await handler.Handle(
                new UserIdQuery
                {
                    Login = login,
                    Password = password
                }, CancellationToken.None);

            //Assert
            Assert.True(userId == ContentFactory.UserA_Id);
        }

        [Fact]
        public async Task UserIdQueryHandler_Fail_WrongLogin()
        {
            //Arrange
            var handler = new UserIdQueryHandler(_users);

            var login = "userRandom";
            var password = "password";

            //Act
            //Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(
                async () =>
                await handler.Handle(
                new UserIdQuery
                {
                    Login = login,
                    Password = password
                }, CancellationToken.None));
        }

        [Fact]
        public async Task UserIdQueryHandler_Fail_WrongPassword()
        {
            //Arrange
            var handler = new UserIdQueryHandler(_users);

            var login = "userA";
            var password = "wrongPassword";

            //Act
            //Assert
            await Assert.ThrowsAsync<TreeNoteUserWrongPasswordException>(
                async () =>
                await handler.Handle(
                new UserIdQuery
                {
                    Login = login,
                    Password = password
                }, CancellationToken.None));
        }
    }
}
