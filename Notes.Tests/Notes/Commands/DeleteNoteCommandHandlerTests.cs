﻿using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Notes.Commands.DeleteNote;

namespace Notes.Tests.Common.Notes.Commands;

public class DeleteNoteCommandHandlerTests : TestCommandBase
{
    
    
    [Fact]
    public async Task DeleteNoteCommandHandler_Success()
    {
        //Arrange
        var handler = new DeleteNoteCommandHandler(Context);
        
        //Act
        await handler.Handle(new DeleteNoteCommand()
        {
            Id = NotesContextFactory.NoteIdForDelete,
            UserId = NotesContextFactory.UserAId
        }, CancellationToken.None);
        
        //Assert
        Assert.Null(await Context.Notes.SingleOrDefaultAsync(note => 
            note.Id == NotesContextFactory.NoteIdForDelete));
    }

    [Fact]
    public async Task DeleteNoteCommandHandler_FailOnWrongId()
    {
        //Arrange
        var handler = new DeleteNoteCommandHandler(Context);
        
        //Act
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await handler.Handle(new DeleteNoteCommand()
            {
                Id = Guid.NewGuid(),
                UserId = NotesContextFactory.UserAId
            }, CancellationToken.None);
        });
    }

    [Fact]
    public async Task DeleteNoteCommandHandler_FailOnWrongUserId()
    {
        //Arrange
        var deleteHandler = new DeleteNoteCommandHandler(Context);
        var createHandler = new CreateNoteCommandHandler(Context);
        var noteId = await createHandler.Handle(new CreateNoteCommand
        {
            UserId = NotesContextFactory.UserAId,
            Title = "NoteTitle",
            Details = "NoteDetails"
        }, CancellationToken.None);
        //Act
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await deleteHandler.Handle(new DeleteNoteCommand()
            {
                Id = noteId,
                UserId = NotesContextFactory.UserBId
            }, CancellationToken.None);
        });
    }
}