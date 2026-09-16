using BlogService.Application.Interfaces.Repositories;
using BlogService.Domain.Entities;
using BlogService.Infrastructure.presistence;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace BlogService.Infrastructure.Repositories;

public class BlogRepository : IBlogRepository
{
    private readonly BlogDbContext _context;

    public BlogRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Blog>> GetAllAsync()
    {
        return await _context.Blogs
            .AsNoTracking()
            .OrderByDescending(b => b.PublishedDate)
            .ToListAsync();
    }

    public async Task<Blog?> GetByMatchNoAsync(int matchNo)
    {
        return await _context.Blogs
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.MatchNo == matchNo);
    }

    public async Task<Blog> AddAsync(Blog blog)
    {
        await _context.Blogs.AddAsync(blog);
        await _context.SaveChangesAsync();

        return blog;
    }

    public async Task<Blog> UpdateAsync(Blog blog)
    {
        _context.Blogs.Update(blog);
        await _context.SaveChangesAsync();

        return blog;
    }

    public async Task DeleteAsync(Blog blog)
    {
        _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Blogs.AnyAsync(b => b.Id == id);
    }
}