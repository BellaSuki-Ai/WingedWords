public class SearchFilter
{
    public string Keyword { get; set; }
    public string Author { get; set; }
    public string Theme { get; set; }
    public AphorismCategory? Category { get; set; }
    public bool OnlyFavorites { get; set; }
}