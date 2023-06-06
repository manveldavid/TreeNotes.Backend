using Application.Common.Workers;
using Tests.Common;

namespace Tests.Application.Workers.Note.Finder
{
    public class NotesFromDescriptionTests : TestBase
    {
        [Fact]
        public async Task NotesFromDescription_Success()
        {
            //Arrange
            var createDate = DateTime.Today;
            var fragment = "root";
            var lastEditDate = DateTime.Today;
            var noteId = ContentFactory.NoteId_Root;
            var appointedUserId = Guid.Empty;
            var isChecked = false;
            var isShared = false;
            var parentNoteId = Guid.Empty;
            var title = "noteRoot";
            var userId = ContentFactory.UserC_Id;
            var weight = 100;


            //Act
            var note = TreeNoteWorker.Finder
                .NotesFromDescription(_notes, userId, fragment)
                .FirstOrDefault(note =>

                note.Title == title &&
                note.Description.Contains(fragment) &&

                note.Check == isChecked &&
                note.Share == isShared &&

                note.Id == noteId &&
                note.Parent == parentNoteId &&
                note.Creator == userId &&
                note.User == appointedUserId &&

                note.Weight == weight &&

                note.LastEdit == lastEditDate &&
                note.Creation == createDate);

            //Assert
            Assert.NotNull(note);
        }

        [Fact]
        public async Task NotesFromDescription_Fail_Title()
        {
            //Arrange
            var fragment = "someDescription";
            var userId = ContentFactory.UserC_Id;

            //Act
            var note = TreeNoteWorker.Finder
                .NotesFromDescription(_notes, userId, fragment)
                .FirstOrDefault();

            //Assert
            Assert.Null(note);
        }
    }
}
