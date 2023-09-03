namespace JustBlog.MVC.Models
{
    public class PostQuery
    {
        public bool Published { get; set; }
        public int LatestPostSize { get; set; }
        public DateTime YearMonth { get; set; }
        public string? UrlSlug { get; set; }
        public string? Category { get; set; }
        public string? Tag { get; set; }
    }
}
