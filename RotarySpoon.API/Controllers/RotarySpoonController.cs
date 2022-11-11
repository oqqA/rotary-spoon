using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using RotarySpoon.API.Contracts;
using RotarySpoon.DataAccess;

namespace RotarySpoon.API.Controllers;

[ApiController]
[Route("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class RotarySpoonController : ControllerBase
{
    private readonly ILogger<RotarySpoonController> _logger;

    public RotarySpoonController(ILogger<RotarySpoonController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetTodoListsResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get()
    {
        var todoList = TodoItemsRepository.Get();

        var response = new GetTodoListsResponse
        {
            TodoList = todoList.Select(x => new GetTodoListDto
            {
                Id = x.Id,
                Text = x.Text,
                CreatedDate = x.CreatedDate
            }).ToArray()
        };

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Create([FromBody] CreateTodoListRequest request)
    {
        var todoItem = new TodoItem
        {
            Id = Guid.NewGuid(),
            Text = request.Text,
            CreatedDate = DateTimeOffset.UtcNow
        };

        var todoItemId = TodoItemsRepository.Add(todoItem);
        return Ok(todoItemId);
    }

    [HttpPut]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update([FromBody] UpdateTodoListRequest request)
    {
        var status = TodoItemsRepository.Update(request.Id, request.Text);
        return Ok(status);
    }

    [HttpDelete]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Close([FromBody] CloseTodoListRequest request)
    {
        var status = TodoItemsRepository.Close(request.Id);
        return Ok(status);
    }

}