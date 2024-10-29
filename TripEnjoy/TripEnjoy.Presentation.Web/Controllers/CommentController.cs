using AutoMapper;
using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface;
using TripEnjoy.Application.Interface.Comment;
using TripEnjoy.Application.Interface.Room;

namespace TripEnjoy.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IAccountService _accountService;
        private readonly IRoomService _roomService;
        private readonly IMapper _mapper;

        public CommentController(ICommentService commentService, IAccountService accountService, IRoomService roomService, IMapper mapper)
        {
            _commentService = commentService;
            _accountService = accountService;
            _roomService = roomService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComment()
        {
            var comments = await _commentService.GetAllCommentAsync();
            return Ok(comments);    
        }

        // GET: api/Comment/Room/{roomId}
        [HttpGet("Room/{roomId}")]
        public async Task<IActionResult> GetCommentsByRoomId(int roomId)
        {
            var comments = await _commentService.GetCommentByRoomIdAsync(roomId);
            if (comments == null || !comments.Any())
            {
                return NotFound("No comments found for this room.");
            }

            return Ok(comments);
        }

        // POST: api/Comment
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CommentResponseDTO obj)
        {
            // Get user information
            ClaimsPrincipal claims = this.User;
            var email = claims.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            var account = await _accountService.GetAccountByEmail(email);
            if (account == null)
            {
                return NotFound("Account not found.");
            }

            int accountId = account.AccountId;

            CommentDTO comment = new CommentDTO
            {
                CommentLevel = 1, // Main comment
                CommentContent = obj.Content,
                RoomId = obj.RoomId,
                AccountId = accountId,
                CommentDate = DateTime.Now,
                CommentReply = "0" // Not a reply, just a comment
            };

            // mapper
            var commentModel = _mapper.Map<TripEnjoy.Domain.Models.Comment>(comment);
            commentModel = await _commentService.CreateCommentAsync(commentModel);

            // Update comment count in the related room
            var room = await _roomService.GetRoomDetailByIdAsync(obj.RoomId);
            if (room == null)
            {
                return NotFound("Room not found.");
            }

            return Ok(new { RoomID = obj.RoomId, Comment = comment });

        }

        // POST: api/Comment/AddReply
        [HttpPost("Reply")]
        public async Task<IActionResult> AddReplyComment([FromBody] CommentResponseDTO obj)
        {
            // Get user information
            ClaimsPrincipal claims = this.User;
            var email = claims.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            var account = await _accountService.GetAccountByEmail(email);
            if (account == null)
            {
                return NotFound("Account not found.");
            }
            int accountId = account.AccountId;
            CommentDTO replyComment = new CommentDTO
            {
                CommentLevel = 2, // Main comment
                CommentContent = obj.Content,
                RoomId = obj.RoomId,
                AccountId = accountId,
                CommentDate = DateTime.Now,
                CommentReply = obj.ReplyToComment
            };

            // mapper
            var commentModel = _mapper.Map<TripEnjoy.Domain.Models.Comment>(replyComment);
            commentModel = await _commentService.CreateCommentAsync(commentModel);

            return Ok(new { Comment = replyComment });
        }
    }
}
