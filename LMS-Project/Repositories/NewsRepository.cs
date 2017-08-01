using LMS_Project.Models;
using LMS_Project.Models.LMS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LMS_Project.Repositories
{
    public class NewsRepository : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<News> News()
        {
            return db.News;
        }

        public News GetSpecificNews(int? id)
        {
            return News().FirstOrDefault(n => n.ID == id);
        }

        public void Add(News news)
        {
            db.News.Add(news);
            SaveChanges();
        }

        public void Edit(News news)
        {
            db.Entry(news).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(int? id)
        {
            News news = GetSpecificNews(id);

            if (news != null)
            {
                db.News.Remove(news);
                SaveChanges();
            }
        }

        private void SaveChanges()
        {
            db.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                db.Dispose();
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}