using Application.Interfaces;
using Domain;

namespace Application.Common.Workers
{
    public static class TreeNoteWorker
    {
        public static class Finder
        {
            public static IQueryable<TreeNote> NoteFromId(IDbContext<TreeNote> dbContext, Guid noteId)
            {
                var entity = dbContext.Set.Where(x => x.Id == noteId);

                return entity;
            }

            public static IQueryable<TreeNote> RootNotesFromUserId(IDbContext<TreeNote> dbContext, Guid userId)
            {
                var entity = dbContext.Set
                    .Where(x =>
                    (x.Creator == userId && x.Parent == Guid.Empty) ||
                            x.User == userId);

                return entity;
            }

            public static IQueryable<TreeNote> NotesFromTitle(IDbContext<TreeNote> dbContext, Guid userId, string fragment)
            {
                var entity = dbContext.Set
                    .Where(note =>
                    (note.Creator == userId || note.User == userId) &&
                    (note.Title.ToLower().Contains(fragment.ToLower())));

                return entity;
            }

            public static IQueryable<TreeNote> NotesFromDescription(IDbContext<TreeNote> dbContext, Guid userId, string fragment)
            {
                var entity = dbContext.Set
                    .Where(note =>
                    (note.Creator == userId || note.User == userId) &&
                    (note.Description.ToLower().Contains(fragment.ToLower())));

                return entity;
            }

            public static IQueryable<TreeNote> AllChildsFromId(IDbContext<TreeNote> dbContext, Guid entityId)
            {
                var childs = dbContext.Set.Where(n => n.Parent == entityId);

                if (childs.Count() > 0)
                {
                    var result = childs;
                    foreach (var child in childs)
                    {
                        result = result.Concat(AllChildsFromId(dbContext, child.Id));
                    }

                    return result;
                }

                return childs;
            }
            public static IQueryable<TreeNote> FirstChildsFromId(IDbContext<TreeNote> dbContext, Guid noteId)
            {
                var childs = dbContext.Set.Where(n => n.Parent == noteId);

                return childs;
            }

            public static IQueryable<TreeNote> AllParentsFromId(IDbContext<TreeNote> dbContext, Guid noteId)
            {
                var parents = NoteFromId(dbContext, noteId);
                noteId = parents.Count() > 0 ?
                    parents.First().Parent : Guid.Empty;

                while (noteId != Guid.Empty)
                {
                    IQueryable<TreeNote> parent = NoteFromId(dbContext, noteId);
                    noteId = parent.Count() > 0 ?
                        parent.First().Parent : Guid.Empty;
                    parents = parents.Concat(parent);
                }

                return parents;
            }
        }
    }
}
