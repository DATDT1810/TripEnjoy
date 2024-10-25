    using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.Comment;
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;

namespace TripEnjoy.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<IEnumerable<Domain.Models.Comment>> GetCommentByRoomIdAsync(int roomId)
        {
            return await _context.Comments
           .Where(c => c.RoomId == roomId)
           .OrderBy(c => c.CommentDate) 
           .ToListAsync();
        }
    }
}
