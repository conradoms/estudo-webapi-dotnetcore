using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.DTOs;
using TodoApi.Services.Interfaces;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoService _todoService;
        public TodoItemsController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> Get()
        {
            return await _todoService.FindAll();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> Get(int id)
        {
            var todoItem = await _todoService.Find(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            if (!await _todoService.Exists(id))
            {
                return NotFound();
            }

            await _todoService.Update(todoItemDTO);

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> Post(TodoItemDTO todoItemDTO)
        {
            var idCreated = await _todoService.Create(todoItemDTO);

            todoItemDTO.Id = idCreated;

            return todoItemDTO;
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItemDTO>> Delete(int id)
        {
            if (!await _todoService.Exists(id))
            {
                return NotFound();
            }

            return await _todoService.Delete(id);
        }
    }
}
