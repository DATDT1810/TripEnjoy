using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface;
using TripEnjoy.Application.Interface.Comment;
using TripEnjoy.Application.Interface.Rate;
using TripEnjoy.Application.Interface.Room;
using TripEnjoy.Domain.Models;
using System.Linq;


namespace TripEnjoy.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IRateService _rateService;
        private readonly IRoomService _roomService;
        private readonly IAccountService _accountService;
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public RateController(IRateService rateService, IRoomService roomService, IAccountService accountService, ICommentService commentService, IMapper mapper)
        {
            _rateService = rateService;
            _roomService = roomService;
            _accountService = accountService;
            _commentService = commentService;
            _mapper = mapper;
        }

        // GET: api/Rate/
        [HttpGet]
        public async Task<IActionResult> GetAllRates()
        {
            var rates = await _rateService.GetAllRateAsync();
            return Ok(rates);
        }

        // GET: api/Rate/Room/{id}
        [HttpGet("Room/{id}")]
        public async Task<IActionResult> GetRatesForRoom(int id)
        {
            var room = await _roomService.GetRoomDetailByIdAsync(id);
            if (room == null)
            {
                return NotFound("Room not found");
            }
            var rates = await _rateService.GetRatesForRoomAsync(id);
            var rateDTOs = rates.Select(rate => new RateDTO
            {
                RateId = rate.RateId,
                RateValue = rate.RateValue,
                RateDate = rate.RateDate,
                RoomId = rate.RoomId,
                AccountId = rate.AccountId,
                FullName = rate.Account.AccountFullname 
            });

            return Ok(rateDTOs);
        }

        // POST: api/Rate
        [HttpPost]
        public async Task<IActionResult> RateRoom([FromBody] RateDTO rateDTO)
        {
            ClaimsPrincipal claims = this.User;
            //var userID = "1ccb72fd-a14e-48ee-a4d6-496977e0cf5a";
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            var account = await _accountService.GetAccountByIdAsync(userID);
            var accountId = account.AccountId;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (rateDTO.RateValue == null)
            {
                rateDTO.RateValue = 1; // Default to minimum rating
            }

            rateDTO.AccountId = accountId;

            var rate = _mapper.Map<Rate>(rateDTO);
            await _rateService.CreateRateAsync(rate);
            return Ok(rate);
        }

        // POST: api/Rate/RateAndComment
        [HttpPost("RateAndComment")]
        public async Task<IActionResult> RateAndComment([FromBody] RateAndCommentDTO rateAndCommentDTO)
        {
            // Get the authenticated user
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userID))
            {
                return Unauthorized("User is not authenticated.");
            }

            // Fetch account details from the user ID
            var account = await _accountService.GetAccountByIdAsync(userID);
            if (account == null)
            {
                return Unauthorized("Account not found.");
            }

            int accountId = account.AccountId;

            // Validate the incoming DTO
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (rateAndCommentDTO.RateValue <= 0 || rateAndCommentDTO.RateValue > 5)
            {
                return BadRequest("Invalid Rate Value. It must be between 1 and 5.");
            }

            if (string.IsNullOrWhiteSpace(rateAndCommentDTO.CommentContent))
            {
                return BadRequest("Comment content cannot be empty.");
            }

            try
            {
                // Create the Rate entry
                var rateDTO = new RateDTO
                {
                    RoomId = rateAndCommentDTO.RoomId,
                    RateValue = rateAndCommentDTO.RateValue,
                    RateDate = DateTime.Now,
                    AccountId = accountId
                };

                var rate = _mapper.Map<Rate>(rateDTO);
                await _rateService.CreateRateAsync(rate);

                // Create the Comment entry
                var commentDTO = new CommentDTO
                {
                    RoomId = rateAndCommentDTO.RoomId,
                    CommentContent = rateAndCommentDTO.CommentContent,
                    CommentDate = DateTime.Now,
                    CommentLevel = 1,  // Regular comment
                    AccountId = accountId,
                    CommentReply = "0"  // No reply, main comment
                };

                var comment = _mapper.Map<Comment>(commentDTO);
                await _commentService.CreateCommentAsync(comment);

                return Ok(new { Rate = rate, Comment = comment });
            }
            catch (Exception ex)
            {
                // Log the exception and return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

    }
}
