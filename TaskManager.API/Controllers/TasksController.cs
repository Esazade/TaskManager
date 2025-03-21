using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;

    public TasksController(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
    {
        var tasks = await _taskRepository.GetAllAsync();
        return Ok(tasks);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetTask(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
            return NotFound();

        return Ok(task);
    }

     [HttpPost]
    public async Task<ActionResult<TaskItem>> CreateTask(TaskCreateDto taskDto)
    {
        var task = new TaskItem
        {
            Title = taskDto.Title,
            Description = taskDto.Description,
            IsCompleted = taskDto.IsCompleted,
            DueDate = taskDto.DueDate
        };

        await _taskRepository.AddAsync(task);
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, TaskUpdateDto taskDto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
            return NotFound();

        task.Title = taskDto.Title;
        task.Description = taskDto.Description;
        task.IsCompleted = taskDto.IsCompleted;
        task.DueDate = taskDto.DueDate;

        await _taskRepository.UpdateAsync(task);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
            return NotFound();

        await _taskRepository.DeleteAsync(task);
        return NoContent();
    }
}
