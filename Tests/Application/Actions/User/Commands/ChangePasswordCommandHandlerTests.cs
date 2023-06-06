using Application.Actions.User.Commands.ChangePassword;
using Application.Common.Exceptions;
using Application.Common.Workers;
using Tests.Common;

namespace Tests.Application.Actions.User.Commands
{
    public class ChangePasswordCommandHandlerTests : TestBase
    {
        [Fact]
        public async Task ChangePasswordCommandHandler_Success()
        {
            //Arrange
            var handler = new ChangePasswordCommandHandler(_users);

            var login = "userA";
            var password = "password";
            var newPassword = "newPassword";

            //Act
            await handler.Handle(
                new ChangePasswordCommand
                {
                    Login = login,
                    NewPassword = newPassword,
                    OldPassword = password
                },
                CancellationToken.None);

            //Assert
            Assert.NotNull(TreeNoteUserWorker.Finder
                .UserFromLoginPassword(_users, login, newPassword)
                .SingleOrDefault(user =>
                user.Id == ContentFactory.UserA_Id &&
                user.Login == login &&
                user.Code == TreeNoteUserWorker.Encoder
                .CodeFromLoginPassword(login, newPassword)));
        }

        [Fact]
        public async Task ChangePasswordCommandHandler_Fail_WrongPassword()
        {
            //Arrange
            var handler = new ChangePasswordCommandHandler(_users);

            var login = "userA";
            var newPassword = "newPassword";
            var password = "wrongPassword";

            //Act
            //Assert
            await Assert.ThrowsAsync<TreeNoteUserWrongPasswordException>(
                async () =>
                await handler.Handle(
                new ChangePasswordCommand
                {
                    Login = login,
                    NewPassword = newPassword,
                    OldPassword = password
                },
                CancellationToken.None));
        }

        [Fact]
        public async Task ChangePasswordCommandHandler_Fail_WrongLogin()
        {
            //Arrange
            var handler = new ChangePasswordCommandHandler(_users);

            var login = "userRandom";
            var newPassword = "newPassword";
            var password = "wrongPassword";

            //Act
            //Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(
                async () =>
                await handler.Handle(
                new ChangePasswordCommand
                {
                    Login = login,
                    NewPassword = newPassword,
                    OldPassword = password
                },
                CancellationToken.None));
        }
    }
}
