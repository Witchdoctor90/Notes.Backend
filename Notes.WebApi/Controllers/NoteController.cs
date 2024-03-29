using System;
using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Notes.Commands.DeleteNote;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Application.Notes.Queries;
using Notes.Application.Notes.Queries.GetNotesList;
using Notes.WebApi.Models;

namespace Notes.WebApi.Controllers;

[Produces("application/json")]
[Route("api/[controller]/[action]")]
public class NoteController : BaseController
{
    private readonly IMapper _mapper;

    public NoteController(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    /// <summary>
    /// Gets the list of notes
    /// </summary>
    /// <remarks>Sample request:
    /// GET /note
    /// </remarks>
    /// <returns>Returns NoteListVm</returns>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<NotesListVm>> GetAll()
    {
        var query = new GetNotesListQuery()
        {
            UserId = UserId
        };
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }
    
    /// <summary>
    /// Get detailed note
    /// </summary>
    /// <param name="id">Id of the note</param>
    /// <remarks>Sample request:
    /// GET /note/Get/A72173A8-3C44-4934-9F4E-FE668ABC6A58
    /// </remarks>
    /// <returns>Returns NoteDetailsVm</returns>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<NoteDetailsVm>> Get([FromBody]Guid id)
    {
        var query = new GetNoteDetailsQuery()
        {
            UserId = UserId,
            Id = id
        };
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }
    
    /// <summary>
    /// Create a new note
    /// </summary>
    /// <param name="createNoteDto">A note to create</param>
    /// <remarks>Sample request:
    /// POST /note/Create/
    /// {
    ///     title: "note title"
    ///     details: "note details"
    /// }
    /// </remarks>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <returns>Note id</returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateNoteDto createNoteDto)
    {
        var command = _mapper.Map<CreateNoteCommand>(createNoteDto);
        command.UserId = UserId;
        var noteId = await Mediator.Send(command);
        return Ok(noteId);
    }
    
    /// <summary>
    /// Update an existing note
    /// </summary>
    /// <param name="updateNoteDto">A note to update</param>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <remarks>Sample request:
    /// PUT /note/Update
    /// {
    ///     id: "1CD400AE-BB62-4E61-87A6-72096F283D62",
    ///     title: "new title"
    ///     details: "new details"
    /// }
    /// </remarks>
    /// <returns>No Content</returns>
    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update([FromBody] UpdateNoteDto updateNoteDto)
    {
        var command = _mapper.Map<UpdateNoteCommand>(updateNoteDto);
        command.UserId = UserId;
        await Mediator.Send(command);
        return NoContent();
    }
    
    /// <summary>
    /// Delete an existing note
    /// </summary>
    /// <param name="id">An id of a note to delete</param>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <remarks>Sample request:
    /// DELETE /note/Delete
    /// {
    ///     id: "1CD400AE-BB62-4E61-87A6-72096F283D62",
    /// }
    /// </remarks>
    /// <returns>No Content</returns>
    [HttpDelete]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        var command = new DeleteNoteCommand()
        {
            Id = id,
            UserId = UserId
        };
        await Mediator.Send(command);
        return NoContent();
    }
    
}