using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}   
