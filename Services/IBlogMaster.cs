using BlogMaster.Model;

namespace BlogMaster.Services
{
    public interface IBlogMaster
    {
        List<Blog> GetAllBlogs(string searchTerm = null, string sortBy = "date", string direction = "desc");
        Blog GetBlogById(int id);
        Blog AddOrUpdateBlog(Blog blog);
        void RemoveBLog(int id);
    }
}
