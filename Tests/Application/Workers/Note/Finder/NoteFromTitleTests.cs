using Application.Common.Workers;
using Tests.Common;

namespace Tests.Application.Workers.Note.Finder
{
    public class NoteFromTitleTests : TestBase
    {
        [Fact]
        public async Task NoteFromTitle_Success()
        {
            //Arrange
            var description = "root";
            var noteId = ContentFactory.NoteId_Root;
            var appointedUserId = Guid.Empty;
            var isChecked = false;
            var isShared = false;
            var parentNoteId = Guid.Empty;
            var fragment = "noteRoot";
            var userId = ContentFactory.UserC_Id;
            var weight = 100;

            //Act
            var note = TreeNoteWorker.Finder
                .NotesFromTitle(_notes, userId, fragment)
                .FirstOrDefault(note =>

                note.Title.Contains(fragment) &&
                note.Description == description &&

                note.Check == isChecked &&
                note.Share == isShared &&

                note.Id == noteId &&
                note.Parent == parentNoteId &&
                note.Creator == userId &&
                note.User == appointedUserId &&

                note.Weight == weight);

            //Assert
            Assert.NotNull(note);
        }

        [Fact]
        public async Task NoteFromTitle_Fail_Title()
        {
            //Arrange
            var fragment = "someTitle";
            var userId = ContentFactory.UserC_Id;

            //Act
            var note = TreeNoteWorker.Finder
                .NotesFromTitle(_notes, userId, fragment)
                .FirstOrDefault();

            //Assert
            Assert.Null(note);
        }
    }
}
