using Application.Common.Workers;
using Tests.Common;

namespace Tests.Application.Workers.Note.Finder
{
    public class AllParentsFromParentIdTests : TestBase
    {
        [Fact]
        public async Task AllParentsFromParentId_Success_ChildA()
        {
            //Arrange
            var noteId = ContentFactory.NoteId_Child_A;

            //Act
            var parents = TreeNoteWorker.Finder.AllParentsFromId(_notes, noteId);

            //Assert
            Assert.True(parents.Count() == 3);
        }

        [Fact]
        public async Task AllParentsFromParentId_Success_ParentA()
        {
            //Arrange
            var noteId = ContentFactory.NoteId_Parent_A;

            //Act
            var parents = TreeNoteWorker.Finder.AllParentsFromId(_notes, noteId);

            //Assert
            Assert.True(parents.Count() == 2);
        }

        [Fact]
        public async Task AllParentsFromParentId_Success_Root()
        {
            //Arrange
            var noteId = ContentFactory.NoteId_Root;

            //Act
            var parents = TreeNoteWorker.Finder.AllParentsFromId(_notes, noteId);

            //Assert
            Assert.True(parents.Count() == 1);
        }
    }
}
