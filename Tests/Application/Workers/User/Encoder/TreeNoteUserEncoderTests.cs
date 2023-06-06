using Application.Common.Workers;
using Domain;
using Tests.Common;

namespace Tests.Application.Workers.UserWorker.Encoder
{
    public class TreeNoteUserEncoderTests:TestBase
    {
        [Fact]
        public async Task TreeNoteUserEncoder()
        {
            //Assert
            var login = "Login";
            var password = "Password";

            var wrongCode =   "608010767118451207514090111011401000";
            var successCode = "608010767118451207513090111011401000";
            //Act
            var result = TreeNoteUserWorker.Encoder.CodeFromLoginPassword(login, password);

            //Arrange
            Assert.True(result == successCode);
            Assert.True(result != wrongCode);
        }
    }
}
