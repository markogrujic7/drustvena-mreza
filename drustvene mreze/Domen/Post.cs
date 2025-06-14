namespace drustvene_mreze.Domen
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }

        public int? UserId { get; set; }
        public string? UserName { get; set; }
    }
}
