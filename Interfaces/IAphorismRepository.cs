using WingedWords.Models;

namespace WingedWords.Interfaces
{
    public interface IAphorismRepository
    {
        List<Aphorism> GetAll();
        void Save(List<Aphorism> aphorisms);
    }
}