using Application.Actions.User.Commands.Create;
using Application.Common.Workers;
using Tests.Common;

namespace Tests.Application.Actions.User.Commands
{
    public class CreateCommandHandlerTests : TestBase
    {
        [Fact]
        public async Task CreateCommandHandler_Success()
        {
            //Arrange
            var handler = new CreateCommandHandler(_users);

            var login = "login";
            var password = "password";

            //Act
            var userId = await handler.Handle(
                new CreateCommand
                {
                    Login = login,
                    Password = password
                },
                CancellationToken.None);

            //Assert
            Assert.NotNull(TreeNoteUserWorker.Finder
                .UserFromLoginPassword(_users, login, password)
                .SingleOrDefault(user =>
                user.Id == userId &&
                user.Login == login &&
                user.Code == TreeNoteUserWorker.Encoder
                .CodeFromLoginPassword(login, password)));
        }
    }
}
