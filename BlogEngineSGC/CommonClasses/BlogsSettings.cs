namespace BlogEngineSGC.CommonClasses
{
    public class BlogsSettings
    {
        public int CommentsCloseAfterDays { get; set; } = 10;

        public bool DisplayComments { get; set; } = true;

        public PostsListView ListView { get; set; } = PostsListView.TitlesAndExcerpts;

        public string Owner { get; set; } = "The Owner";

        public int PostsPerPage { get; set; } = 4;
    }
}
