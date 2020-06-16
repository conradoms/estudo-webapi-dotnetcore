using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Services.Interfaces;

namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        public async Task<List<TodoItemDTO>> FindAll()
        {
            using (var db = new TodoContext())
            {
                return await db.TodoItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();
            }
        }
        
        public async Task<TodoItemDTO> Find(int id)
        {
            using (var db = new TodoContext())
            {
                var todoItem = await db.TodoItems.FindAsync(id);

                if (todoItem != null)
                {
                    return ItemToDTO(todoItem);
                }

                return null;
            }
        }

        public async Task<int> Create(TodoItemDTO todoItemDTO)
        {
            var todoItem = new TodoItem
            {
                IsComplete = todoItemDTO.IsComplete,
                Name = todoItemDTO.Name
            };

            using (var db = new TodoContext())
            {
                db.TodoItems.Add(todoItem);
                await db.SaveChangesAsync();

                return todoItem.Id;
            }
        }

        public async Task Update(TodoItemDTO todoItemDTO)
        {
            using (var db = new TodoContext())
            {
                var todoItem = await db.TodoItems.FindAsync(todoItemDTO.Id);
          
                todoItem.Name = todoItemDTO.Name;
                todoItem.IsComplete = todoItemDTO.IsComplete;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<TodoItemDTO> Delete(int id)
        {
            using (var db = new TodoContext())
            {
                var todoItem = await db.TodoItems.FindAsync(id);
                db.TodoItems.Remove(todoItem);
                await db.SaveChangesAsync();

                return ItemToDTO(todoItem);
            }
        }

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
        new TodoItemDTO
        {
            Id = todoItem.Id,
            Name = todoItem.Name,
            IsComplete = todoItem.IsComplete
        };

        public async Task<bool> Exists(int id)
        {
            using (var db = new TodoContext())
            {
                return await db.TodoItems.AnyAsync(e => e.Id == id);
            }
        }
    }
}