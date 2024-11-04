using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;

namespace TripEnjoy.Application.Interface.Comment
{
    public interface ICommentService
    {
        Task<IEnumerable<Domain.Models.Comment>> GetAllCommentAsync();
        Task<IEnumerable<Domain.Models.Comment>> GetCommentByRoomIdAsync(int roomId);
        Task<Domain.Models.Comment> CreateCommentAsync(Domain.Models.Comment comment);
        Task<IEnumerable<CommentResponse>> GetCommentAndReplyByRoomIdAsync(int roomId);
    }
}
