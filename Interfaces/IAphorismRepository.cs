public interface IAphorismRepository
{
    List<Aphorism> GetAll();
    void Save(List<Aphorism> aphorisms);
}