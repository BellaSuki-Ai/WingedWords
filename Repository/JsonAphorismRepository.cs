using System.Text.Json;
using WingedWords.Interfaces;
using WingedWords.Models;

namespace WingedWords.Repository
{
    public class JsonAphorismRepository : IAphorismRepository
    {
        // Шлях до файлу поряд з exe
        private readonly string _filePath = "aphorisms.json";

        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,           // відступ у файлі
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        };

        public List<Aphorism> GetAll()
        {
            if (!File.Exists(_filePath))
                return new List<Aphorism>();

            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Aphorism>>(json, _options)
                       ?? new List<Aphorism>();
            }
            catch
            {
                // Якщо файл пошкоджений  повертаємо порожній список
                return new List<Aphorism>();
            }
        }

        public void Save(List<Aphorism> aphorisms)
        {
            string json = JsonSerializer.Serialize(aphorisms, _options);
            File.WriteAllText(_filePath, json);
        }
    }
}