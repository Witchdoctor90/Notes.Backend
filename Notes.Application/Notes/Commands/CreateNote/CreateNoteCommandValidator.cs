﻿using FluentValidation;

namespace Notes.Application.Notes.Commands.CreateNote;

public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
{
    public CreateNoteCommandValidator()
    {
        RuleFor(command => command.Title)
            .NotEmpty().MaximumLength(250);
        RuleFor(command => command.UserId).NotEqual(Guid.Empty);
    }
}