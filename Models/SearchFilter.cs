namespace WingedWords.Models
{
    public class SearchFilter
    {
        public string Keyword { get; set; } = "";
        public string Author { get; set; } = "";
        public string Theme { get; set; } = "";
        public AphorismCategory? Category { get; set; } = null;
        public bool OnlyFavorites { get; set; } = false;

        // Перевіряє чи фільтр взагалі активний
        public bool IsEmpty() =>
            string.IsNullOrWhiteSpace(Keyword) &&
            string.IsNullOrWhiteSpace(Author) &&
            string.IsNullOrWhiteSpace(Theme) &&
            Category == null &&
            !OnlyFavorites;
    }
}