using LMS_Project.Models.LMS;
using System.Collections.Generic;

namespace LMS_Project.ViewModels
{
    public class MyDocumentsVM
    {
        public IEnumerable<Document> Documents { get; set; }
        public bool ShowGrades { get; set; }
    }
}