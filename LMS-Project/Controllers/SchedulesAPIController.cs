using LMS_Project.Models;
using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using LMS_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace LMS_Project.Controllers
{
    public class SchedulesAPIController : ApiController
    {
        private SchedulesRepository repository = new SchedulesRepository();

        public List<EnumWeekDayVM> GetWeekDays()
        {
            List<EnumWeekDayVM> result = new List<EnumWeekDayVM>();

            foreach (WeekDays weekDay in Enum.GetValues(typeof(WeekDays)))
            {
                result.Add(new EnumWeekDayVM { Key = weekDay, Value = weekDay.ToString() });
            }

            return result;
        }

        public List<PartialScheduleVM> Get()
        {
            return repository.Schedules().Select(s => new PartialScheduleVM
            {
                ID = s.ID,
                Classroom = s.Classroom.Name + (s.Classroom.Remarks == null ? "" : " - " + s.Classroom.Remarks),
                SubjectName = s.Course.Subject.Name,
                TeacherName = s.Course.Teacher.ToString(),
                WeekDay = s.WeekDay.ToString(),
                BeginningTime = s.BeginningTime,
                EndingTime = s.EndingTime,
                IsDeletable = s.Course.Documents.Count == 0
            }).ToList();
        }

        /// <summary>
        /// Create a new CreateScheduleVM object, related to the Schedule that matches the ID
        /// </summary>
        /// <param name="id">ID of the Schedule to be replicated</param>
        /// <returns></returns>
        /// <remarks>Returns an empty CreateScheduleVM object if any error occurs</remarks>
        public CreateEditScheduleVM Get(int? id)
        {
            if (id == null)
                return new CreateEditScheduleVM();

            Schedule schedule = repository.Schedule(id);

            if (schedule == null)
                return new CreateEditScheduleVM();

            return new CreateEditScheduleVM
            {
                WeekDay = schedule.WeekDay,
                BeginningTime = new DateTime() + DecodeTime(schedule.BeginningTime),
                EndingTime = new DateTime() + DecodeTime(schedule.EndingTime),
                CourseID = schedule.CourseID,
                ClassroomID = schedule.ClassroomID,
                Students = schedule.Students.Select(s => s.Id).ToArray()
            };
        }

        public bool IsDeletable(int? id)
        {
            if (id == null)
                return false;

            Schedule schedule = repository.Schedule(id);
            if (schedule == null)
                return false;

            return schedule.Course.Documents.Count == 0;
        }

        // POST: api/SchedulesAPI
        public HttpResponseMessage Post([FromBody]CreateEditScheduleVM model)
        {
            HttpResponseMessage response = ValidateModel(model);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                db.Schedules.Add(CopySchedule(model, db));
                db.SaveChanges();
            }

            return response;
        }

        // PUT: api/SchedulesAPI/5
        public HttpResponseMessage Put([FromBody]CreateEditScheduleVM model)
        {
            HttpResponseMessage response = ValidateModel(model);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                db.Entry(CopySchedule(model, db, model.ID)).State = EntityState.Modified;
                db.SaveChanges();
            }

            return response;
        }

        private HttpResponseMessage ValidateModel(CreateEditScheduleVM model)
        {
            /// Some basic validity checks ///
            // - Is the beginning time lower than the ending time ? - //
            if (model.BeginningTime >= model.EndingTime)
            {
                HttpError err = new HttpError("The ending time must be greater than the beginning time.");
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, err);
            }

            // - At least one student must take part to the lesson - //
            if (model.Students.Length == 0)
            {
                HttpError err = new HttpError("You must select at least one student for that lesson.");
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, err);
            }

            // - Are all elements to create the schedule actually available at the given date and times? - //
            List<string> availability = TestAvailability(model);

            if (availability.Count > 0)
                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable,
                                                   new HttpError(string.Join(@"\\", availability)));

            // - Is the classroom big enough for all the students? - //
            Classroom classroom = new ClassroomsRepository().Classroom(model.ClassroomID);
            int nbStudents = model.Students.Where(s => s != null).Count();
            if (classroom.AmountStudentsMax < nbStudents)
                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable,
                                                   new HttpError("The selected classroom is too small for the amount of users.\\" +
                                                                 classroom.AmountStudentsMax.ToString() + " places for " + nbStudents.ToString() + " students."));

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private List<string> TestAvailability(CreateEditScheduleVM schedule)
        {
            // Let's check if all data are available at the given day and times, starting with the teacher
            Course course = new CoursesRepository().Course(schedule.CourseID);

            if (course == null)
            {
                return new List<string> { "The selected course is invalid." };
            }

            Schedule availability = repository.TeacherAvailability(course.TeacherID,
                                                                   schedule.WeekDay,
                                                                   schedule.BeginningTime.ToString(ScheduleConstants.TIME_FORMAT),
                                                                   schedule.EndingTime.ToString(ScheduleConstants.TIME_FORMAT));

            if (availability != null && availability.ID != schedule.ID)
            {
                return new List<string> { course.Teacher.ToString() + " is not available at the given day and times:",
                                          availability.ToString() };
            }

            // Then, we can check if the room is available
            Classroom classroom = new ClassroomsRepository().Classroom(schedule.ClassroomID);

            if (classroom == null)
            {
                return new List<string> { "The selected classroom is invalid." };
            }

            availability = repository.ClassroomAvailability(schedule.ClassroomID,
                                                            schedule.WeekDay,
                                                            schedule.BeginningTime.ToString(ScheduleConstants.TIME_FORMAT),
                                                            schedule.EndingTime.ToString(ScheduleConstants.TIME_FORMAT));

            if (availability != null && availability.ID != schedule.ID)
            {
                return new List<string> { "The classroom " + classroom.Name + " is not available at the given day and times:",
                                          availability.ToString() };
            }

            // Finally, let's check if all the students are available for the lesson
            foreach (string studentId in schedule.Students)
            {
                availability = repository.StudentAvailability(studentId,
                                                              schedule.WeekDay,
                                                              schedule.BeginningTime.ToString(ScheduleConstants.TIME_FORMAT),
                                                              schedule.EndingTime.ToString(ScheduleConstants.TIME_FORMAT));

                if (availability != null && availability.ID != schedule.ID)
                {
                    return new List<string> { new UsersRepository().UserById(studentId).ToString() + " is not available at the given day and times:",
                                              availability.ToString() };
                }
            }

            return new List<string>();
        }

        private Schedule CopySchedule(CreateEditScheduleVM model, ApplicationDbContext db, int? scheduleId = null)
        {
            Schedule schedule = null;

            if (scheduleId == null)
                schedule = new Schedule { Students = new List<User>() };
            else
            {
                schedule = db.Schedules.FirstOrDefault(s => s.ID == scheduleId);

                // Delation of all students who "disappeared" from the previous schedule
                foreach (User student in schedule.Students.Where(s => !model.Students.Contains(s.Id)))
                {
                    student.Schedules.Remove(schedule);
                }
            }

            schedule.WeekDay = model.WeekDay;
            schedule.BeginningTime = model.BeginningTime.ToString(ScheduleConstants.TIME_FORMAT);
            schedule.EndingTime = model.EndingTime.ToString(ScheduleConstants.TIME_FORMAT);
            schedule.Course = db.Courses.FirstOrDefault(c => c.ID == model.CourseID);
            schedule.Classroom = db.Classrooms.FirstOrDefault(c => c.ID == model.ClassroomID);

            // Filter on the students to be added to the new version of the schedule
            List<string> studentsId = schedule.Students.Select(s => s.Id).ToList();
            model.Students = model.Students.Where(s => s != null && !studentsId.Contains(s)).ToArray();

            foreach (string studentId in model.Students)
                schedule.Students.Add(db.LMSUsers.FirstOrDefault(u => u.Id == studentId));

            return schedule;
        }

        /// <summary>
        /// Transforms a string in format "HH:mm" into a TimeSpan
        /// </summary>
        /// <param name="time">String to be parsed</param>
        /// <returns></returns>
        private TimeSpan DecodeTime(string time)
        {
            try
            {
                // Checks that the string actually matches the "HH:mm" format
                Regex rgx = new Regex(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");

                if (string.IsNullOrEmpty(time) || !rgx.IsMatch(time))
                    return new TimeSpan();

                string[] split = time.Split(new string[] { ":" }, StringSplitOptions.None);

                return new TimeSpan(int.Parse(split[0]), int.Parse(split[1]), 0);
            }
            catch
            {
                return new TimeSpan();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
