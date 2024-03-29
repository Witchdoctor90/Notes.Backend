using FluentValidation;
using Notes.Domain;

namespace Notes.Application.Notes.Queries;

public class GetNoteDetailsQueryValidator : AbstractValidator<GetNoteDetailsQuery>
{
    public GetNoteDetailsQueryValidator()
    {
        RuleFor(note => note.UserId).NotEqual(Guid.Empty);
        RuleFor(note => note.Id).NotEqual(Guid.Empty);
    }
}