namespace WingedWords.Models
{
    public class Aphorism
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Text { get; set; } = "";
        public string Author { get; set; } = "";
        public string Source { get; set; } = "";
        public AphorismCategory Category { get; set; } = AphorismCategory.Aphorism;
        public string Theme { get; set; } = "";
        public List<string> Keywords { get; set; } = new();
        public bool IsFavorite { get; set; } = false;
    }
}