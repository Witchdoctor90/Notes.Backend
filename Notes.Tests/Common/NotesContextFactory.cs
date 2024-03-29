using Notes.Persistence;
using Microsoft.EntityFrameworkCore;
using Notes.Domain;

namespace Notes.Tests.Common;

public class NotesContextFactory
{
    public static Guid UserAId = Guid.NewGuid();
    public static Guid UserBId = Guid.NewGuid();

    public static Guid NoteIdForDelete = Guid.NewGuid();
    public static Guid NoteIdForUpdate = Guid.NewGuid();

    public static NotesDbContext Create()
    {
        var options = new DbContextOptionsBuilder<NotesDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new NotesDbContext(options);
        context.Database.EnsureCreated();
        context.Notes.AddRange(
            new Note
            {
                CreationDate = DateTime.Today,
                Details = "Details1",
                EditDate = null,
                Id = Guid.Parse("32AFB44E-304E-443F-95D7-4C02BED1B53B"),
                Title = "Title1",
                UserId = UserAId
            },
            new Note
            {
                CreationDate = DateTime.Today,
                Details = "Details2",
                EditDate = null,
                Id = Guid.Parse("8EFBFA3F-C5BB-4CBF-97B7-C5802FF15CF2"),
                Title = "Title2",
                UserId = UserBId
            },
            new Note
            {
                CreationDate = DateTime.Today,
                Details = "Details3",
                EditDate = null,
                Id = NoteIdForDelete,
                Title = "Title3",
                UserId = UserAId
            },
            new Note
            {
                CreationDate = DateTime.Today,
                Details = "Details4",
                EditDate = null,
                Id = NoteIdForUpdate,
                Title = "Title4",
                UserId = UserBId
            });
        context.SaveChanges();
        return context;
    }

    public static void Destroy(NotesDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}