using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.DTOs;

namespace TodoApi.Services.Interfaces
{
    public interface ITodoService
    {
        Task<List<TodoItemDTO>> FindAll();
        Task<TodoItemDTO> Find(int id);
        Task<int> Create(TodoItemDTO todoItemDTO);
        Task Update(TodoItemDTO todoItemDTO);
        Task<TodoItemDTO> Delete(int id);
        Task<bool> Exists(int id);
    }
}