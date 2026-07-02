using TaskManagementAPI.Models;

namespace TaskManagementAPI.Interfaces;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllAsync(TaskItemStatus? status);
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem> AddAsync(TaskItem task);
    Task<TaskItem> UpdateAsync(TaskItem task);
    Task DeleteAsync(TaskItem task);
    Task<IReadOnlyList<TaskItem>> GetOverduePendingAsync(DateTime nowUtc);
}
