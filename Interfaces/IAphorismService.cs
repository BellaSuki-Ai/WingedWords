public interface IAphorismService
{
    List<Aphorism> GetAll();
    List<Aphorism> Search(SearchFilter filter);
    void Add(Aphorism aphorism);
    void Update(Aphorism aphorism);
    void Delete(string id);
    void ToggleFavorite(string id);
}