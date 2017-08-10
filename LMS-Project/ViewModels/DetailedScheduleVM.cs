using LMS_Project.Models.LMS;
using System.Collections.Generic;

namespace LMS_Project.ViewModels
{
    public class DetailedScheduleVM
    {
        public Schedule Schedule { get; set; }
        public List<Document> Documents { get; set; }
    }
}