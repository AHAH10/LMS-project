using LMS_Project.Models;
using LMS_Project.Models.LMS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LMS_Project.Repositories
{
    public class SchedulesRepository : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Schedule> Schedules()
        {
            return db.Schedules;
        }

        public IEnumerable<Schedule> StudentSchedules(string studentUserId)
        {
            List<Schedule> schedules = new List<Schedule>();

            foreach (Schedule schedule in Schedules())
            {
                if (schedule.Students.Where(s => s.Id == studentUserId).Count() > 0)
                {
                    schedules.Add(schedule);
                }
            }

            return schedules;
        }

        public IEnumerable<Schedule> TeacherSchedules(string teacherUserId)
        {
            return Schedules().Where(s => s.Course.TeacherID == teacherUserId);
        }

        public Schedule Schedule(int? id)
        {
            return Schedules().FirstOrDefault(s => s.ID == id);
        }

        public void Add(Schedule schedule)
        {
            db.Schedules.Add(schedule);
            SaveChanges();
        }

        public void Edit(Schedule schedule)
        {
            db.Entry(schedule).State = EntityState.Modified;
            SaveChanges();
        }

        public void Delete(int id)
        {
            Schedule schedule = Schedule(id);

            if (schedule != null)
            {
                db.Schedules.Remove(schedule);
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