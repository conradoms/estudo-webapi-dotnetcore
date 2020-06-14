using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.DTOs;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        public TodoItemsController()
        {
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            using (var db = new TodoContext())
            {
                return await db.TodoItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();
            }
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(int id)
        {
            using (var db = new TodoContext())
            {
                var todoItem = await db.TodoItems.FindAsync(id);

                if (todoItem == null)
                {
                    return NotFound();
                }

                return ItemToDTO(todoItem);
            }
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(int id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            using (var db = new TodoContext())
            {
                var todoItem = await db.TodoItems.FindAsync(id);
                if (todoItem == null)
                {
                    return NotFound();
                }

                todoItem.Name = todoItemDTO.Name;
                todoItem.IsComplete = todoItemDTO.IsComplete;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoItemExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItemDTO todoItemDTO)
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

                return CreatedAtAction(nameof(GetTodoItem),
                 new { id = todoItem.Id },
                 todoItemDTO);
            }
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(int id)
        {
            using (var db = new TodoContext())
            {
                var todoItem = await db.TodoItems.FindAsync(id);
                if (todoItem == null)
                {
                    return NotFound();
                }

                db.TodoItems.Remove(todoItem);
                await db.SaveChangesAsync();

                return todoItem;
            }
        }

        private bool TodoItemExists(int id)
        {
            using (var db = new TodoContext())
            {
                return db.TodoItems.Any(e => e.Id == id);
            }
        }

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
        new TodoItemDTO
        {
            Id = todoItem.Id,
            Name = todoItem.Name,
            IsComplete = todoItem.IsComplete
        };
    }
}
