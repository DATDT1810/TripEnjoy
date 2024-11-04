using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface.Comment;

namespace TripEnjoy.Application.Services.Comment
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<Domain.Models.Comment> CreateCommentAsync(Domain.Models.Comment comment)
        {
            return await _commentRepository.CreateCommentAsync(comment);
        }

        public async Task<IEnumerable<Domain.Models.Comment>> GetAllCommentAsync()
        {
            return await _commentRepository.GetAllCommentAsync();
        }

        public async Task<IEnumerable<Domain.Models.Comment>> GetCommentByRoomIdAsync(int roomId)
        {
            return await _commentRepository.GetCommentByRoomIdAsync(roomId);
        }

        // lấy tất cả comment và rep của phòng
        // bổ sung phương thức
        public async Task<IEnumerable<CommentResponse>> GetCommentAndReplyByRoomIdAsync(int roomId)
        {
            var commentRepo =  await _commentRepository.GetCommentByRoomIdAsync(roomId);
            if(commentRepo == null)
            {
                return new List<CommentResponse>();
            }
            var commentResponse = new List<CommentResponse>();
            foreach (var comment in commentRepo)
            {
                var commentRes = new CommentResponse
                {
                  CommentId =  comment.CommentId,
                    AccountId = comment.AccountId,
                    RoomId = comment.RoomId,
                    CommentContent = comment.CommentContent,
                    CommentLevel = comment.CommentLevel,
                    CommentReply = comment.CommentReply,
                    CommentDate = comment.CommentDate,
                    AccountName = comment.Account.AccountFullname,
                    AccountImage = comment.Account.AccountImage
                };
                commentResponse.Add(commentRes);
            }
            return commentResponse;
        }
    }
}

