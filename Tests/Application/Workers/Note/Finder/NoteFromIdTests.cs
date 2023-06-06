using Application.Common.Workers;
using Tests.Common;

namespace Tests.Application.Workers.Note.Finder
{
    public class NoteFromIdTests:TestBase
    {
        [Fact]
        public async Task NoteFromId_Success()
        {
            //Arrange
            var createDate = DateTime.Today;
            var description = "root";
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
            var note = TreeNoteWorker.Finder.NoteFromId(_notes, noteId)
                .FirstOrDefault(note =>

                note.Title == title &&
                note.Description == description &&

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
        public async Task NoteFromId_Fail_NoteId()
        {
            //Arrange
            var noteId = Guid.NewGuid();


            //Act
            var note = TreeNoteWorker.Finder.NoteFromId(_notes, noteId).FirstOrDefault();

            //Assert
            Assert.Null(note);
        }
    }
}
