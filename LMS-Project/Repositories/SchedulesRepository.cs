using LMS_Project.Models;
using LMS_Project.Models.LMS;
using LMS_Project.ViewModels;
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

            return schedules.OrderBy(s => s.WeekDay).ThenBy(s => s.BeginningTime);
        }

        public IEnumerable<Schedule> TeacherSchedules(string teacherUserId)
        {
            return Schedules().Where(s => s.Course.TeacherID == teacherUserId)
                              .OrderBy(s => s.WeekDay)
                              .ThenBy(s => s.BeginningTime);
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
                              .FirstOrDefault(s => (string.Compare(s.BeginningTime, beginningTime) != 1 && string.Compare(s.EndingTime, beginningTime) != -1) ||
                                                   (string.Compare(s.BeginningTime, endingTime) != 1 && string.Compare(s.EndingTime, endingTime) != -1));
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
                              .Any(student => student.Id.Equals(studentId)))
                              .OrderBy(s => s.BeginningTime)
                              .FirstOrDefault(s => s.WeekDay == weekDay &&
                                                   (string.Compare(s.BeginningTime, beginningTime) != 1 && string.Compare(s.EndingTime, beginningTime) != -1 ||
                                                   (string.Compare(s.BeginningTime, endingTime) != 1 && string.Compare(s.EndingTime, endingTime) != -1)));
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
                              .FirstOrDefault(s => (string.Compare(s.BeginningTime, beginningTime) != 1 && string.Compare(s.EndingTime, beginningTime) != -1) ||
                                                   (string.Compare(s.BeginningTime, endingTime) != 1 && string.Compare(s.EndingTime, endingTime) != -1));
        }

        public Schedule Schedule(int? id)
        {
            return Schedules().FirstOrDefault(s => s.ID == id);
        }

        private List<User> StudentsInLesson(List<string> studentsInLesson)
        {
            return db.LMSUsers.Where(u => studentsInLesson.Contains(u.Id)).Select(u => u).ToList();
        }

        /// <summary>
        /// Indicates if the student actually takes part or not to the course related to the schedule
        /// </summary>
        /// <param name="studentId">Student ID</param>
        /// <param name="id">Schedule ID</param>
        /// <returns></returns>
        internal bool TakesPart(string studentId, int id)
        {
            return Schedules().Where(s => s.Students.Join(db.LMSUsers,
                                                          schStudent => schStudent.Id,
                                                          student => student.Id,
                                                          (schStudent, student) => student)
                              .Any(student => student.Id.Equals(studentId))).Count() > 0;
        }

        /// <summary>
        /// Indicates if the teacher is in charge or not of the course related to the schedule
        /// </summary>
        /// <param name="teacherId">Teacher ID</param>
        /// <param name="id">Schedule ID</param>
        /// <returns></returns>
        internal bool IsInCharge(string teacherId, int id)
        {
            Schedule schedule = Schedule(id);

            if (schedule == null)
                return false;

            return schedule.Course.TeacherID == teacherId;
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

        public void Edit(Schedule schedule, List<string> students)
        {
            // Remove the previous list of students
            List<User> previousStudents = db.LMSUsers.Where(u => u.Schedules.Join(db.Schedules,
                                                                                  s => s.ID,
                                                                                  sch => sch.ID,
                                                                                  (s, sch) => s)
                                                     .Any(s => s.ID == schedule.ID))
                                                     .ToList();

            foreach (User student in previousStudents)
            {
                student.Schedules.Remove(schedule);
                db.Entry(student).State = EntityState.Modified;
            }

            List<User> studentsInLesson = StudentsInLesson(students);

            schedule.Students = studentsInLesson;
            db.Entry(schedule).State = EntityState.Modified;

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