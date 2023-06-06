using Application.Actions.User.Commands.Delete;
using Application.Common.Exceptions;
using Application.Common.Workers;
using Tests.Common;

namespace Tests.Application.Actions.User.Commands
{
    public class DeleteCommandHandlerTests : TestBase
    {
        [Fact]
        public async Task DeleteCommandHandler_Success()
        {
            //Arrange
            var handler = new DeleteCommandHandler(_users);

            var login = "userA";
            var password = "password";

            //Act
            await handler.Handle(new DeleteCommand
            {
                Login = login,
                Password = password
            }, CancellationToken.None);

            //Assert
            var user = TreeNoteUserWorker.Finder.UserFromLogin(_users, login).FirstOrDefault();
            Assert.Null(user);
        }

        [Fact]
        public async Task DeleteCommandHandler_Success_Fail_WrongLogin()
        {
            //Arrange
            var handler = new DeleteCommandHandler(_users);

            var login = "userRandom";
            var password = "password";

            //Act
            //Assert
            var user = TreeNoteUserWorker.Finder.UserFromLogin(_users, login).FirstOrDefault();
            Assert.Null(user);
            await Assert.ThrowsAsync<EntityNotFoundException>(async () => 
                await handler.Handle(new DeleteCommand
                {
                    Login = login,
                    Password = password
                }, CancellationToken.None));
        }

        [Fact]
        public async Task DeleteCommandHandler_Fail_WrongPassword()
        {
            //Arrange
            var handler = new DeleteCommandHandler(_users);

            var login = "userA";
            var password = "wrongPassword";

            //Act
            //Assert
            var user = TreeNoteUserWorker.Finder.UserFromLogin(_users, login).FirstOrDefault();
            Assert.NotNull(user);
            await Assert.ThrowsAsync<TreeNoteUserWrongPasswordException>(async () =>
                await handler.Handle(new DeleteCommand
                {
                    Login = login,
                    Password = password
                }, CancellationToken.None));
        }
    }
}
