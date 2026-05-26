public class Aphorism
{
    public string Id { get; set; }
    public string Text { get; set; }
    public string Author { get; set; }
    public string Source { get; set; }        // книга, фільм, народне...
    public AphorismCategory Category { get; set; }
    public string Theme { get; set; }         // кохання, мудрість, гумор...
    public List<string> Keywords { get; set; }
    public bool IsFavorite { get; set; }
}