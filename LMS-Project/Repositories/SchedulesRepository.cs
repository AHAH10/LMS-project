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

            return schedules.OrderBy(s => s.BeginningTime)
                            .GroupBy(s => s.WeekDay)
                            .SelectMany(s => s);
        }

        public IEnumerable<Schedule> TeacherSchedules(string teacherUserId)
        {
            return Schedules().Where(s => s.Course.TeacherID == teacherUserId)
                              .OrderBy(s => s.BeginningTime)
                              .GroupBy(s => s.WeekDay)
                              .SelectMany(s => s);
        }

        /// <summary>
        /// Checks if the teacher is available at the given day and times
        /// </summary>
        /// <param name="teacherId">Teacher ID</param>
        /// <param name="weekDay">Week of the day</param>
        /// <param name="beginningTime">Beginning time</param>
        /// <param name="endingTime">Ending time</param>
        /// <returns>Returns NULL if the teacher is available, the nearest schedule otherwise</returns>
        public Schedule TeacherAvailability(string teacherId, WeekDays weekDay, string beginningTime, string endingTime)
        {
            return Schedules().Where(s => s.Course.TeacherID == teacherId &&
                                          s.WeekDay == weekDay)
                              .OrderBy(s => s.BeginningTime)
                              .FirstOrDefault(s => (string.Compare(s.BeginningTime, beginningTime) == -1 && string.Compare(s.EndingTime, beginningTime) == 1) ||
                                                   (string.Compare(s.BeginningTime, endingTime) == -1 && string.Compare(s.EndingTime, endingTime) == 1));
        }

        /// <summary>
        /// Checks if the student is available at the given day and times
        /// </summary>
        /// <param name="studentId">Student ID</param>
        /// <param name="weekDay">Week of the day</param>
        /// <param name="beginningTime">Beginning time</param>
        /// <param name="endingTime">Ending time</param>
        /// <returns>Returns NULL if the student is available, the nearest schedule otherwise</returns>
        public Schedule StudentAvailability(string studentId, WeekDays weekDay, string beginningTime, string endingTime)
        {
            return Schedules().Where(s => s.Students.Join(db.LMSUsers,
                                                          schStudent => schStudent.Id,
                                                          student => student.Id,
                                                          (schStudent, student) => student)
                              .Any(u => u.Id.Equals(studentId)))
                              .OrderBy(s => s.BeginningTime)
                              .FirstOrDefault(s => (string.Compare(s.BeginningTime, beginningTime) == -1 && string.Compare(s.EndingTime, beginningTime) == 1 ||
                                                   (string.Compare(s.BeginningTime, endingTime) == -1 && string.Compare(s.EndingTime, endingTime) == 1)));
        }

        /// <summary>
        /// Checks if the classroom is available at the given day and times
        /// </summary>
        /// <param name="classroomId">Classroom ID</param>
        /// <param name="weekDay">Week of the day</param>
        /// <param name="beginningTime">Beginning time</param>
        /// <param name="endingTime">Ending time</param>
        /// <returns>Returns NULL if the classroom is available, the nearest schedule otherwise</returns>
        public Schedule ClassroomAvailability(int classroomId, WeekDays weekDay, string beginningTime, string endingTime)
        {
            return Schedules().Where(s => s.ClassroomID == classroomId &&
                                          s.WeekDay == weekDay)
                              .OrderBy(s => s.BeginningTime)
                              .FirstOrDefault(s => (string.Compare(s.BeginningTime, beginningTime) == -1 && string.Compare(s.EndingTime, beginningTime) == 1) ||
                                                   (string.Compare(s.BeginningTime, endingTime) == -1 && string.Compare(s.EndingTime, endingTime) == 1));
        }

        public Schedule Schedule(int? id)
        {
            return Schedules().FirstOrDefault(s => s.ID == id);
        }

        public void Add(Schedule schedule, List<string> students)
        {
            List<User> studentsInLesson = db.LMSUsers.Where(u => students.Contains(u.Id)).Select(u => u).ToList();

            schedule.Students = studentsInLesson;

            db.Schedules.Add(schedule);

            foreach (User student in studentsInLesson)
            {
                if (student.Schedules == null)
                    student.Schedules = new List<Schedule> { schedule };
                else
                    student.Schedules.Add(schedule);

                db.Entry(student).State = EntityState.Modified;
            }

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