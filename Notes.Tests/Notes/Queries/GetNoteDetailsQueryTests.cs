using System.Xml.XPath;
using AutoMapper;
using Notes.Application.Notes.Queries;
using Notes.Persistence;
using Notes.Tests.Common;
using Shouldly;

namespace Notes.Tests.Notes.Queries;

[Collection("QueryCollection")]

public class GetNoteDetailsQueryTests
{
    private readonly NotesDbContext Context;
    private readonly IMapper Mapper;

    public GetNoteDetailsQueryTests(QueryTestFixture fixture)
    {
        Context = fixture.Context;
        Mapper = fixture.Mapper;
    }

    [Fact]
    public async Task GetNoteDetailsQuery_Success()
    {
        //Arrange
        var handler = new GetNoteDetailsQueryHandler(Context, Mapper);
        
        //Act
        var result = await handler.Handle(new GetNoteDetailsQuery()
        {
            UserId = NotesContextFactory.UserBId,
            Id = Guid.Parse("8EFBFA3F-C5BB-4CBF-97B7-C5802FF15CF2")
        }, CancellationToken.None);
        //Assert
        result.ShouldBeOfType<NoteDetailsVm>();
        result.Title.ShouldBe("Title2");
        result.CreationDate.ShouldBe(DateTime.Today);
    }
}