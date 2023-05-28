using System;
using DemoWebApp.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Models;

namespace DemoWebApp.Controllers
{
    [ApiController]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        private readonly ApplicationDbContext _chats;

        public ChatsController(ApplicationDbContext chats)
        {
            _chats = chats;
        }

        [HttpGet("api/chats")]
        public IActionResult Read(
            [FromQuery] string orderBy = "Id",
            [FromQuery] string order = "asc",
            [FromQuery] int page = 1,
            [FromQuery] int perPage = 25)
        {
            // to-do: додати можливість отримувати cписок доступних для користувача чатів

            var userChats = _chats.Chats.Where(Chat => Chat.Id == order).ToList();

            if (userChats.Count == 0)
            {
                return NotFound();
            }

            return Ok(userChats);

        }

        [HttpPost("api/chats")]
        public ActionResult<Chat> CreateChat(Chat chat)
        {
            var newChatId = _chats.Count + 1;
            chat.Id = newChatId;

            _chats.Add(chat);

            return CreatedAtAction(nameof(GetChat), new { id = chat.Id }, chat);
        }

        [HttpDelete("api/chats/{id}")]
        public IActionResult Delete(int id)
        {
            // to-do: додати можливість видаляти чат

            var chat = _chats.FirstOrDefault(c => c.Id == id);
            if (chat == null)
            {
                return NotFound();
            }

            _chats.Remove(chat);

            return NoContent();
        }

        [HttpGet("api/chats/{chatId}/messages")]
        public ActionResult<IEnumerable<Message>> ReadMessages(
            int chatId,
            [FromQuery] string orderBy = "Id",
            [FromQuery] string order = "asc",
            [FromQuery] int page = 1,
            [FromQuery] int perPage = 25)
        {
            // to-do: додати можливість отримувати список повідомлень у чаті

            var chat = _chats.FirstOrDefault(c => c.Id == chatId);
            if (chat == null)
            {
                return NotFound();
            }

            var sortedMessages = order.ToLower() == "desc"
                ? chat.Messages.OrderByDescending(GetSortExpression(orderBy))
                : chat.Messages.OrderBy(GetSortExpression(orderBy));

            var paginatedMessages = sortedMessages
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .ToList();

            return Ok(paginatedMessages);
        }
    }
}