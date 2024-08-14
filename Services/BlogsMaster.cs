using BlogMaster.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace BlogMaster.Services
{
    public class BlogsMaster : IBlogMaster
    {
        private readonly string _filePath = "blogs.json";
        private readonly List<Blog> _blogs = new();
        public BlogsMaster()
        {
            var json = File.ReadAllText(_filePath);
            _blogs = JsonConvert.DeserializeObject<List<Blog>>(json) ?? new List<Blog>();
        }

        public List<Blog> GetAllBlogs(string searchTerm = null, string sortBy = "date", string direction = "desc")
        {
            var blogs =  _blogs.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                blogs = blogs.Where(b => b.Username.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                         b.Text.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            switch (sortBy.ToLower())
            {
                case "username":
                    blogs = direction == "desc" ? blogs.OrderByDescending(b => b.Username) : blogs.OrderBy(b => b.Username);
                    break;
                case "date":
                default:
                    blogs = direction == "desc" ? blogs.OrderByDescending(b => b.DateCreated) : blogs.OrderBy(b => b.DateCreated);
                    break;
            }

            return blogs.ToList();
        }

        public Blog GetBlogById(int id)
        {
            return _blogs.FirstOrDefault(b => b.Id == id);
        }

        public Blog AddOrUpdateBlog(Blog blog)
        {
            if (blog.Id == 0)
            {
                blog.Id = _blogs.Count > 0 ? _blogs.Max(b => b.Id) + 1 : 1;
                _blogs.Add(blog);
            }
            else
            {
                var index = _blogs.FindIndex(b => b.Id == blog.Id);
                if (index != -1)
                {
                    _blogs[index] = blog;
                }
            }

            SaveToFile();
            return blog;
        }

        public void RemoveBLog(int id)
        {
            var blog = _blogs.FirstOrDefault(b => b.Id == id);
            if (blog != null)
            {
                _blogs.Remove(blog);
                SaveToFile();
            }
        }

        private void SaveToFile()
        {
            var json = JsonConvert.SerializeObject(_blogs, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
    }
}
