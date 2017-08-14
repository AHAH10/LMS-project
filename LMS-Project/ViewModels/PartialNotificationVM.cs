using LMS_Project.Models.LMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS_Project.ViewModels
{
    public class PartialNotificationVM
    {
        public int ID { get; set; }
        
        public string Grade { get; set; }
        public string Comment { get; set; }

        public Course Course { get; set; }
        public Document Document { get; set; }

        public string SendingDate { get; set; }
        public DateTime? ReadingDate { get; set; }
    }
}