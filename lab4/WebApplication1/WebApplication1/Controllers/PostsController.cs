using System;
using DemoWebApp.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Models;
using WebApplication1.Services.Repositories;
using System.ComponentModel.Design;
using System.Data;
using Microsoft.Extensions.Hosting;

namespace DemoWebApp.Controllers
{
    [ApiController]
    [Authorize(Roles = "admin,user")]
    public class PostsController : ControllerBase
    {
        private readonly PostsRepositoryService service;

        public PostsController(IRepository<Post> service)
        {
            this.service = (PostsRepositoryService)service;
        }

        [HttpGet("api/posts")]
        public IActionResult Read(
            [FromQuery] string orderBy = "Id",
            [FromQuery] string order = "asc",
            [FromQuery] int page = 1,
            [FromQuery] int perPage = 25)
        {
            var filterBy = new Post();

            return Ok(new Response()
            {
                Status = 200,
                Data = new
                {
                    count = service.Count(filterBy),
                    items = service.Read(filterBy, orderBy, order, page, perPage)
                }
            });
        }

        [HttpGet("api/posts/{id}")]
        public IActionResult ReadById(int id)
        {
            return Ok(new Response()
            {
                Status = 200,
                Data = service.Read(id)
            });
        }

        [HttpPost("api/posts")]
        public IActionResult Create([FromBody] Post post)
        {
            var user = Helpers.AuthHelper.GetUser(HttpContext.User);
            post.UserId = user.Id;
            post.PublishedOn = DateTime.Now;

            return Created(nameof(Post), new Response()
            {
                Status = 201,
                Data = service.Create(post)
            });
        }

        [HttpDelete("api/posts/{id}")]
        public IActionResult Delete(int id)
        {
            var user = Helpers.AuthHelper.GetUser(HttpContext.User);

            if (user.isAdmin == true || user == post.User)
            {
                service.Delete(id);

                return Ok(new Response()
                {
                    Status = 200
                });
            }
            return BadRequest(new Response()
            {
                Status = 400,
            });
        }

        [HttpPut("api/posts/{id}")]
        public IActionResult Update(int id, [FromBody] Post post)
        {
            var user = Helpers.AuthHelper.GetUser(HttpContext.User);

            if (user.isAdmin == true || user == post.User)
            {

                service.Update(post.Title);
                service.Update(post.Content);

                return Ok(new Response()
                {
                    Status = 200
                });
            }
            return BadRequest(new Response()
            {
                Status = 400,
            });
        }

        [HttpPost("api/posts/{postId}/comments")]
        public IActionResult CreateComment(int postId, [FromBody] CommentToPost comment)
        {
            var user = Helpers.AuthHelper.GetUser(HttpContext.User);
            comment.UserId = user.Id;
            comment.PostId = postId;
            comment.PublishedOn = DateTime.Now;

            if (comment.Content = null)
            {
                return BadRequest(new Response()
                {
                    Status = 400,
                });
            }

            return Created(nameof(CommentToPost), new Response()
            {
                Status = 201,
                Data = service.AddCommentToPost(comment)
            });
        }

        [HttpPut("api/posts/{postId}/comments/{commentId}")]
        public IActionResult UpdateComment(int postId, int commentId, [FromBody] CommentToPost comment)
        {

            var user = Helpers.AuthHelper.GetUser(HttpContext.User);

            if (user.isAdmin == true || user == post.User)
            {
                if (comment.Content = null)
                {
                    return BadRequest(new Response()
                    {
                        Status = 400,
                    });
                }

                service.Update(comment.Content);

                return Ok(new Response()
                {
                    Status = 200
                });
            }
            return BadRequest(new Response()
            {
                Status = 400,
            });
        }

        [HttpDelete("api/posts/{postId}/comments/{commentId}")]
        public IActionResult DeleteComment(int postId, int commentId, [FromBody] CommentToPost comment)
        {
            var user = Helpers.AuthHelper.GetUser(HttpContext.User);

            if (user == post.User && comment.PostId == postId)
            {
                service.Delete(comment);

                return Ok(new Response()
                {
                    Status = 200
                });
            }
            return BadRequest(new Response()
            {
                Status = 400,
            });
        }


    }
}