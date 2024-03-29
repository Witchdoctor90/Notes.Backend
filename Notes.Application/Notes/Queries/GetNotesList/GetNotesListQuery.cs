using MediatR;

namespace Notes.Application.Notes.Queries.GetNotesList;

public class GetNotesListQuery : IRequest<NotesListVm>
{
    public Guid UserId { get; set; }
    
}   