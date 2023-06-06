using Application.Common.Workers;
using Tests.Common;

namespace Tests.Application.Workers.User.Finder
{
    public class UserFromLoginTests : TestBase
    {
        [Fact]
        public async Task UserFromLogin_Success()
        {
            //Arrange
            var login = "userA";
            var password = "password";

            //Act
            var user = TreeNoteUserWorker.Finder.UserFromLogin(_users, login)
                .FirstOrDefault(user =>
                user.Id == ContentFactory.UserA_Id &&
                user.Code == TreeNoteUserWorker.Encoder.CodeFromLoginPassword(login, password) &&
                user.Login == login);

            //Assert
            Assert.NotNull(user);
        }

        [Fact]
        public async Task UserFromLogin_Fail()
        {
            //Arrange
            var login = "userRandom";

            //Act
            var user = TreeNoteUserWorker.Finder.UserFromLogin(_users, login)
                .FirstOrDefault(user => user.Login == login);

            //Assert
            Assert.Null(user);
        }
    }
}
