using RefikHaber.Models;

namespace RefikHaber.Repostories
{
    public interface IHaberRepository : IRepository<Haber>
    {
        void Guncelle(Haber haber);
        void Kaydet();
    }
}
