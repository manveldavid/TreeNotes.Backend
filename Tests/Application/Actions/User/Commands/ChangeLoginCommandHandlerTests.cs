using Application.Actions.User.Commands.ChangeLogin;
using Application.Common.Exceptions;
using Application.Common.Workers;
using Tests.Common;

namespace Tests.Application.Actions.User.Commands
{
    public class ChangeLoginCommandHandlerTests : TestBase
    {
        [Fact]
        public async Task ChangeLoginCommandHandler_Success()
        {
            //Arrange
            var handler = new ChangeLoginCommandHandler(_users);

            var login = "userA";
            var password = "password";
            var newLogin = "login";

            //Act
            await handler.Handle(
                new ChangeLoginCommand
                {
                    OldLogin = login,
                    NewLogin = newLogin,
                    Password = password
                },
                CancellationToken.None);

            //Assert
            Assert.NotNull(TreeNoteUserWorker.Finder
                .UserFromLoginPassword(_users, newLogin, password)
                .SingleOrDefault(user =>
                user.Id == ContentFactory.UserA_Id &&
                user.Login == newLogin &&
                user.Code == TreeNoteUserWorker.Encoder
                .CodeFromLoginPassword(newLogin, password)));
        }

        [Fact]
        public async Task ChangeLoginCommandHandler_Fail_WrongPassword()
        {
            //Arrange
            var handler = new ChangeLoginCommandHandler(_users);

            var login = "userA";
            var password = "wrongPassword";
            var newLogin = "login";

            //Act
            //Assert
            await Assert.ThrowsAsync<TreeNoteUserWrongPasswordException>(
                async () =>
                await handler.Handle(
                new ChangeLoginCommand
                {
                    OldLogin = login,
                    NewLogin = newLogin,
                    Password = password
                },
                CancellationToken.None));
        }

        [Fact]
        public async Task ChangeLoginCommandHandler_Fail_WrongLogin()
        {
            //Arrange
            var handler = new ChangeLoginCommandHandler(_users);

            var login = "userRandom";
            var password = "password";
            var newLogin = "login";

            //Act
            //Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(
                async () =>
                await handler.Handle(
                new ChangeLoginCommand
                {
                    OldLogin = login,
                    NewLogin = newLogin,
                    Password = password
                },
                CancellationToken.None));
        }
    }
}
