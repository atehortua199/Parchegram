﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Parchegram.Model.Request.Comment;
using Parchegram.Model.Response.General;
using Parchegram.Service.Services.Interfaces;
using System.Threading.Tasks;

namespace Parchegram.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IWebHostEnvironment _env;

        public CommentController(ICommentService commentService,
            IWebHostEnvironment env)
        {
            _commentService = commentService;
            _env = env;
        }

        [HttpGet("Get/{idPost}/{limit?}")]
        public async Task<IActionResult> Get([FromRoute] int idPost, [FromRoute] int limit = 0)
        {
            Response result = await _commentService.GetCommentsByPost(idPost, limit);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PostCommentRequest postCommentRequest)
        {
            if (ModelState.IsValid)
            {
                Response result = await _commentService.PostComment(postCommentRequest);
                return Ok(result);
            }
            Response response = new Response();
            return BadRequest(response.GetResponse("Modelo no valido", 0, false));
        }

        [HttpDelete("Delete/{idPost}/{idComment}/{NameUser}")]
        public async Task<IActionResult> Delete([FromRoute] int idPost, [FromRoute] int idComment, [FromRoute] string nameUser)
        {
            DeleteCommentRequest deleteCommentRequest = new DeleteCommentRequest(idPost, idComment, nameUser);
            Response result = await _commentService.DeleteComment(deleteCommentRequest);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateCommentRequest updateCommentRequest)
        {
            if (ModelState.IsValid)
            {
                Response result = await _commentService.UpdateComment(updateCommentRequest);
                return Ok(result);
            }
            Response response = new Response();
            return BadRequest(response.GetResponse("Modelo no valido", 0, false));
        }
    }
}
