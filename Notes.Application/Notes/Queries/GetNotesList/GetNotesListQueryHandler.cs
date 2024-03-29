using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Queries.GetNotesList;

public class GetNotesListQueryHandler : IRequestHandler<GetNotesListQuery, NotesListVm>
{
    private readonly INotesDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetNotesListQueryHandler(INotesDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<NotesListVm> Handle(GetNotesListQuery request, CancellationToken cancellationToken)
    {
        var notesQuery = await _dbContext.Notes.Where(n => n.UserId == request.UserId)
            .ProjectTo<NoteLookupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        return new NotesListVm { Notes = notesQuery };
    }
}