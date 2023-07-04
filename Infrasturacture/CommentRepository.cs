using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Data;
using Microsoft.Extensions.Options;
using Infrasturacture.Dapper;
using MySqlConnector;

namespace Infrasturacture
{
    public class CommentRepository : ICommentRepository
    {
        private readonly BlogDbContext db;


        public CommentRepository(BlogDbContext db)
        {
            this.db = db;

        }

        public async Task AddComment(Comment comment)
        {
            await db.AddAsync(comment);
            await db.SaveChangesAsync();
        }

        public async Task DeleteCommentById(Guid id)
        {
            var comment = await GetCommentById(id);
            db.Comments.Remove(comment);
            await db.SaveChangesAsync();

        }

        public async Task<IEnumerable<Comment>> GetAllComments()
        {
            return await db.Comments.ToListAsync();
        }

        public async Task<Comment> GetCommentById(Guid id)
        {
            var result = await db.Comments.SingleOrDefaultAsync(c => c.Id == id);
            if (result == null)
            {
                throw new Exception("通过id未找到评论");//应该使用自定义异常
            }
            return result;
        }

        public Task<IEnumerable<Comment>> GetCommentsByUserId(Guid id)
        {
            return (Task<IEnumerable<Comment>>)db.Comments.Where(c => c.CommentUser!.Id == id);//显示转换
        }

        public Task<IEnumerable<Comment>> GetCommentsByUserName(string name)
        {

            return (Task<IEnumerable<Comment>>)db.Comments.Where(c => c.UserName == name);//显示转换
        }

        public Task UpDateComment(Guid commentId)
        {
            throw new NotImplementedException();
        }
    }
}
