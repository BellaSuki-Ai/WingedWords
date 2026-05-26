using WingedWords.Models;

namespace WingedWords.Interfaces
{
    public interface IAphorismService
    {
        List<Aphorism> GetAll();
        List<Aphorism> Search(SearchFilter filter);
        Aphorism? GetById(string id);
        void Add(Aphorism aphorism);
        void Update(Aphorism aphorism);
        void Delete(string id);
        void ToggleFavorite(string id);
        List<string> GetAllAuthors();
        List<string> GetAllThemes();
    }
}