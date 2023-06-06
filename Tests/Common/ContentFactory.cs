using Application.Common.Workers;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Tests.Common
{
    public static class ContentFactory
    {
        #region GlobalData

        public static string UserA_Login = "userA";
        public static string UserB_Login = "userB";
        public static string UserC_Login = "userC";

        public static string Password = "password";

        public static Guid UserA_Id = Guid.NewGuid();
        public static Guid UserB_Id = Guid.NewGuid();
        public static Guid UserC_Id = Guid.NewGuid();

        public static Guid NoteId_Root = Guid.NewGuid();
        public static Guid NoteId_Parent_A = Guid.NewGuid();
        public static Guid NoteId_Parent_B = Guid.NewGuid();
        public static Guid NoteId_Parent_C = Guid.NewGuid();
        public static Guid NoteId_Child_A = Guid.NewGuid();
        public static Guid NoteId_Child_B = Guid.NewGuid();
        public static Guid NoteId_Child_C = Guid.NewGuid();
        public static Guid NoteId_Child_D = Guid.NewGuid();
        public static Guid NoteId_Child_E = Guid.NewGuid();
        public static Guid NoteId_Child_F = Guid.NewGuid();

        #endregion

        public static NotesDbContext CreateNoteDB()
        {
            var options = new DbContextOptionsBuilder<NotesDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var noteDbContext = new NotesDbContext(options);
            noteDbContext.Database.EnsureCreated();
            noteDbContext.Set.AddRange(
            #region Notes
                    new TreeNote
                    {
                        Description = "root",
                        Id = NoteId_Root,
                        User = Guid.Empty,
                        Title = "noteRoot",
                        Creator = UserC_Id,
                        Share = false,
                        Check = false,
                        Creation = DateTime.Today,
                        LastEdit = DateTime.Today,
                        Parent = Guid.Empty,
                        Number = 1,
                        Weight = 100
                    },
                        new TreeNote
                        {
                            Description = "parent",
                            Id = NoteId_Parent_A,
                            Parent = NoteId_Root,
                            User = UserA_Id,
                            Title = "noteFirstParent",
                            Creator = UserC_Id,
                            Share = false,
                            Check = false,
                            Creation = DateTime.Today,
                            LastEdit = DateTime.Today,
                            Number = 2,
                            Weight = 1
                        },
                            #region A_Childs
                            new TreeNote
                            {
                                Description = "child",
                                Id = NoteId_Child_A,
                                Parent = NoteId_Parent_A,
                                User = Guid.Empty,
                                Title = "noteFirstChild",
                                Creator = UserA_Id,
                                Share = false,
                                Check = false,
                                Creation = DateTime.Today,
                                LastEdit = DateTime.Today,
                                Number = 3,
                                Weight = 1
                            },
                            new TreeNote
                            {
                                Description = "child",
                                Id = NoteId_Child_B,
                                Parent = NoteId_Parent_A,
                                User = Guid.Empty,
                                Title = "noteSecondChild",
                                Creator = UserA_Id,
                                Share = false,
                                Check = false,
                                Creation = DateTime.Today,
                                LastEdit = DateTime.Today,
                                Number = 4,
                                Weight = 1
                            },
                            new TreeNote
                            {
                                Description = "child",
                                Id = NoteId_Child_C,
                                Parent = NoteId_Parent_A,
                                User = Guid.Empty,
                                Title = "noteThirdChild",
                                Creator = UserA_Id,
                                Share = false,
                                Check = false,
                                Creation = DateTime.Today,
                                LastEdit = DateTime.Today,
                                Number = 5,
                                Weight = 1
                            },
                            #endregion

                        new TreeNote
                        {
                            Description = "parent",
                            Id = NoteId_Parent_B,
                            Parent = NoteId_Root,
                            User = UserB_Id,
                            Title = "noteSecondParent",
                            Creator = UserC_Id,
                            Share = false,
                            Check = true,
                            Creation = DateTime.Today,
                            LastEdit = DateTime.Today,
                            Number = 6,
                            Weight = 1
                        },
                            #region B_Childs
                            new TreeNote
                            {
                                Description = "child",
                                Id = NoteId_Child_D,
                                Parent = NoteId_Parent_B,
                                User = Guid.Empty,
                                Title = "noteFirstChild",
                                Creator = UserB_Id,
                                Share = false,
                                Check = false,
                                Creation = DateTime.Today,
                                LastEdit = DateTime.Today,
                                Number = 7,
                                Weight = 1
                            },
                            new TreeNote
                            {
                                Description = "child",
                                Id = NoteId_Child_E,
                                Parent = NoteId_Parent_B,
                                User = Guid.Empty,
                                Title = "noteSecondChild",
                                Creator = UserB_Id,
                                Share = false,
                                Check = false,
                                Creation = DateTime.Today,
                                LastEdit = DateTime.Today,
                                Number = 8,
                                Weight = 1
                            },
                            #endregion

                        new TreeNote
                        {
                            Description = "parent",
                            Id = NoteId_Parent_C,
                            Parent = NoteId_Root,
                            User = Guid.Empty,
                            Title = "noteThirdParent",
                            Creator = UserC_Id,
                            Share = true,
                            Check = false,
                            Creation = DateTime.Today,
                            LastEdit = DateTime.Today,
                            Number = 9,
                            Weight = 1
                        },
                            #region C_Childs
                            new TreeNote
                            {
                                Description = "child",
                                Id = NoteId_Child_F,
                                Parent = NoteId_Parent_C,
                                User = Guid.Empty,
                                Title = "noteFirstChild",
                                Creator = UserC_Id,
                                Share = true,
                                Check = false,
                                Creation = DateTime.Today,
                                LastEdit = DateTime.Today,
                                Number = 10,
                                Weight = 1
                            }
                            #endregion
                    #endregion
                );
            noteDbContext.SaveChanges();
            return noteDbContext;
        }
        public static UsersDbContext CreateUserDB()
        {
            var options = new DbContextOptionsBuilder<UsersDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var userDbContext = new UsersDbContext(options);
            userDbContext.Database.EnsureCreated();
            userDbContext.Set.AddRange(
            #region Users
                new TreeNoteUser
                {
                    Id = UserA_Id,
                    Login = UserA_Login,
                    Code = TreeNoteUserWorker.Encoder.CodeFromLoginPassword(UserA_Login,Password)
                },
                new TreeNoteUser
                {
                    Id = UserB_Id,
                    Login = UserB_Login,
                    Code = TreeNoteUserWorker.Encoder.CodeFromLoginPassword(UserB_Login, Password)
                },
                new TreeNoteUser
                {
                    Id = UserC_Id,
                    Login = UserC_Login,
                    Code = TreeNoteUserWorker.Encoder.CodeFromLoginPassword(UserC_Login, Password)
                }
                #endregion
                );
            userDbContext.SaveChanges();
            return userDbContext;
        }
        public static void Destroy(DbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
