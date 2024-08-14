
namespace BlogMaster.Model
{
    public class Blog
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime DateCreated { get; set; }
        public string Text { get; set; }

        internal static List<Blog> ToList()
        {
            throw new NotImplementedException();
        }
    }
}
