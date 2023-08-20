using Application.Common.Workers;
using Tests.Common;

namespace Tests.Application.Workers.User.Finder
{
    public class UserFromPassCodeTests : TestBase
    {
        [Fact]
        public async Task UserFromPassCode_Success()
        {
            //Arrange
            var login = "userA";
            var password = "password";

            var passcode = TreeNoteUserWorker.Encoder.CodeFromLoginPassword(login, password);

            //Act
            var user = TreeNoteUserWorker.Finder.UserFromPasscode(_users, passcode)
                .FirstOrDefault(user =>
                user.Id == ContentFactory.UserA_Id &&
                user.Code == passcode &&
                user.Login == login);

            //Assert
            Assert.NotNull(user);
        }

        [Fact]
        public async Task UserFromPassCode_Fail()
        {
            //Arrange
            var passcode = "passcode";

            //Act
            var user = TreeNoteUserWorker.Finder.UserFromPasscode(_users, passcode)
                .FirstOrDefault();

            //Assert
            Assert.Null(user);
        }
    }
}
