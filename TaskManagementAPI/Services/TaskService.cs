using TaskManagementAPI.DTOs;
using TaskManagementAPI.Interfaces;
using TaskManagementAPI.Middleware;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly ILogger<TaskService> _logger;

    public TaskService(ITaskRepository repository, ILogger<TaskService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<TaskResponseDto>> GetAllAsync(TaskItemStatus? status)
    {
        var tasks = await _repository.GetAllAsync(status);
        return tasks.Select(MapToDto);
    }

    public async Task<TaskResponseDto> GetByIdAsync(int id)
    {
        var task = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Task with Id {id} was not found.");

        return MapToDto(task);
    }

    public async Task<TaskResponseDto> CreateAsync(CreateTaskDto dto)
    {
        if (dto.DueDate < DateTime.UtcNow)
            throw new BadRequestException("DueDate cannot be in the past.");

        var task = new TaskItem
        {
            Title = dto.Title.Trim(),
            Description = dto.Description,
            DueDate = dto.DueDate,
            Status = TaskItemStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(task);
        _logger.LogInformation("Created task {TaskId} ({Title}).", created.Id, created.Title);

        return MapToDto(created);
    }

    public async Task<TaskResponseDto> UpdateAsync(int id, UpdateTaskDto dto)
    {
        var task = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Task with Id {id} was not found.");

        task.Title = dto.Title.Trim();
        task.Description = dto.Description;
        task.DueDate = dto.DueDate;
        task.Status = dto.Status;
        task.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(task);
        _logger.LogInformation("Updated task {TaskId}.", updated.Id);

        return MapToDto(updated);
    }

    public async Task DeleteAsync(int id)
    {
        var task = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Task with Id {id} was not found.");

        await _repository.DeleteAsync(task);
        _logger.LogInformation("Deleted task {TaskId}.", id);
    }

    private static TaskResponseDto MapToDto(TaskItem t) => new()
    {
        Id = t.Id,
        Title = t.Title,
        Description = t.Description,
        DueDate = t.DueDate,
        Status = t.Status.ToString(),
        CreatedAt = t.CreatedAt,
        UpdatedAt = t.UpdatedAt
    };
}
