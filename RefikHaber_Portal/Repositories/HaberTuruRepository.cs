﻿using RefikHaber.Utility;

namespace RefikHaber.Repostories
{
    public class HaberTuruRepository : Repository<HaberTuru>, IHaberTuruRepository
    {
        private UygulamaDbContext _uygulamaDbContext;

        public HaberTuruRepository(UygulamaDbContext uygulamaDbContext) : base(uygulamaDbContext)
        {
            _uygulamaDbContext = uygulamaDbContext;
        }

        public void Guncelle(HaberTuru haberTuru)
        {
            _uygulamaDbContext.Update(haberTuru);
        }

        public void Kaydet()
        {
            _uygulamaDbContext.SaveChanges();
        }
    }
}
