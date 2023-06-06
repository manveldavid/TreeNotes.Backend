using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Tests.Common
{
    public abstract class TestBase:IDisposable
    {
        protected readonly NotesDbContext _notes; 
        protected readonly UsersDbContext _users;
        public TestBase()
        {
            _notes = ContentFactory.CreateNoteDB();
            _users = ContentFactory.CreateUserDB();
        }
        public void Dispose()
        {
            ContentFactory.Destroy(_notes);
            ContentFactory.Destroy(_users);
        }
    }
}
