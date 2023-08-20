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

            var passcode = TreeNoteUserWorker.Encoder.CodeFromLoginPassword(login, password);

            //Act
            await handler.Handle(new DeleteCommand
            {
                Passcode = passcode
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
            var passcode = TreeNoteUserWorker.Encoder.CodeFromLoginPassword(login, password);

            //Act
            //Assert
            var user = TreeNoteUserWorker.Finder.UserFromLogin(_users, login).FirstOrDefault();
            Assert.Null(user);
            await Assert.ThrowsAsync<EntityNotFoundException>(async () => 
                await handler.Handle(new DeleteCommand
                {
                    Passcode = passcode
                }, CancellationToken.None));
        }
    }
}
