using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using LMS_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LMS_Project.Controllers
{
    public class NotificationsAPIController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<PartialNotificationVM> GetAllNotifications(string userID)
        {
            List<PartialNotificationVM> _notifications = new List<PartialNotificationVM>();

            foreach(Notification n in new NotificationRepository().Notifications(userID))
            {
                PartialNotificationVM notification = new PartialNotificationVM {
                    ID =n.ID,
                    SendingDate =n.SendingDate,
                    ReadingDate =n.ReadingDate,
                    Course =new Course{
                        Teacher = new User {
                            FirstName = n.Grade.Document.Course.Teacher.FirstName,
                            LastName = n.Grade.Document.Course.Teacher.LastName
                        },
                        Subject =new Subject {
                            Name =n.Grade.Document.Course.Subject.Name
                        },
                    },
                    Document=new Document{
                        DocumentName=n.Grade.Document.DocumentName,
                        ContentType=n.Grade.Document.ContentType
                    },
                    Grade=Enum.GetName(typeof(AssignmentGrade),n.Grade.AGrade),
                    Comment=n.Grade.Comment
                };
                _notifications.Add(notification);
            }
            return _notifications;
        }
        public IEnumerable<PartialNotificationVM> GetUnreadNotifications(string userID)
        {
            List<PartialNotificationVM> _notifications = new List<PartialNotificationVM>();

            foreach (Notification n in new NotificationRepository().UnreadNotifications(userID))
            {
                PartialNotificationVM notification = new PartialNotificationVM
                {
                    ID = n.ID,
                    SendingDate = n.SendingDate,
                    ReadingDate = n.ReadingDate,
                    Course = new Course
                    {
                        Teacher = new User
                        {
                            FirstName = n.Grade.Document.Course.Teacher.FirstName,
                            LastName = n.Grade.Document.Course.Teacher.LastName
                        },
                        Subject = new Subject
                        {
                            Name = n.Grade.Document.Course.Subject.Name
                        },
                    },
                    Document = new Document
                    {
                        DocumentName = n.Grade.Document.DocumentName,
                        ContentType = n.Grade.Document.ContentType
                    },
                    Grade = Enum.GetName(typeof(AssignmentGrade), n.Grade.AGrade),
                    Comment = n.Grade.Comment
                };
                _notifications.Add(notification);
            }
            return _notifications;
        }
        [HttpPost]
        public void SetNotificationAsRead(int notificationID)
        {
            new NotificationRepository().NotificationRead(notificationID);
        }
        [HttpPost]
        public void SetAllNotificationsAsReaded(string id)
        {
            foreach(Notification n in new NotificationRepository().Notifications(id))
            {
                new NotificationRepository().NotificationRead(n.ID);
            }
        }
    }
}