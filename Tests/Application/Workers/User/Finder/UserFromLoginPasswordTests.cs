using Application.Common.Workers;
using Tests.Common;

namespace Tests.Application.Workers.User.Finder
{
    public class UserFromLoginPasswordTests : TestBase
    {
        [Fact]
        public async Task UserFromLoginPassword_Success()
        {
            //Arrange
            var login = "userA";
            var password = "password";

            //Act
            var user = TreeNoteUserWorker.Finder.UserFromLoginPassword(_users, login, password)
                .FirstOrDefault(user =>
                user.Id == ContentFactory.UserA_Id &&
                user.Code == TreeNoteUserWorker.Encoder.CodeFromLoginPassword(login, password) &&
                user.Login == login);

            //Assert
            Assert.NotNull(user);
        }

        [Fact]
        public async Task UserFromLoginPassword_Fail()
        {
            //Arrange
            var login = "userRandom";
            var password = "password";

            //Act
            var user = TreeNoteUserWorker.Finder.UserFromLoginPassword(_users, login, password)
                .FirstOrDefault(user =>
                user.Code == TreeNoteUserWorker.Encoder.CodeFromLoginPassword(login, password) &&
                user.Login == login);

            //Assert
            Assert.Null(user);
        }
    }
}
