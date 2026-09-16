using BlogService.Application.DTOs;

namespace BlogService.Application.Interfaces.Services;

public interface IBlogService
{
    Task<List<BlogDto>> GetAllBlogsAsync();

    Task<BlogDto?> GetBlogByMatchNoAsync(int matchNo);

    Task<BlogDto> CreateBlogAsync(CreateBlogDto createBlogDto);

    Task<BlogDto?> UpdateBlogAsync(int matchNo, UpdateBlogDto updateBlogDto);

    Task<bool> DeleteBlogAsync(int matchNo);
}