using Application.Common.Workers;
using Tests.Common;

namespace Tests.Application.Workers.Note.Finder
{
    public class RootNotesFromUserIdTests : TestBase
    {
        [Fact]
        public async Task RootNotesFromUserId_Success()
        {
            //Arrange
            var userId = ContentFactory.UserA_Id;

            //Act
            var notes = TreeNoteWorker.Finder.RootNotesFromUserId(_notes, userId);
            //Assert
            Assert.True(notes.Count() == 1);
        }

        [Fact]
        public async Task RootNotesFromUserId_Fail_WrongUserId()
        {
            //Arrange
            var userId = Guid.NewGuid();

            //Act
            var notes = TreeNoteWorker.Finder.RootNotesFromUserId(_notes, userId);

            //Assert
            Assert.True(notes.Count() == 0);
        }
    }
}
