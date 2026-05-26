using WingedWords.Interfaces;
using WingedWords.Models;

namespace WingedWords.Services
{
    public class AphorismService : IAphorismService
    {
        private readonly IAphorismRepository _repository;
        private List<Aphorism> _cache; // Кеш щоб не читати файл щоразу

        public AphorismService(IAphorismRepository repository)
        {
            _repository = repository;
            _cache = _repository.GetAll();
        }

        public List<Aphorism> GetAll() => _cache;

        public Aphorism? GetById(string id) =>
            _cache.FirstOrDefault(a => a.Id == id);

        // --- CRUD ---

        public void Add(Aphorism aphorism)
        {
            _cache.Add(aphorism);
            _repository.Save(_cache);
        }

        public void Update(Aphorism aphorism)
        {
            int index = _cache.FindIndex(a => a.Id == aphorism.Id);
            if (index == -1) return;

            _cache[index] = aphorism;
            _repository.Save(_cache);
        }

        public void Delete(string id)
        {
            _cache.RemoveAll(a => a.Id == id);
            _repository.Save(_cache);
        }

        public void ToggleFavorite(string id)
        {
            var aphorism = GetById(id);
            if (aphorism == null) return;

            aphorism.IsFavorite = !aphorism.IsFavorite;
            _repository.Save(_cache);
        }

        // --- Пошук ---

        public List<Aphorism> Search(SearchFilter filter)
        {
            if (filter.IsEmpty()) return _cache;

            return _cache.Where(a =>
            {
                // Пошук по ключовому слову (в тексті, авторі, ключових словах)
                bool matchKeyword = string.IsNullOrWhiteSpace(filter.Keyword) ||
                    a.Text.Contains(filter.Keyword, StringComparison.OrdinalIgnoreCase) ||
                    a.Author.Contains(filter.Keyword, StringComparison.OrdinalIgnoreCase) ||
                    a.Keywords.Any(k => k.Contains(filter.Keyword, StringComparison.OrdinalIgnoreCase));

                bool matchAuthor = string.IsNullOrWhiteSpace(filter.Author) ||
                    a.Author.Contains(filter.Author, StringComparison.OrdinalIgnoreCase);

                bool matchTheme = string.IsNullOrWhiteSpace(filter.Theme) ||
                    a.Theme.Contains(filter.Theme, StringComparison.OrdinalIgnoreCase);

                bool matchCategory = filter.Category == null ||
                    a.Category == filter.Category;

                bool matchFavorite = !filter.OnlyFavorites || a.IsFavorite;

                return matchKeyword && matchAuthor && matchTheme && matchCategory && matchFavorite;
            }).ToList();
        }

        // --- Допоміжні методи для заповнення фільтрів ---

        public List<string> GetAllAuthors() =>
            _cache.Select(a => a.Author)
                  .Where(a => !string.IsNullOrWhiteSpace(a))
                  .Distinct()
                  .OrderBy(a => a)
                  .ToList();

        public List<string> GetAllThemes() =>
            _cache.Select(a => a.Theme)
                  .Where(t => !string.IsNullOrWhiteSpace(t))
                  .Distinct()
                  .OrderBy(t => t)
                  .ToList();
    }
}