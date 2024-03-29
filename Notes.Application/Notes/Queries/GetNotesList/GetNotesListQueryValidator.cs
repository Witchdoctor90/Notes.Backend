using FluentValidation;

namespace Notes.Application.Notes.Queries.GetNotesList;

public class GetNotesListQueryValidator : AbstractValidator<GetNotesListQuery>
{
    public GetNotesListQueryValidator()
    {
        RuleFor(notes => notes.UserId).NotEqual(Guid.Empty);
    }
}