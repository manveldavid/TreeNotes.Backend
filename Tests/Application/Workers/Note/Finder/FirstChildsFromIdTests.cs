using Application.Common.Workers;
using Tests.Common;

namespace Tests.Application.Workers.Note.Finder
{
    public class FirstChildsFromIdTests : TestBase
    {
        [Fact]
        public async Task FirstChildsFromId_Success_Root()
        {
            //Arrange
            var noteId = ContentFactory.NoteId_Root;

            //Act
            var childs = TreeNoteWorker.Finder.FirstChildsFromId(_notes, noteId);

            //Assert
            Assert.True(childs.Count() == 3);
        }

        [Fact]
        public async Task FirstChildsFromId_Success_ParentA()
        {
            //Arrange
            var noteId = ContentFactory.NoteId_Parent_A;

            //Act
            var childs = TreeNoteWorker.Finder.FirstChildsFromId(_notes, noteId);

            //Assert
            Assert.True(childs.Count() == 3);
        }

        [Fact]
        public async Task FirstChildsFromId_Success_ChildA()
        {
            //Arrange
            var noteId = ContentFactory.NoteId_Child_A;

            //Act
            var childs = TreeNoteWorker.Finder.FirstChildsFromId(_notes, noteId);

            //Assert
            Assert.True(childs.Count() == 0);
        }
    }
}
