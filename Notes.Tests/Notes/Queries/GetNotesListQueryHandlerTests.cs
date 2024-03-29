using System.Diagnostics;
using AutoMapper;
using Notes.Application.Notes.Queries.GetNotesList;
using Notes.Persistence;
using Notes.Tests.Common;
using Shouldly;

namespace Notes.Tests.Notes.Queries;

[Collection("QueryCollection")]
public class GetNotesListQueryHandlerTests
{
    private readonly NotesDbContext Context;
    private readonly IMapper Mapper;

    public GetNotesListQueryHandlerTests(QueryTestFixture fixture)
    {
        Context = fixture.Context;
        Mapper = fixture.Mapper;
    }
    
    [Fact]
    public async Task GetNoteListQueryHandler_Success()
    {
        //Arrange
        var handler = new GetNotesListQueryHandler(Context, Mapper);
        
        //Act
        var result = await handler.Handle(new GetNotesListQuery()
        {
            UserId = NotesContextFactory.UserBId
        }, CancellationToken.None);
        
        //Assert
        result.ShouldBeOfType<NotesListVm>();
        result.Notes.Count.ShouldBe(2);
    }
}